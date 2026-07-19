using System;
using UnityEngine;
using Fusion;

public class Heart : NetworkBehaviour
{
    [SerializeField] ItemData itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority) return;
        if (other.CompareTag("Player"))
        {
            if(other.TryGetComponent<Health>(out var health))
            {
                health.Heal(itemData.healAmount);
                Runner.Despawn(Object);
            }
        }
    }
}
