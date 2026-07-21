using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector3 Direction;
    public NetworkBool Jump;
    public NetworkBool Attack;

    // Debugging
    public NetworkBool DamagePlayer;
    public NetworkBool HealPlayer;
}
