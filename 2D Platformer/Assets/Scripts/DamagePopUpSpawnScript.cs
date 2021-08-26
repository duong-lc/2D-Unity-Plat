using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DamagePopUpSpawnScript : MonoBehaviour
{
    public GameObject popUpText;
    [SerializeField] private Transform  spawnPos;
    // Update is called once per frame

    public GameObject SpawnDamagedText(){
        
            
        if(gameObject.tag == "Enemy-Boss"){
            GameObject obj = Instantiate(popUpText, spawnPos.position, Quaternion.identity, this.transform);
            obj.GetComponent<DamagePopUpTextScript>().startOffset = new Vector2(0,0);
            obj.GetComponent<DamagePopUpTextScript>().endOffset = new Vector2(0,0.7f);
            
            //obj.GetComponent<RectTransform>().transform.localPosition.Set(0,0,0);
            //Debug.Log(obj.transform.position);
            return obj;
        }
        else{
            return Instantiate(popUpText, this.transform.position, Quaternion.identity, this.transform);
        }
    }
}
