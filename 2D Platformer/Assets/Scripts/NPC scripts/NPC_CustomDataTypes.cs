using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct AttackPattern
{
    public Transform attackZone;
    public Vector2 attackBox;
    public Collider2D playerCollider;
    
    public float attackIntervalSec;
    public float elapsedTime;
    public float attackDamage;
}
