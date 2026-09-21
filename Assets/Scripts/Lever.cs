using Fusion;
using UnityEngine;

public class Lever : NetworkBehaviour
{
    [Networked] public bool flipped { get; set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void flip()
    {
        flipped = !flipped;
        Debug.Log($"I have been flipped to {flipped}");
    }
}
