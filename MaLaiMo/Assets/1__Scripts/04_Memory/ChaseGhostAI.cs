using UnityEngine;

public class ChaseGhostAI : MonoBehaviour
{
    [Header("=== 基本目標設定 ===")]
    public Transform playerTransform;
    public Transform startTriggerPoint;
    public Transform endTriggerPoint;

    [Header("=== 智慧轉向路點 ===")]
    [Tooltip("將走廊所有轉角的點都丟進來，鬼會根據障礙物自動選擇最適合的路點繞路")]
    public Transform[] hallwayWaypoints;

    [Header("=== 氛圍與速度控制 ===")]
    public float maxSpeed = 3.0f;
    public float minSpeed = 2.2f;
    public float proximityRange = 3.5f;
    public float catchDistance = 1.1f;

    [Header("=== 狀態顯示 ===")]
    public bool isChasing = false;
    private bool hasFinished = false;
    private Transform currentTarget;

    void Update()
    {
        if (hasFinished || playerTransform == null) return;

        // 1. 啟動偵測
        if (!isChasing && startTriggerPoint != null)
        {
            if (Vector3.Distance(playerTransform.position, startTriggerPoint.position) < 2.5f)
            {
                isChasing = true;
                Debug.Log("<color=red>【Console】鬼魂現身！開始針對性追逐...</color>");
            }
        }

        if (isChasing)
        {
            DetermineTarget();
            HandleMovement();
            CheckGameStatus();
        }
    }

    private void DetermineTarget()
    {
        // 使用射線檢查是否看得到玩家
        Vector3 directionToPlayer = (playerTransform.position + Vector3.up) - (transform.position + Vector3.up);
        bool canSeePlayer = !Physics.Raycast(transform.position + Vector3.up, directionToPlayer, directionToPlayer.magnitude);

        if (canSeePlayer)
        {
            // 如果看得到，玩家就是目標
            currentTarget = playerTransform;
        }
        else
        {
            // 如果被牆擋住，找出距離玩家最近的路點（轉角），先繞過去
            currentTarget = GetBestWaypoint();
        }
    }

    private Transform GetBestWaypoint()
    {
        Transform bestWP = null;
        float minDistance = float.MaxValue;

        foreach (Transform wp in hallwayWaypoints)
        {
            // 找出離玩家最近的路點，作為「繞路」的目標
            float dist = Vector3.Distance(wp.position, playerTransform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                bestWP = wp;
            }
        }
        return bestWP != null ? bestWP : playerTransform;
    }

    private void HandleMovement()
    {
        if (currentTarget == null) return;

        Vector3 targetPos = currentTarget.position;
        float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // 動態降速邏輯
        float currentSpeed = maxSpeed;
        if (distToPlayer < proximityRange)
        {
            float speedPercent = Mathf.Clamp01((distToPlayer - catchDistance) / (proximityRange - catchDistance));
            currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, speedPercent);
        }

        // 移動
        Vector3 moveDest = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        transform.position = Vector3.MoveTowards(transform.position, moveDest, currentSpeed * Time.deltaTime);

        // 轉向 (始終面向移動方向)
        Vector3 lookDir = moveDest - transform.position;
        if (lookDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 5f);
        }
    }

    private void CheckGameStatus()
    {
        float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distToPlayer <= catchDistance)
        {
            isChasing = false;
            hasFinished = true;
            Debug.Log("<color=black>【Console】死了（玩家被鬼魂截擊）</color>");
        }

        if (endTriggerPoint != null && Vector3.Distance(playerTransform.position, endTriggerPoint.position) < 2.0f)
        {
            isChasing = false;
            hasFinished = true;
            Debug.Log("<color=blue>【Console】逃脫成功！</color>");
        }
    }
}