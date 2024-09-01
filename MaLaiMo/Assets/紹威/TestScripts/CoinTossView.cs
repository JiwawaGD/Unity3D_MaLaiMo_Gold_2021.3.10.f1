using UnityEngine;
using System.Collections;
using System.Linq;

public class CoinTossView : MonoBehaviour
{
    public enum CoinResult
    {
        ShengPo,    // 聖筊 (一正一反)
        YinPo,      // 陰筊 (兩個都反面)
        YangPo      // 陽筊 (兩個都正面)
    }

    public GameObject playerCamera;
    public GameObject coinTossCamera;
    public GameObject[] coins; // 兩個硬幣的遊戲物體
    public float tossHeight = 2f; // 投擲高度
    public float tossDuration = 1f; // 上升和下降的持續時間
    public float pauseDuration = 3f; // 硬幣落地後停留時間
    public float throwForce = 5f;
    public float rotationForce = 2f;

    private Vector3 initialCameraPosition;
    private Vector3[] initialCoinPositions;
    private Rigidbody[] coinRigidbodies;

    private void Awake()
    {
        Debug.Log("CoinTossView Awake called");
    }
    private void Start()
    {
        Debug.Log("CoinTossView Start method called");

        if (coins == null)
        {
            Debug.LogError("Coins array is null in the inspector");
            return;
        }

        Debug.Log($"Coins array length: {coins.Length}");

        if (coins.Length == 0)
        {
            Debug.LogError("No coins assigned in the inspector");
            return;
        }

        initialCameraPosition = coinTossCamera ? coinTossCamera.transform.position : Vector3.zero;
        initialCoinPositions = new Vector3[coins.Length];
        coinRigidbodies = new Rigidbody[coins.Length];

        for (int i = 0; i < coins.Length; i++)
        {
            if (coins[i] == null)
            {
                Debug.LogError($"Coin at index {i} is not assigned in the inspector");
                continue;
            }

            initialCoinPositions[i] = coins[i].transform.position;
            coinRigidbodies[i] = coins[i].GetComponent<Rigidbody>();

            if (coinRigidbodies[i] == null)
            {
                Debug.LogError($"Rigidbody component not found on coin at index {i}");
            }
        }

        Debug.Log($"CoinTossView initialized. Coins count: {coins.Length}, Rigidbodies count: {coinRigidbodies.Count(rb => rb != null)}");
    }

    public void StartCoinToss()
    {
        Debug.Log("StartCoinToss method called");
        if (coinRigidbodies == null || coinRigidbodies.Length == 0)
        {
            Debug.LogError("coinRigidbodies is null or empty. Trying to reinitialize...");
            Start();

            if (coinRigidbodies == null || coinRigidbodies.Length == 0)
            {
                Debug.LogError("Reinitialization failed. Cannot proceed with coin toss.");
                return;
            }
        }
        StartCoroutine(CoinTossSequence());
    }

    private IEnumerator CoinTossSequence()
    {
        // 切換到硬幣投擲相機
        playerCamera.SetActive(false);
        coinTossCamera.SetActive(true);

        // 投擲硬幣
        ThrowCoins();

        // 等待硬幣落地
        yield return new WaitForSeconds(pauseDuration);

        // 檢查結果
        CoinResult result = CheckResult();
        Debug.Log("擲筊結果: " + result.ToString());
        TriggerResultEvent(result);

        // 切換回玩家相機
        coinTossCamera.SetActive(false);
        playerCamera.SetActive(true);

        // 重置硬幣和相機位置
        ResetPositions();

        // 重新啟用玩家控制
        // 假設有一個 FirstPersonController 腳本
        FindObjectOfType<FirstPersonController>().enabled = true;
    }

    private void ThrowCoins()
    {
        foreach (var rb in coinRigidbodies)
        {
            if (rb != null)
            {
                // 添加向上的力
                rb.AddForce(Vector3.up * throwForce, ForceMode.Impulse);

                // 添加隨機的水平力，使硬幣散開
                Vector2 randomHorizontal = Random.insideUnitCircle * throwForce * 0.1f;
                rb.AddForce(new Vector3(randomHorizontal.x, 0, randomHorizontal.y), ForceMode.Impulse);

                // 添加隨機旋轉
                rb.AddTorque(Random.insideUnitSphere * rotationForce, ForceMode.Impulse);
            }
        }
    }

    private CoinResult CheckResult()
    {
        int facingUpCount = 0;
        foreach (var coin in coins)
        {
            if (IsCoinFaceUp(coin))
            {
                facingUpCount++;
            }
        }

        if (facingUpCount == 1) return CoinResult.ShengPo;
        if (facingUpCount == 2) return CoinResult.YangPo;
        return CoinResult.YinPo;
    }

    private bool IsCoinFaceUp(GameObject coin)
    {
        // 假設硬幣的"正面"是其local up方向
        float dotProduct = Vector3.Dot(coin.transform.up, Vector3.up);
        return dotProduct > 0.5f; // 如果硬幣偏離垂直線不超過60度，就認為是正面朝上
    }

    private void TriggerResultEvent(CoinResult result)
    {
        switch (result)
        {
            case CoinResult.ShengPo:
                Debug.Log("聖筊: 神明同意");
                break;
            case CoinResult.YinPo:
                Debug.Log("陰筊: 神明不同意");
                break;
            case CoinResult.YangPo:
                Debug.Log("陽筊: 需要再問一次");
                break;
        }
    }

    private void ResetPositions()
    {
        coinTossCamera.transform.position = initialCameraPosition;
        for (int i = 0; i < coins.Length; i++)
        {
            coins[i].transform.position = initialCoinPositions[i];
            coinRigidbodies[i].velocity = Vector3.zero;
            coinRigidbodies[i].angularVelocity = Vector3.zero;
        }
    }
}