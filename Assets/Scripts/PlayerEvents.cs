using System;
using UnityEngine;

public static class PlayerEvents
{
    public static Health LocalPlayerHealth { get; private set; }
    public static event Action<Health> OnLocalPlayerSpawned;

    public static void LocalPlayerSpawned(Health health)
    {
        LocalPlayerHealth = health;
        OnLocalPlayerSpawned?.Invoke(health);
    }

    public static void Clear()
    {
        LocalPlayerHealth = null;
    }
}