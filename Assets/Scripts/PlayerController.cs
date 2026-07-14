using Fusion;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float MoveSpeed = 5f;
    private NetworkCharacterController _cc;
    private CharacterController _characterController;
    public GameObject _camera;
    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void Spawned()
    {
        if (!Object.HasInputAuthority) return;

        _camera = new GameObject("Camera");
        _camera.AddComponent<Camera>();
        _camera.AddComponent<ThirdPersonCamera>();
        _camera.GetComponent<ThirdPersonCamera>().Target = transform;
        _camera.transform.SetParent(this.transform);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        Destroy(_camera);
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            HandleMovement(data);
        }
    }

    private void HandleMovement(NetworkInputData data)
    {
        Vector3 move = data.Direction.normalized * MoveSpeed;

        if (data.Jump)
        {
            _cc.Jump();
        }

        _cc.Move(move);
    }
}