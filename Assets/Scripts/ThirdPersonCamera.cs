using Fusion;
using UnityEngine;
public class ThirdPersonCamera : MonoBehaviour
{
    public Transform Target;
    public float MouseSensitivity = 10f;

    private float _verticalRotation;
    private float _horizontalRotation;

    private void LateUpdate()
    {
        if (Target == null)
            return;

        Vector3 newPosition = new Vector3(Target.position.x, Target.position.y + 5.0f, Target.position.z - 7.0f);
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(40f, 0f, 0f);
    }
}