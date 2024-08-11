using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    public DivinationBlockSystem divinationSystem;
    public Button throwButton;

    void Start()
    {
        if (throwButton != null)
        {
            throwButton.onClick.AddListener(ThrowBlocks);
        }
    }

    void ThrowBlocks()
    {
        if (divinationSystem != null)
        {
            divinationSystem.ThrowBlocks();
        }
        else
        {
            Debug.LogError("Divination System not assigned!");
        }
    }
}