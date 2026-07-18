using System;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] ItemData itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(other.TryGetComponent<Health>(out var health))
            {
                health.Heal(itemData.healAmount);
                Destroy(gameObject);
            }
        }
    }
}
