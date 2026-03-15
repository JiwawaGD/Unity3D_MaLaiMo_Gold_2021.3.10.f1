using UnityEngine;
using UnityEngine.AI; // 必須引用導航系統

[RequireComponent(typeof(NavMeshAgent))]
public class ChaseGhostAI : MonoBehaviour
{
    [Header("=== 基本目標設定 ===")]
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

        // 初始務必關閉 Agent，防止在尚未觸發前因為沒踩在網格上而報錯
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
                StartChase();
            }
        }

        // 2. 追逐邏輯 (增加安全性檢查：確保 Agent 已啟動且在網格上)
        if (isChasing && agent.enabled && agent.isOnNavMesh)
        {
            HandleChaseMovement();
        }
    }

    private void StartChase()
    {
        // 尋找最近的導航點，確保啟動時不會報錯
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 3.0f, NavMesh.AllAreas))
        {
            // 先定位 Transform
            transform.position = hit.position;
            // 再啟動 Agent
            agent.enabled = true;
            isChasing = true;
            Debug.Log("<color=red>【Console】鬼魂已啟動並開始追逐！</color>");
        }
        else
        {
            Debug.LogWarning("【追逐系統】鬼魂離藍色區域太遠，請手動將鬼魂移近地板！");
        }
    }

    private void HandleChaseMovement()
    {
        float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // --- 動態氛圍降速 (當鬼魂靠近玩家時會稍微放慢) ---
        float currentSpeed = maxSpeed;
        if (distToPlayer < proximityRange)
        {
            float speedPercent = Mathf.Clamp01((distToPlayer - catchDistance) / (proximityRange - catchDistance));
            currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, speedPercent);
        }
        agent.speed = currentSpeed;

        // --- 設定目標 ---
        agent.SetDestination(playerTransform.position);

        // --- 判定邏輯 ---
        // A. 抓到玩家
        if (distToPlayer <= catchDistance)
        {
            EndChase("<color=black>【Console】死了（被抓住了）</color>");
        }

        // B. 逃脫成功
        if (endTriggerPoint != null && Vector3.Distance(playerTransform.position, endTriggerPoint.position) < 2.5f)
        {
            EndChase("<color=blue>【Console】逃脫成功！抵達藍色區域。</color>");
        }
    }

    private void EndChase(string message)
    {
        isChasing = false;
        hasFinished = true;
        if (agent.isActiveAndEnabled)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }
        Debug.Log(message);
    }
}