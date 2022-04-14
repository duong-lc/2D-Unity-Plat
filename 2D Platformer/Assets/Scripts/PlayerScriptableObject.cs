using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BasePlayerScriptableObject", order = 1)]
public class PlayerScriptableObject : ScriptableObject
{
    [Header("Ground Check Values")]
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    // [Header("Player Values")]
}
