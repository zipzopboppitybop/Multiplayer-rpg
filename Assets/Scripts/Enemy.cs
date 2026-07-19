using Fusion;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    [Networked] private NetworkObject networkedTarget { get; set; }
    [Networked] private Vector3 targetLastLocation { get; set; }
    [Networked] private NetworkBool hasLastKnownLocation { get; set; }

    [SerializeField] EnemyData data;

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;

        CheckForPlayers();

        float step = data.speed * Runner.DeltaTime;

        if (networkedTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, networkedTarget.transform.position, step);
        }
        else if (hasLastKnownLocation)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetLastLocation, step);

            if (Vector3.Distance(transform.position, targetLastLocation) < 0.1f)
            {
                hasLastKnownLocation = false;
            }
        }
    }

    private void CheckForPlayers()
    {
        if (networkedTarget != null)
        {
            float distance = Vector3.Distance(transform.position, networkedTarget.transform.position);
            if (distance > data.detectionRadius)
            {
                targetLastLocation = networkedTarget.transform.position;
                hasLastKnownLocation = true;
                networkedTarget = null;
                Debug.Log("Player has left my sight!");
            }
        }
        else
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, data.detectionRadius);
            foreach (var hit in hitColliders)
            {
                if (hit.CompareTag("Player") && hit.TryGetComponent<NetworkObject>(out var playerNetObj))
                {
                    networkedTarget = playerNetObj;
                    Debug.Log($"[DETECTED] Target set to: {playerNetObj.Id}");
                    break;
                }
            }
        }
    }
}