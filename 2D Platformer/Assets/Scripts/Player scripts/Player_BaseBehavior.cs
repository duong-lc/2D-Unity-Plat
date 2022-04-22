using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Player_BaseBehavior : MonoBehaviour
{
    [SerializeField] protected CharacterVariantsSO variantData;
    
    public float Speed => variantData.speed; //movement speed (left and right)
    public float JumpForce => variantData.jumpForce; //jump force (up)
    public int TakeDamageDelayMS => variantData.takeDamageDelayMS;
    public int DeathDelayMS => variantData.deathDelayMS;
    
    protected static int[] Attacks;
    protected static int Death, Fall, Jump, Run, TakeHit;
    
    protected Animator Animator;//getting animator to set conditions for animation transitions
    public PlayerBehavior parent_PlayerBehaviorScript;
    public GameObject parent_Player;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        
        parent_Player = GameObject.Find("Player");
        parent_PlayerBehaviorScript = parent_Player.GetComponent<PlayerBehavior>();

        Attacks = new int[variantData.attacks.Length];
        for (int i = 0; i < Attacks.Length; i++ )
        {
            Attacks[i] = Animator.StringToHash(variantData.attacks[i]);
        }
        
        Death = Animator.StringToHash(variantData.death);
        Fall = Animator.StringToHash(variantData.fall);
        Jump = Animator.StringToHash(variantData.jump);
        Run = Animator.StringToHash(variantData.run);
        TakeHit = Animator.StringToHash(variantData.takeHit);
    }

    public virtual void OnCharacterDeath()
    {
        
    }
    protected async void DeathDelay()
    {
        await Task.Delay(DeathDelayMS); 
    }
}
