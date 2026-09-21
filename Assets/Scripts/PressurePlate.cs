using Fusion;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;

public class PressurePlate : NetworkBehaviour
{
    [Networked] private NetworkObject networkedTarget { get; set; }
    [Networked] public NetworkBool isSteppedOn { get; set; }
    private Vector3 originalHeight;
    private Vector3 newHeight;

    public override void Spawned()
    {
        originalHeight = transform.position;
        newHeight = new Vector3(transform.position.x, -1, transform.position.z);
    }
    public override void FixedUpdateNetwork()
    {
        UnityEngine.Debug.Log($"[{Object.Id}] auth={Object.HasStateAuthority} rot={transform.rotation.eulerAngles.y}");
        if (!Object.HasStateAuthority) return;

        CheckForPlayers();

        if (networkedTarget != null)
        {
            isSteppedOn = true;
            transform.position = newHeight;
        }
        else 
        {
            isSteppedOn = false;
            transform.position = originalHeight;
        }
    }

    private void CheckForPlayers()
    {
        if (networkedTarget != null)
        {
            float distance = Vector3.Distance(transform.position, networkedTarget.transform.position);
            if (distance > 2)
            {
                networkedTarget = null;
            }
        }
        else
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2);
            foreach (var hit in hitColliders)
            {
                if (hit.CompareTag("Player") && hit.TryGetComponent<NetworkObject>(out var playerNetObj))
                {
                    UnityEngine.Debug.Log($"Plate detected player: {playerNetObj.Id}, authority: {Object.HasStateAuthority}");
                    networkedTarget = playerNetObj;
                    break;
                }
            }
        }
    }
}
