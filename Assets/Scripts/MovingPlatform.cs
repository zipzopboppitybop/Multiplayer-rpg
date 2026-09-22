using Fusion;
using UnityEngine;

public class MovingPlatform : NetworkBehaviour
{
    [SerializeField] private Transform[] stops;
    [SerializeField] private float speed;
    [Networked] private int currentStop { get; set; }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;

        MovePlatform();
    }

    private void MovePlatform()
    {
        if (stops.Length == 0) return;

        Vector3 target = stops[currentStop].position;
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Runner.DeltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            currentStop = (currentStop + 1) % stops.Length;
        }
    }
}
