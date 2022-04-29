using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/NPCEnemyVariantsSO", order = 0)]

public class NPCEnemyVariantsSO : ScriptableObject
{
    public float runSpeed = 1;
    public LayerMask PlayerLayer;

    
}
