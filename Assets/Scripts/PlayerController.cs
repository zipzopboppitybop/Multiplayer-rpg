using Fusion;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float MoveSpeed = 5f;
    private Health _hp;
    private NetworkCharacterController _cc;
    private CharacterController _characterController;
    public GameObject _camera;
    private readonly Collider[] hitColliders = new Collider[10];
    Vector3 GetHitboxCenter() => transform.position + (transform.forward * 1.5f);
    Vector3 GetHitboxHalfExtents() => new Vector3(1.5f, 1.5f, 2.0f) / 2f;
    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        _characterController = GetComponent<CharacterController>();
        _hp = GetComponent<Health>();
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

        PlayerEvents.LocalPlayerSpawned(_hp);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        Destroy(_camera);
        PlayerEvents.Clear();
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

        if (data.Attack)
        {
            Attack();
        }

        if (data.DamagePlayer)
        {
            _hp.Damage(1);
        }

        if (data.HealPlayer)
        {
            _hp.Heal(1);
        }

        _cc.Move(move);
    }
    private void Attack()
    {
        Vector3 hitboxCenter = GetHitboxCenter();
        Vector3 halfExtents = GetHitboxHalfExtents();

        int numColliders = Physics.OverlapBoxNonAlloc(hitboxCenter, halfExtents, hitColliders, transform.rotation);

        for (int i = 0; i < numColliders; i++)
        {
            Collider hit = hitColliders[i];

            if (hit.CompareTag("Enemy") && hit.TryGetComponent<Health>(out var enemy))
            {
                enemy.Damage(1);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(GetHitboxCenter(), transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, GetHitboxHalfExtents() * 2f);
    }
}