using UnityEngine;
using Fusion;
using UnityEngine.Events;
public class Health : NetworkBehaviour
{
    [SerializeField] private int _health;
    private bool isEnemy;
    private EnemyData enemyData;

    private int _maxHealth;

    // Health Events
    public UnityEvent<int> OnHealthCreated;
    public UnityEvent<int> OnDamagePlayer;
    public UnityEvent<int> OnHealPlayer;
    public UnityEvent OnPlayerDeath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (TryGetComponent<Enemy>(out var enemy))
        {
            isEnemy = true;
            enemyData = enemy.data;
            _maxHealth = enemyData.maxHealth;
        }
        else
        {
            _maxHealth = _health;
        }

        OnHealthCreated.Invoke(_maxHealth);

        Debug.Log(_maxHealth);
    }

    public void Damage(int damage)
    {
        if (!Object.HasStateAuthority) return;
        if (_health <= 0) return;

        _health -= damage;

        if (!isEnemy)
        {
            OnDamagePlayer.Invoke(_health);
        }

        if (_health <= 0)
        {
            _health = 0;

            if (isEnemy)
            {
                Runner.Spawn(enemyData.droppedItem, transform.position, Quaternion.identity, PlayerRef.None);
                Runner.Despawn(Object);
            }

            OnPlayerDeath.Invoke();
            Debug.Log("I am dead");
        }

        Debug.Log(_health);
    }

    public void Heal(int heal)
    {
        if (!Object.HasStateAuthority) return;
        if (_health >= _maxHealth) return;

        _health += heal;

        if (!isEnemy)
        {
            OnHealPlayer.Invoke(_health);
        }

        if (_health >= _maxHealth)
        {
            _health = _maxHealth;

            Debug.Log("I am full hp");
        }

        Debug.Log(_health);
    }
}
