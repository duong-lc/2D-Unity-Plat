using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractStatueScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite activatedSprite, unactivatedSprite;
    public GameObject floatingPlatform;
    public GameObject particleSystem;
    public Transform startPosition, endPosition;
    public float travelTime;
    private float startTime;
    private bool isActivated;
    void Awake()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = unactivatedSprite;
        floatingPlatform.transform.position = startPosition.position;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("playerArrow"))
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = activatedSprite;
            StartCoroutine(PingPongPosition());
            startTime = Time.time;
            particleSystem.GetComponent<ParticleSystem>().Play(true);
            this.gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }

    private IEnumerator PingPongPosition()
    {
        while(true){
            yield return StartCoroutine(MovePlatform(startPosition, endPosition));
            yield return StartCoroutine(MovePlatform(endPosition, startPosition));
        }
    }

    IEnumerator MovePlatform(Transform startPos, Transform endPos)
    {
        float alpha = 0.0f;
        float rate = 1.0f/travelTime;
        while (alpha < 1.0f)    
        {
            alpha += Time.deltaTime * rate;
            floatingPlatform.transform.position = Vector2.Lerp(startPos.position, endPos.position, alpha);
            yield return null;
        }
    }
}
