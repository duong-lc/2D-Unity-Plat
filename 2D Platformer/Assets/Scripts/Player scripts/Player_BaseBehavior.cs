using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Player_BaseBehavior : MonoBehaviour
{
    [SerializeField] private CharacterVariantsSO _varientData;
    
    public float Speed => _varientData.speed; //movement speed (left and right)
    public float JumpForce => _varientData.jumpForce; //jump force (up)
    public int TakeDamageDelayMS => _varientData.takeDamageDelayMS;
    public int DeathDelayMS => _varientData.deathDelayMS;
    
    protected static readonly int[] Attacks;
    protected static readonly int Death, Fall, Idle, Jump, Run, TakeHit;
    
    protected Animator Animator;//getting animator to set conditions for animation transitions
    public PlayerBehavior parent_PlayerBehaviorScript;
    public GameObject parent_Player;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        
        parent_Player = GameObject.Find("Player");
        parent_PlayerBehaviorScript = parent_Player.GetComponent<PlayerBehavior>();
    }

    public virtual void OnCharacterDeath()
    {
        
    }

    protected async void DeathDelay()
    {
        await Task.Delay(DeathDelayMS); 
    }
}
