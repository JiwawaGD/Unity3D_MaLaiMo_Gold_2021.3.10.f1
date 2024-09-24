using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderSystem : MonoBehaviour
{
    public float fadeDuration = 1.0f;
    public int iNextSceneID;

    private CanvasGroup canvasGroup;
    private bool isFading = false;

    private void Awake()
    {
        GameObject fadeObject = new GameObject("FadeObject");
        Canvas fadeCanvas = fadeObject.AddComponent<Canvas>();
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGroup = fadeObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        DontDestroyOnLoad(fadeObject);
    }

    private void Update()
    {
        if (isFading)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(LoadSceneWithFade());
        }
    }

    private System.Collections.IEnumerator LoadSceneWithFade()
    {
        isFading = true;

        // 淡入效果
        float fadeSpeed = 1.0f / fadeDuration;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += fadeSpeed * Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(iNextSceneID);

        // 淡出效果
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }

        isFading = false;
    }
}
