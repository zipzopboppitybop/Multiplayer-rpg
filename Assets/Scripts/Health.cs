using UnityEngine;
using Fusion;
using UnityEngine.Events;
public class Health : NetworkBehaviour
{
    [SerializeField] private int _startingHealth;
    private bool isEnemy;
    private EnemyData enemyData;
    private NetworkObject _enemyObject;
    private int _maxHealth;
    private PlayerSpawner _spawner;

    [Networked, OnChangedRender(nameof(OnHealthChanged))]
    private int _health { get; set; }
    public int CurrentHealth => _health;

    // Health Events
    public UnityEvent<int> OnHealthCreated;
    public UnityEvent<int> OnDamagePlayer;
    public UnityEvent<int> OnHealPlayer;
    public UnityEvent OnPlayerDeath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Spawned()
    {
        _spawner = GameObject.FindWithTag("PlayerSpawner").GetComponent<PlayerSpawner>();
        if (TryGetComponent<Enemy>(out var enemy))
        {
            isEnemy = true;
            enemyData = enemy.data;
            _enemyObject = enemy.GetComponent<NetworkObject>();
            _maxHealth = enemyData.maxHealth;
        }
        else
        {
            _maxHealth = _startingHealth;
        }

        if (Object.HasStateAuthority)
        {
            _health = _maxHealth;
        }

        OnHealthCreated.Invoke(_maxHealth);
    }

    private void OnHealthChanged()
    {
        if (!isEnemy)
        {
            OnDamagePlayer.Invoke(_health);
        }
    }

    public void Damage(int damage)
    {
        if (!Object.HasStateAuthority) return;
        if (_health <= 0) return;

        _health -= damage;

        if (_health <= 0)
        {
            _health = 0;

            if (isEnemy)
            {
                Runner.Spawn(enemyData.droppedItem, transform.position, Quaternion.identity, PlayerRef.None);
                Runner.Despawn(Object);
                _spawner._spawnedEnemies.Remove(_enemyObject);
            }

            OnPlayerDeath.Invoke();
        }
    }

    public void Heal(int heal)
    {
        if (!Object.HasStateAuthority) return;
        if (_health >= _maxHealth) return;

        _health += heal;

        if (_health >= _maxHealth)
        {
            _health = _maxHealth;

        }
    }
}
