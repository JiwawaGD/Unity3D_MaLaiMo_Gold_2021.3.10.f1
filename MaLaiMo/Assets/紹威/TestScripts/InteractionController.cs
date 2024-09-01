using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask InteractiveItem;
    public GameObject promptImage;
    public GameObject uiPreviewPanel;
    public CoinTossView coinTossView;

    private Camera playerCamera;
    private bool isLookingAtCoin = false;

    void Start()
    {
        Debug.Log("InteractionController Start method called");
        playerCamera = GetComponentInChildren<Camera>();
        promptImage.gameObject.SetActive(false);
        uiPreviewPanel.SetActive(false);

        if (coinTossView == null)
        {
            coinTossView = GetComponent<CoinTossView>();
            if (coinTossView == null)
            {
                Debug.LogError("CoinTossView component not found");
            }
            else
            {
                Debug.Log("CoinTossView component found and assigned");
            }
        }
    }

    void Update()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, InteractiveItem))
        {
            promptImage.gameObject.SetActive(true);
            isLookingAtCoin = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                ShowUIPreview();
            }
        }
        else
        {
            promptImage.gameObject.SetActive(false);
            isLookingAtCoin = false;
        }

        if (uiPreviewPanel.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            StartCoinTossView();
        }
    }

    void ShowUIPreview()
    {
        uiPreviewPanel.SetActive(true);
        // 禁用玩家移動
        GetComponent<FirstPersonController>().enabled = false;
    }

    void StartCoinTossView()
    {
        Debug.Log("StartCoinTossView called");
        uiPreviewPanel.SetActive(false);
        if (coinTossView != null)
        {
            if (coinTossView.coinTossCamera != null)
            {
                coinTossView.coinTossCamera.SetActive(true);
            }
            else
            {
                Debug.LogError("coinTossCamera in CoinTossView is null");
            }
            coinTossView.StartCoinToss();
        }
        else
        {
            Debug.LogError("CoinTossView is null");
        }
    }
}