using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogSystem : MonoBehaviour
{
    public TMP_Text textLabel;
    public TextAsset textAsset;
    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;

    private bool isFading;
    private int index;
    private List<string> textList = new List<string>();

    private void Start()
    {
        StartCoroutine(FadeInAndDisplayText());
    }

    private IEnumerator FadeInAndDisplayText()
    {
        Color startColor = textLabel.color;
        startColor.a = 0f;
        textLabel.color = startColor;

        isFading = true;
        index = 0;

        GetTextFromFile(textAsset);
        textLabel.text = textList[index];

        yield return StartCoroutine(FadeTextAlpha(startColor.a, 1f, fadeInTime));

        while (index < textList.Count - 1)
        {
            yield return new WaitForSeconds(2f);

            index++;
            textLabel.text = textList[index];

            yield return StartCoroutine(FadeTextAlpha(1f, 0f, fadeOutTime));
        }

        isFading = false;
        Destroy(gameObject, 1.5f);
    }

    private void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        string[] lineData = file.text.Split('\n');

        foreach (string line in lineData)
        {
            textList.Add(line);
        }
    }

    private IEnumerator FadeTextAlpha(float startAlpha, float endAlpha, float fadeTime)
    {
        float elapsedTime = 0f;
        Color color = textLabel.color;

        while (elapsedTime < fadeTime)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeTime);
            color.a = alpha;
            textLabel.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        textLabel.color = color;
    }
}
