using UnityEngine;
using UnityEngine.AI; // 必須引用導航系統

[RequireComponent(typeof(NavMeshAgent))]
public class ChaseGhostAI : MonoBehaviour
{
    [Header("=== 目標設定 ===")]
    public Transform playerTransform;   // 拖入玩家
    public Transform startTriggerPoint; // 拖入紅框物件
    public Transform endTriggerPoint;   // 拖入藍框物件

    [Header("=== 氛圍速度控制 ===")]
    public float maxSpeed = 3.5f;       // 正常追逐速度
    public float minSpeed = 2.4f;       // 靠近玩家時降速
    public float proximityRange = 4.0f; // 多近開始降速
    public float catchDistance = 1.1f;  // 抓到距離

    private NavMeshAgent agent;
    private bool isChasing = false;
    private bool hasFinished = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // 初始關閉 Agent，防止在觸發前噴錯
        agent.enabled = false;
        Debug.Log("<color=cyan>【追逐系統】NavMesh 已就緒，等待紅框觸發...</color>");
    }

    void Update()
    {
        if (hasFinished || playerTransform == null) return;

        // 1. 紅框啟動偵測
        if (!isChasing && startTriggerPoint != null)
        {
            if (Vector3.Distance(playerTransform.position, startTriggerPoint.position) < 2.5f)
            {
                isChasing = true;
                agent.enabled = true; // 正式啟動導航
                Debug.Log("<color=red>【Console】鬼魂現身！開始追逐！</color>");
            }
        }

        // 2. 追逐邏輯
        if (isChasing && agent.isActiveAndEnabled)
        {
            float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // --- 動態氛圍降速 ---
            float currentSpeed = maxSpeed;
            if (distToPlayer < proximityRange)
            {
                float speedPercent = Mathf.Clamp01((distToPlayer - catchDistance) / (proximityRange - catchDistance));
                currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, speedPercent);
            }
            agent.speed = currentSpeed;

            // --- 設定目標（NavMesh 會自動處理轉彎）---
            agent.SetDestination(playerTransform.position);

            // --- 判定邏輯 ---
            // A. 抓到玩家
            if (distToPlayer <= catchDistance)
            {
                isChasing = false;
                hasFinished = true;
                agent.isStopped = true;
                Debug.Log("<color=black>【Console】死了（被抓住了）</color>");
            }

            // B. 逃脫成功
            if (endTriggerPoint != null && Vector3.Distance(playerTransform.position, endTriggerPoint.position) < 2.0f)
            {
                isChasing = false;
                hasFinished = true;
                agent.enabled = false;
                Debug.Log("<color=blue>【Console】逃脫成功！抵達藍色區域。</color>");
            }
        }
    }
}