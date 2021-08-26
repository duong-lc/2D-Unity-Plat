using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriterEffect : MonoBehaviour
{
    public void BeginEffect(string textToType, TMP_Text text, float totalTimeDisplay)
    {
        if(textToType == "")
            text.text = "";
        else
            StartCoroutine(TypeWriteCoroutine(textToType, text, totalTimeDisplay));
    }

    private IEnumerator TypeWriteCoroutine(string textToType, TMP_Text text, float totalTimeDisplay)
    {
        int startIndex = 0;

        while(startIndex <= textToType.Length)
        {
            text.text = textToType.Substring(0, startIndex);

            yield return new WaitForSeconds(totalTimeDisplay / textToType.Length);
            startIndex++;
        }
    }
}
