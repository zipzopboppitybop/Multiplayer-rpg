using UnityEngine;
using Fusion;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int maxHealth;
    public NetworkPrefabRef droppedItem;
    public int speed;
    public float detectionRadius;
}