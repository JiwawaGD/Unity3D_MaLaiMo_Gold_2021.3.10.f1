using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractionController : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask InteractiveItem;
    public GameObject promptImage;
    public GameObject itemCoinObj;
    public GameObject uiPreviewPanel;
    public Camera playerCamera;
    public Camera coinCloseupCamera;

    public GameObject coinPlusPlus;   // ++ 結果
    public GameObject coinMinusMinus; // -- 結果
    public GameObject coinPlusMinus;  // +- 結果

    public GameObject hand; // 手部模型

    public float throwingDuration = 1f;  // 鏡頭旋轉到20.9度的時間
    public float holdDuration = 6.15f;   // 保持在20.9度的時間
    public float closeupDuration = 3f;   // 硬幣特寫的持續時間
    public float returnDelay = 0.5f;     // 從特寫視角回到玩家視角的延遲

    private bool isLookingAtCoin = false;
    private FirstPersonController fpsController;
    private bool isThrowingCoin = false;
    private Quaternion originalRotation;
    private bool canDetectCoin = true;

    void Start()
    {
        fpsController = GetComponent<FirstPersonController>();
        if (fpsController == null)
        {
            Debug.LogError("FirstPersonController not found on this GameObject!");
        }

        promptImage.SetActive(false);
        uiPreviewPanel.SetActive(false);
        coinCloseupCamera.gameObject.SetActive(false);
        hand.SetActive(false);
        DisableAllCoinObjects();
    }

    void Update()
    {
        if (isThrowingCoin) return;

        if (canDetectCoin)
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionDistance, InteractiveItem))
            {
                isLookingAtCoin = true;
                promptImage.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    ShowUIPreview();
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    promptImage.SetActive(false);
                    itemCoinObj.SetActive(false);
                    canDetectCoin = false;
                    StartCoroutine(ThrowCoin());
                }
            }
            else
            {
                promptImage.SetActive(false);
                isLookingAtCoin = false;
            }
        }
    }
    /// <summary>
    /// 顯示硬幣投擲的預覽畫面
    /// </summary>
    void ShowUIPreview()
    {
        uiPreviewPanel.SetActive(true);
        fpsController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    /// <summary>
    /// 關閉所有硬幣物件
    /// </summary>
    void DisableAllCoinObjects()
    {
        coinPlusPlus.SetActive(false);
        coinMinusMinus.SetActive(false);
        coinPlusMinus.SetActive(false);
    }
    /// <summary>
    /// 投擲硬幣
    /// </summary>
    /// <returns></returns>
    IEnumerator ThrowCoin()
    {
        isThrowingCoin = true;
        uiPreviewPanel.SetActive(false);
        fpsController.enabled = false;
        DisableAllCoinObjects();

        originalRotation = playerCamera.transform.localRotation;
        Quaternion throwRotation = Quaternion.Euler(20.9f, originalRotation.eulerAngles.y, originalRotation.eulerAngles.z);

        yield return StartCoroutine(SmoothRotateCamera(originalRotation, throwRotation, throwingDuration));

        hand.SetActive(true);
        yield return new WaitForSeconds(holdDuration);
        hand.SetActive(false);

        int result = Random.Range(0, 3);
        GameObject selectedCoin = null;
        string resultString = "";

        switch (result)
        {
            case 0:
                selectedCoin = coinPlusPlus;
                resultString = "正正 (++)";
                break;
            case 1:
                selectedCoin = coinMinusMinus;
                resultString = "反反 (--)";
                break;
            case 2:
                selectedCoin = coinPlusMinus;
                resultString = "正反 (+-)";
                break;
        }

        Debug.Log("硬幣投擲結果: " + resultString);
        selectedCoin.SetActive(true);

        playerCamera.gameObject.SetActive(false);
        coinCloseupCamera.gameObject.SetActive(true);

        yield return new WaitForSeconds(closeupDuration);
        yield return new WaitForSeconds(returnDelay);

        coinCloseupCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);

        playerCamera.transform.localRotation = originalRotation;

        DisableAllCoinObjects();
        fpsController.enabled = true;
        isThrowingCoin = false;
        // 注意：我們不在這裡重新啟用提示圖像，它會在下一次射線檢測到物體時自動顯示

        // 重新啟用射線檢測和 itemCoinObj
        canDetectCoin = true;
        itemCoinObj.SetActive(true);
    }
    /// <summary>
    /// 平滑旋轉相機
    /// </summary>
    /// <param name="startRotation"></param>
    /// <param name="endRotation"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator SmoothRotateCamera(Quaternion startRotation, Quaternion endRotation, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            playerCamera.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        playerCamera.transform.localRotation = endRotation;
    }
}