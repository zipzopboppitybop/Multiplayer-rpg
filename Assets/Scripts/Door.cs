using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class Door : NetworkBehaviour
{
    [SerializeField] private PressurePlate pressurePlate;
    [SerializeField] private Lever lever;
    public bool isDoor;
    private Quaternion originalRotation;
    private Quaternion originalTransform;

    [Networked] private float currentOpenAngle { get; set; }
    private const float maxOpenAngle = 90f;
    private const float openSpeed = 220f;

    private float currentOpenHeight;
    private const float maxOpenHeight = 90f;
    private const float raiseSpeed = 220f;

    public override void Spawned()
    {
        if (isDoor)
        {
            originalRotation = transform.rotation;
        }
    }
    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;

        if (lever.flipped)
        {
            OpenDoor();
            Debug.Log("I will keep the door open");
            return;
        }

        if (!pressurePlate.isSteppedOn)
        {
            if (isDoor)
            {
                ResetDoor();
            }
        }
        else
        {
            if (isDoor)
            {
                OpenDoor();
            }
        }
    }

    private void ResetDoor()
    {
        if (currentOpenAngle > 0)
        {
            currentOpenAngle -= openSpeed * Runner.DeltaTime;
            currentOpenAngle = Mathf.Max(currentOpenAngle, 0f);;

            transform.rotation = originalRotation * Quaternion.Euler(0f, currentOpenAngle, 0f);
        }
    }

    private void OpenDoor()
    {
        if (currentOpenAngle < maxOpenAngle)
        {
            currentOpenAngle += openSpeed * Runner.DeltaTime;
            currentOpenAngle = Mathf.Min(currentOpenAngle, maxOpenAngle);

            transform.rotation = originalRotation * Quaternion.Euler(0f, currentOpenAngle, 0f);
        }
    }
}
