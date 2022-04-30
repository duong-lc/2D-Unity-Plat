using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct AttackPattern
{
    public Transform attackZone;
    public Vector2 attackBox;
    public float attackRadius;
    public Collider2D playerCollider;
    
    public float attackIntervalSec;
    public float attackDamage;
}
