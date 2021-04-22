using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaAfterImage : MonoBehaviour
{
    private SpriteRenderer SR;
    private SpriteRenderer playerKatanaSR;
    private GameObject playerKatana;
    
    void Start()
    {
        SR = this.gameObject.GetComponent<SpriteRenderer>();
        playerKatana = GameObject.FindGameObjectWithTag("Player-Katana");
        

    }

    public void UponInstantiation(Transform currPos_Player)
    {
        playerKatanaSR = playerKatana.GetComponent<SpriteRenderer>();
        SR.sprite = playerKatanaSR.sprite;
        this.gameObject.transform.position = currPos_Player.position;
       
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
