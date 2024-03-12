using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OperatingTips : MonoBehaviour
{
    public Image image;
    public TMP_Text text;
    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;

    private bool isInside;
    private bool isFading;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInside = true;
            if (!isFading)
            {
                StartCoroutine(FabeIn());
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInside = false;
            if (!isFading)
            {
                StartCoroutine(FadeOut());
            }
        }
    }
    public IEnumerator FabeIn()
    {
        isFading = true;

        Color startColor = image.color;
        Color startColorText = text.color;
        startColor.a = 0f;
        startColorText.a = 0f;
        image.color = startColor;
        text.color = startColorText;

        float elapsedTime = 0f;
        while (elapsedTime < fadeInTime)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInTime);
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;
            text.color = newColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isFading = false;
    }

    public IEnumerator FadeOut()
    {
        isFading = true;

        Color startColor = image.color;
        Color startColorText = text.color;
        startColor.a = 1f;
        startColorText.a = 1f;
        image.color = startColor;
        text.color = startColorText;

        float elapsedTime = 0f;
        while (elapsedTime < fadeInTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeInTime);
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;
            text.color = newColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isFading = false;
        Destroy(gameObject,1.5f);

    }
}
