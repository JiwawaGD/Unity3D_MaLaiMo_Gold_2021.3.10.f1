using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractionController : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask InteractiveItem;
    public GameObject promptImage;
    public GameObject uiPreviewPanel;
    public Camera playerCamera;
    public Camera throwingCamera;
    public Camera coinCloseupCamera;
    
    public GameObject coinPlusPlus;   // ++ 結果
    public GameObject coinMinusMinus; // -- 結果
    public GameObject coinPlusMinus;  // +- 結果

    public float throwingDuration = 1f;  // 投擲動作的持續時間
    public float closeupDuration = 3f;   // 硬幣特寫的持續時間
    public float returnDelay = 0.5f;     // 從特寫視角回到玩家視角的延遲

    private bool isLookingAtCoin = false;
    private FirstPersonController fpsController;

    void Start()
    {
        fpsController = GetComponent<FirstPersonController>();
        promptImage.SetActive(false);
        uiPreviewPanel.SetActive(false);
        throwingCamera.gameObject.SetActive(false);
        coinCloseupCamera.gameObject.SetActive(false);
        DisableAllCoinObjects();
    }

    void Update()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, InteractiveItem))
        {
            promptImage.SetActive(true);
            isLookingAtCoin = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                ShowUIPreview();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(ThrowCoin());
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

    IEnumerator ThrowCoin()
    {
        uiPreviewPanel.SetActive(false);
        fpsController.enabled = false;
        DisableAllCoinObjects();

        // 切換到投擲攝像機（手部視角）
        playerCamera.gameObject.SetActive(false);
        throwingCamera.gameObject.SetActive(true);

        // 等待投擲動作完成
        yield return new WaitForSeconds(throwingDuration);

        // 隨機選擇結果
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

        // Debug 輸出投擲結果
        Debug.Log("硬幣投擲結果: " + resultString);

        // 啟用選中的硬幣物件
        selectedCoin.SetActive(true);

        // 切換到硬幣特寫攝像機
        throwingCamera.gameObject.SetActive(false);
        coinCloseupCamera.gameObject.SetActive(true);

        // 等待特寫時間
        yield return new WaitForSeconds(closeupDuration);

        // 等待從特寫視角回到玩家視角的延遲
        yield return new WaitForSeconds(returnDelay);

        // 回到第一人稱視角
        coinCloseupCamera.gameObject.SetActive(false);
        DisableAllCoinObjects();
        playerCamera.gameObject.SetActive(true);

        // 重新啟用 FPS 控制器
        fpsController.enabled = true;
    }
}