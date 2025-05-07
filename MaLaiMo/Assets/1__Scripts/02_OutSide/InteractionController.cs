using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractionController : MonoBehaviour
{
    public static InteractionController Instance;
    [SerializeField] OutSidePlayer outSidePlayer;
    [SerializeField] SceneController_OutSide sceneControllerOutSide;
    [SerializeField] AUDManager audManager; // 如果有音效控制


    [Header("基本設置")]
    public float interactionDistance = 3f;
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
    public bool useFixedPattern = true; // 是否使用固定模式（三次必中）

    public bool finishedAllThrows = false; // 新增變數
    private bool isLookingAtCoin = false;
    private bool isThrowingCoin = false;
    private Quaternion originalRotation;
    private bool canDetectCoin = true;
    private int throwCount = 0; // 追蹤投擲次數
    

    void Start()
    {
        hand.SetActive(false);
    }



    void DisableAllCoinObjects()
    {
        coinPlusPlus.SetActive(false);
        coinMinusMinus.SetActive(false);
        coinPlusMinus.SetActive(false);
    }

    // 在InteractionController.cs的StartThrowingSequence方法中
    public void StartThrowingSequence()
    {
        if (isThrowingCoin)
        {
            Debug.Log("已經在投擲流程中，阻止重複觸發");
            return;
        }

        isThrowingCoin = true;

        // 確保禁用玩家控制
        if (outSidePlayer != null)
        {
            outSidePlayer._bCanControl = false;
        }

        if (sceneControllerOutSide != null)
        {
            sceneControllerOutSide.SetPlayerControl(false);
        }

        hand.SetActive(true);
        canDetectCoin = false;
        StartCoroutine(ThrowCoin());
    }


    IEnumerator ThrowCoin()
    {
        isThrowingCoin = true;

        DisableAllCoinObjects(); // 清除上次結果

        yield return StartCoroutine(PerformThrowAnimation());

        GameObject selectedCoin = DetermineCoinResult(out string resultString);
        selectedCoin.SetActive(true);

        yield return StartCoroutine(ShowResult());

        throwCount++; // ← 提早計算
        ResetState(); // 確保狀態乾淨
    }


    GameObject DetermineCoinResult(out string resultString)
    {
        if (useFixedPattern)
        {
            if (throwCount >= 2)
            {
                resultString = "正反 (+-)";
                Debug.Log("投擲結果: " + resultString + " [勝利]");
                finishedAllThrows = true; // 三次後標記完成
                SceneController_OutSide.FinishDollar = true;
                return coinPlusMinus;
            }
            else
            {
                resultString = "反反 (--)";
                Debug.Log("投擲結果: " + resultString + " [失敗]");
                return coinMinusMinus;
            }
        }
        else
        {
            int result = Random.Range(0, 3);
            switch (result)
            {
                case 0:
                    resultString = "正正 (++)";
                    Debug.Log("投擲結果: " + resultString);
                    return coinPlusPlus;
                case 1:
                    resultString = "反反 (--)";
                    Debug.Log("投擲結果: " + resultString);
                    return coinMinusMinus;
                default:
                    resultString = "正反 (+-)";
                    Debug.Log("投擲結果: " + resultString);
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

    IEnumerator ResetStateCoroutine()
    {
        playerCamera.transform.localRotation = originalRotation;
        DisableAllCoinObjects();
        isThrowingCoin = false;
        canDetectCoin = true;

        yield return new WaitForSeconds(0.1f);

        // 這裡的引用可能有問題，確保正確引用outSidePlayer和sceneControllerOutSide
        if (outSidePlayer != null)
        {
            outSidePlayer.enabled = true;
            outSidePlayer._bCanControl = true;
        }

        if (sceneControllerOutSide != null)
        {
            sceneControllerOutSide.SetPlayerControl(true);
            sceneControllerOutSide.SetCrosshairEnable(true);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Reset 完成: throwCount = " + throwCount + ", 玩家控制已恢復");
        OnThrowingFinished();
    }



    void ResetState()
    {
        Debug.Log("開始重置玩家狀態");
        StartCoroutine(ResetStateCoroutine());
    }

    public void OnThrowingFinished()
    {
        if (sceneControllerOutSide != null)
        {
            sceneControllerOutSide.OnCoinThrowingFinished();
        }
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