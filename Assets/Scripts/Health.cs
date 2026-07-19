using UnityEngine;
using Fusion;
public class Health : NetworkBehaviour
{
    [SerializeField] private int _health;
    private bool isEnemy;
    private EnemyData enemyData;

    private int _maxHealth;
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

        Debug.Log(_maxHealth);
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
            }
            Debug.Log("I am dead");
        }

        Debug.Log(_health);
    }

    public void Heal(int heal)
    {
        if (!Object.HasStateAuthority) return;
        if (_health >= _maxHealth) return;

        _health += heal;

        if (_health >= _maxHealth)
        {
            _health = _maxHealth;

            Debug.Log("I am full hp");
        }

        Debug.Log(_health);
    }
}
