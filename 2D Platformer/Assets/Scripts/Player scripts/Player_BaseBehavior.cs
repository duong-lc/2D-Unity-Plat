using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player_BaseBehavior : MonoBehaviour
{
    public float speed; //movement speed (left and right)
    public float jumpForce; //jump force (up)
    public Animator animator;//getting animator to set conditions for animation transitions
    
    public PlayerBehavior parent_PlayerBehaviorScript;
    public GameObject parent_Player;

    private void Awake()
    {
        parent_Player = GameObject.Find("Player");
        parent_PlayerBehaviorScript = parent_Player.GetComponent<PlayerBehavior>();
    }
}
