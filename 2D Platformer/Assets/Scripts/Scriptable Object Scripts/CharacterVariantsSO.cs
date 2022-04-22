using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterVariantsSO", order = 0)]

public class CharacterVariantsSO : ScriptableObject
{
    [Header("Animation Names")]
    public string[] attacks;

    
    public string death;
    public string fall;
    public string jump;
    public string run;
    public string takeHit;
    
    
    [Header("Movement Data")] 
    public float speed;
    public float jumpForce;

    [Header("Misc.")] 
    public int takeDamageDelayMS;
    public int deathDelayMS;
}
