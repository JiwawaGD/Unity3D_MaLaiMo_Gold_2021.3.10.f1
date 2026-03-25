using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ChaseGhostAI : MonoBehaviour
{
    [Header("=== 目標與偵測 ===")]
    public Transform playerTransform;   // 玩家物件
    public Transform startTriggerPoint; // 啟動點 (紅框)
    public Transform endTriggerPoint;   // 終點 (藍框)
    public float startDetectionRange = 8.0f; // 提高啟動偵測範圍 (原為 2.5)

    [Header("=== 動畫設定 ===")]
    public Animator ghostAnimator;      // 拖入 Ghostmo_anim_crawl

    [Header("=== 速度與物理靈敏度 ===")]
    public float maxSpeed = 5.0f;       // 提高最大速度 (原為 3.5)
    public float minSpeed = 2.8f;       // 靠近玩家時的最低速 (原為 2.4)
    public float acceleration = 12.0f;  // 加速度 (讓它啟動與轉彎更猛)
    public float angularSpeed = 450f;   // 轉向速度 (讓它轉彎更靈敏)
    public float proximityRange = 5.0f; // 多近開始降速
    public float catchDistance = 1.2f;  // 抓到距離

    [Header("=== AI 聰明度 (預判) ===")]
    [Range(0, 1)] public float predictiveLead = 0.4f; // 預判強度 (0=追屁股, 1=攔截前方)

    private NavMeshAgent agent;
    private bool isChasing = false;
    private bool hasFinished = false;
    private Vector3 lastPlayerPos;
    private Vector3 playerVelocity;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // 初始化 Agent 物理屬性，提升聰明感與靈敏度
        agent.speed = maxSpeed;
        agent.acceleration = acceleration;
        agent.angularSpeed = angularSpeed;
        agent.stoppingDistance = 0.5f;
        agent.enabled = false;

        // 如果沒放 Animator，自動抓子物件
        if (ghostAnimator == null) ghostAnimator = GetComponentInChildren<Animator>();

        if (playerTransform != null) lastPlayerPos = playerTransform.position;

        Debug.Log("<color=cyan>【追逐系統】AI 已強化，等待觸發...</color>");
    }

    void Update()
    {
        if (hasFinished || playerTransform == null)
        {
            UpdateAnimation(false);
            return;
        }

        // 計算玩家當前速度 (用於預判)
        playerVelocity = (playerTransform.position - lastPlayerPos) / Time.deltaTime;
        lastPlayerPos = playerTransform.position;

        // 1. 啟動偵測 (擴大範圍)
        if (!isChasing && startTriggerPoint != null)
        {
            float distToStart = Vector3.Distance(playerTransform.position, startTriggerPoint.position);
            if (distToStart < startDetectionRange)
            {
                isChasing = true;
                agent.enabled = true;
                Debug.Log("<color=red>【Console】鬼魂感知範圍擴大！開始攔截！</color>");
            }
        }

        // 2. 追逐邏輯
        if (isChasing && agent.isActiveAndEnabled)
        {
            float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // --- A. 動態速度調整 (保持氛圍) ---
            float currentSpeed = maxSpeed;
            if (distToPlayer < proximityRange)
            {
                float speedPercent = Mathf.Clamp01((distToPlayer - catchDistance) / (proximityRange - catchDistance));
                currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, speedPercent);
            }
            agent.speed = currentSpeed;

            // --- B. 聰明預判邏輯 ---
            // 不只是追玩家座標，而是追玩家即將到達的位置
            Vector3 targetDestination = playerTransform.position + (playerVelocity * predictiveLead);
            agent.SetDestination(targetDestination);

            // --- C. 動態動畫更新 ---
            bool isMoving = agent.velocity.magnitude > 0.2f;
            UpdateAnimation(isMoving);

            // --- D. 判定邏輯 ---
            // 抓到玩家
            if (distToPlayer <= catchDistance)
            {
                EndChase("<color=black>【Console】抓到了！玩家死亡。</color>");
            }

            // 逃脫成功
            if (endTriggerPoint != null && Vector3.Distance(playerTransform.position, endTriggerPoint.position) < 2.5f)
            {
                EndChase("<color=blue>【Console】玩家逃脫！AI 停止。</color>");
            }
        }
    }

    void EndChase(string message)
    {
        isChasing = false;
        hasFinished = true;
        if (agent.isActiveAndEnabled) agent.isStopped = true;
        UpdateAnimation(false);
        Debug.Log(message);
    }

    void UpdateAnimation(bool moving)
    {
        if (ghostAnimator != null)
        {
            ghostAnimator.SetBool("isMoving", moving);
        }
    }
}