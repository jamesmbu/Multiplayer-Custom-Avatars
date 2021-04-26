using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemporaryOnScreen : MonoBehaviour
{
    private Text ThisText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTempText(string message)
    {
        ThisText = GetComponent<Text>();
        ThisText.text = message;
        StartCoroutine(DisplayBigMessage(ThisText, 1f));
    }
    IEnumerator DisplayBigMessage(Text textField, float fadeTime)
    {
        // Display text for a duration
        yield return new WaitForSeconds(3f);

        textField.color = new Color(textField.color.r,
            textField.color.g,
            textField.color.b,
            1);

        // Fade text out
        while (textField.color.a > 0.0f)
        {
            textField.color = new Color(textField.color.r, textField.color.g,
                textField.color.b, textField.color.a - (Time.deltaTime / fadeTime));
            yield return null;
        }
    }
}
