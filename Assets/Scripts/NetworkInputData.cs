using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector3 Direction;
    public NetworkBool Jump;
    public NetworkBool Damage;
    public NetworkBool Heal;
}
