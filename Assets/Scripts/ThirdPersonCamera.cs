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

        Vector3 newPosition = new Vector3(Target.position.x, Target.position.y + 20.0f, Target.position.z);
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}