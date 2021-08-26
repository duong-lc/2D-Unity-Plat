using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUpTextScript : MonoBehaviour
{
    public Color colorStart, colorEnd;
    public Vector2 startOffset, endOffset;
    public float fadeDuration;
    private float fadeStartTime;

    // Start is called before the first frame update
    private void Awake() {
        fadeStartTime = Time.time;
        
    }
    public void SetText(string DamageText){
        this.gameObject.GetComponent<TextMesh>().text = DamageText;
    }
    private void Update() {
        float alpha = (Time.time-fadeStartTime)/fadeDuration;
        
        if(alpha <= 1){
            this.transform.localPosition = Vector2.Lerp(startOffset, endOffset, alpha);
            this.gameObject.GetComponent<TextMesh>().color = Color.Lerp(colorStart, colorEnd, alpha);
            // Debug.Log(alpha);
        }
        if(alpha > 1)
            Destroy(this.gameObject);
        
        
    }
}
