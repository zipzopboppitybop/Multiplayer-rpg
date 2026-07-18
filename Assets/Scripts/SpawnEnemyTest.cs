using UnityEngine;
using Fusion;
using Unity.VisualScripting;

public class SpawnEnemyTest : NetworkBehaviour
{
    [SerializeField] private PlayerSpawner _spawner;
    [SerializeField] private NetworkPrefabRef _enemy;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private int _spawnCountMax;

    private int _spawnCount;

    private void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority) return;
        if (_spawnCount >= _spawnCountMax) return;

        if (other.CompareTag("Player"))
        {
            _spawner.SpawnEnemy(_enemy, _spawnPosition.position);
            _spawnCount++;
        }
    }
}
