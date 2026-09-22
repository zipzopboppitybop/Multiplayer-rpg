using Fusion;
using UnityEngine;

public class MovingPlatform : NetworkBehaviour
{
    [SerializeField] private float maxZ;
    [SerializeField] private float minZ;
    [SerializeField] private float maxX;
    [SerializeField] private float minX;
    [SerializeField] private float speed;
    [Networked] private float currentZ { get; set; }
    [Networked] private float currentX { get; set; }
    private Vector3 originalPosition;

    public override void Spawned()
    {
        originalPosition = transform.position;
        currentZ = originalPosition.z;
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;

        MoveDown();
    }

    private void MoveDown()
    {
        if (transform.position.z > minZ)
        {
            currentZ -= speed * Runner.DeltaTime;
            currentZ = Mathf.Max(currentZ, minZ); ;

            transform.position = new Vector3(originalPosition.x, originalPosition.y, currentZ);
        }
    }
}
