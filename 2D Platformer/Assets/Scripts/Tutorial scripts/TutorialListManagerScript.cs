using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialListManagerScript : MonoBehaviour
{
    //Adding all of the tutorial game objects into a list so that when i need a tutorial gameobject i don't have 
    //to gameobject.find it and can just loop through the list in case i have changed the names or replace the tags of 
    //those tutorial objects. Plus i can have collective features such as deleting all of tutorial gameobjects under a same
    //script
    public List<GameObject> tutorialGameObjectList;

    public void OnTriggerEnter_TutorialList(GameObject tutElement, Collider2D objContact)
    {
        if(objContact.tag == "Player")
        {
            foreach(Transform child in tutElement.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
            


    public void OnTriggerExit_TutorialList(GameObject tutElement, Collider2D objContact)
    {
        if(objContact.tag == "Player")
        {
            foreach(Transform child in tutElement.transform)
            {
                child.gameObject.SetActive(false);
            }
        } 
    }
}
