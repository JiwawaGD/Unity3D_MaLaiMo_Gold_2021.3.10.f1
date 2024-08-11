using UnityEngine;
using System.Collections;

public class DivinationBlockSystem : MonoBehaviour
{
    public enum BlockResult
    {
        ShengPo,    // 聖筊
        YinPo,      // 陰筊
        YangPo      // 陽筊
    }

    [SerializeField] private GameObject block1;
    [SerializeField] private GameObject block2;
    [SerializeField] private float throwForce = 5f;
    [SerializeField] private float rotationForce = 2f;

    private Rigidbody rb1;
    private Rigidbody rb2;

    private void Start()
    {
        rb1 = block1.GetComponent<Rigidbody>();
        rb2 = block2.GetComponent<Rigidbody>();
    }

    public void ThrowBlocks()
    {
        StartCoroutine(ThrowBlocksCoroutine());
    }

    private IEnumerator ThrowBlocksCoroutine()
    {
        // 拋擲筊杯
        ThrowBlock(rb1);
        ThrowBlock(rb2);

        // 等待筊杯停止移動
        yield return new WaitForSeconds(3f);

        // 檢查結果
        BlockResult result = CheckResult();
        Debug.Log("擲筊結果: " + result.ToString());

        // 這裡可以添加根據結果觸發不同事件的邏輯
        TriggerResultEvent(result);
    }

    private void ThrowBlock(Rigidbody rb)
    {
        rb.AddForce(Vector3.up * throwForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * rotationForce, ForceMode.Impulse);
    }

    private BlockResult CheckResult()
    {
        bool isUp1 = IsBlockFaceUp(block1);
        bool isUp2 = IsBlockFaceUp(block2);

        if (isUp1 != isUp2) return BlockResult.ShengPo;
        if (isUp1 && isUp2) return BlockResult.YangPo;
        return BlockResult.YinPo;
    }

    private bool IsBlockFaceUp(GameObject block)
    {
        // 這裡的邏輯需要根據3D模型來調整
        return block.transform.up.y > 0;
    }

    private void TriggerResultEvent(BlockResult result)
    {
        switch (result)
        {
            case BlockResult.ShengPo:
                Debug.Log("聖筊: 神明同意");
                
                break;
            case BlockResult.YinPo:
                Debug.Log("陰筊: 神明不同意");
                
                break;
            case BlockResult.YangPo:
                Debug.Log("陽筊: 需要再問一次");
                
                break;
        }
    }
}