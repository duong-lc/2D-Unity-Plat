using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]

public abstract class PlayerBaseBehavior : MonoBehaviour
{
    [SerializeField] private CharacterVariantsSO variantData;
    
    public float Speed => variantData.speed; //movement speed (left and right)
    public float JumpForce => variantData.jumpForce; //jump force (up)
    public int TakeDamageDelayMS => variantData.takeDamageDelayMS;
    public int DeathDelayMS => variantData.deathDelayMS;

    protected int[] Attacks;
    protected static int Death, Fall, Jump, Run, TakeHit;
    
    protected Animator Animator => GetComponent<Animator>();//getting animator to set conditions for animation transitions
    protected PlayerBehavior ParentPlayerBehaviorScript => PlayerBehavior.Instance;
    protected GameObject ParentPlayer => ParentPlayerBehaviorScript.gameObject;

    private void Awake()
    {
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
