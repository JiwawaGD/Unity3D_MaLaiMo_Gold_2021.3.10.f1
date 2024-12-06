using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractionController : MonoBehaviour
{
    [Header("基本設置")]
    public float interactionDistance = 3f;
    public LayerMask InteractiveItem;
    public GameObject promptImage;
    public GameObject itemCoinObj;
    public GameObject uiPreviewPanel;
    public Camera playerCamera;
    public Camera coinCloseupCamera;

    [Header("硬幣結果物件")]
    public GameObject coinPlusPlus;   // ++ 結果
    public GameObject coinMinusMinus; // -- 結果
    public GameObject coinPlusMinus;  // +- 結果

    [Header("投擲設置")]
    public GameObject hand;
    public float throwingDuration = 1f;
    public float holdDuration = 6.15f;
    public float closeupDuration = 3f;
    public float returnDelay = 0.5f;

    [Header("模式設置")]
    public bool useFixedPattern = false; // 是否使用固定模式（三次必中）

    private bool isLookingAtCoin = false;
    private FirstPersonController fpsController;
    private bool isThrowingCoin = false;
    private Quaternion originalRotation;
    private bool canDetectCoin = true;
    private int throwCount = 0; // 追蹤投擲次數

    void Start()
    {
        InitializeComponents();
    }

    void InitializeComponents()
    {
        fpsController = GetComponent<FirstPersonController>();
        if (fpsController == null)
        {
            Debug.LogError("FirstPersonController not found!");
        }

        promptImage.SetActive(false);
        uiPreviewPanel.SetActive(false);
        coinCloseupCamera.gameObject.SetActive(false);
        hand.SetActive(false);
        DisableAllCoinObjects();
        throwCount = 0;
    }

    void Update()
    {
        if (isThrowingCoin) return;

        if (canDetectCoin)
        {
            HandleCoinDetection();
        }
    }

    void HandleCoinDetection()
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
                StartThrowingSequence();
            }
        }
        else
        {
            promptImage.SetActive(false);
            isLookingAtCoin = false;
        }
    }

    void ShowUIPreview()
    {
        uiPreviewPanel.SetActive(true);
        fpsController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void DisableAllCoinObjects()
    {
        coinPlusPlus.SetActive(false);
        coinMinusMinus.SetActive(false);
        coinPlusMinus.SetActive(false);
    }

    void StartThrowingSequence()
    {
        promptImage.SetActive(false);
        itemCoinObj.SetActive(false);
        canDetectCoin = false;
        StartCoroutine(ThrowCoin());
    }

    IEnumerator ThrowCoin()
    {
        isThrowingCoin = true;
        uiPreviewPanel.SetActive(false);
        fpsController.enabled = false;
        DisableAllCoinObjects();

        // 執行投擲動畫
        yield return StartCoroutine(PerformThrowAnimation());

        // 決定結果
        GameObject selectedCoin = DetermineCoinResult(out string resultString);
        Debug.Log($"硬幣投擲結果: {resultString} (第 {throwCount + 1} 次)");
        selectedCoin.SetActive(true);

        // 顯示結果
        yield return StartCoroutine(ShowResult());

        // 重置狀態
        ResetState();
        throwCount++;

        // 如果是固定模式且完成三次投擲，重置計數
        if (useFixedPattern && throwCount > 2)
        {
            throwCount = 0;
        }
    }

    GameObject DetermineCoinResult(out string resultString)
    {
        if (useFixedPattern)
        {
            // 固定模式：前兩次無杯，第三次聖杯
            if (throwCount >= 2)
            {
                resultString = "正反 (+-)";
                return coinPlusMinus;
            }
            else
            {
                resultString = "反反 (--)";
                return coinMinusMinus;
            }
        }
        else
        {
            // 隨機模式
            int result = Random.Range(0, 3);
            switch (result)
            {
                case 0:
                    resultString = "正正 (++)";
                    return coinPlusPlus;
                case 1:
                    resultString = "反反 (--)";
                    return coinMinusMinus;
                default:
                    resultString = "正反 (+-)";
                    return coinPlusMinus;
            }
        }
    }

    IEnumerator PerformThrowAnimation()
    {
        originalRotation = playerCamera.transform.localRotation;
        Quaternion throwRotation = Quaternion.Euler(20.9f, originalRotation.eulerAngles.y, originalRotation.eulerAngles.z);

        yield return StartCoroutine(SmoothRotateCamera(originalRotation, throwRotation, throwingDuration));

        hand.SetActive(true);
        yield return new WaitForSeconds(holdDuration);
        hand.SetActive(false);
    }

    IEnumerator ShowResult()
    {
        playerCamera.gameObject.SetActive(false);
        coinCloseupCamera.gameObject.SetActive(true);

        yield return new WaitForSeconds(closeupDuration);
        yield return new WaitForSeconds(returnDelay);

        coinCloseupCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
    }

    void ResetState()
    {
        playerCamera.transform.localRotation = originalRotation;
        DisableAllCoinObjects();
        fpsController.enabled = true;
        isThrowingCoin = false;
        canDetectCoin = true;
        itemCoinObj.SetActive(true);
    }

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