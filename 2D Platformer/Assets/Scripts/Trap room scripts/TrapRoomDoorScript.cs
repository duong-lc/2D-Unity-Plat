using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoomDoorScript : MonoBehaviour
{
    public bool isActivated;
    private TrapRoomScript trapRoomScript;
    public GameObject trapRoom;
    void Awake()
    {
        isActivated = false;
        trapRoomScript = trapRoom.GetComponent<TrapRoomScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isActivated)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 255 ,0 , trapRoomScript.transparency);
            StartCoroutine(Activated());
        }
        else if (!isActivated)
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0 ,0 , trapRoomScript.transparency);
    }

    private IEnumerator Activated()
    {
        yield return new WaitForSeconds(5.0f);
        isActivated = false;
    }

}
