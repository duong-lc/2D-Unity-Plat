using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    private TutorialListManagerScript tutorialListManagerScript;
    void Start()
    {
        tutorialListManagerScript = GameObject.FindGameObjectWithTag("_TutorialListManager").GetComponent<TutorialListManagerScript>();
        tutorialListManagerScript.tutorialGameObjectList.Add(this.gameObject);

        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        tutorialListManagerScript.OnTriggerEnter_TutorialList(this.gameObject, other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        tutorialListManagerScript.OnTriggerExit_TutorialList(this.gameObject, other);
    }
}
