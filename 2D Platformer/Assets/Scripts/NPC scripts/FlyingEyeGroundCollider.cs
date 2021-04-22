using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class FlyingEyeGroundCollider : MonoBehaviour
{
    public GameObject parent;
    private bool isGround = false;

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "ground")
        {
            StartCoroutine(TranslateUp());
        }
    }*/

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "ground")
        {
            StartCoroutine(TranslateUp());
        }
    }

   /* private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "ground")
        {
            isGround = false;
        }
    }
*/

    private IEnumerator TranslateUp()
    {
        while (true)
        {
            parent.transform.Translate(transform.up/200, Space.Self);
            yield return new WaitForSeconds(0.1f);

            if(isGround == false)
                break;
        }
    }
}
