using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform player; // 玩家角色的 Transform

    void Update()
    {
        if (player != null)
        {
            // 讓物件正面朝向玩家
            Vector3 direction = player.position - transform.position;
            direction.y = 0; // 鎖定 Y 軸，保持垂直方向不旋轉
            transform.rotation = Quaternion.LookRotation(-direction);
        }
    }
}
