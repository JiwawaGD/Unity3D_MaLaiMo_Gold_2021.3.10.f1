using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TextUpdater : MonoBehaviour
{
    public Text myText; // 通过Inspector将Text组件分配给这个变量

    private Dictionary<KeyCode, string> keyTextMap = new Dictionary<KeyCode, string>();

    private void Start()
    {
        keyTextMap.Add(KeyCode.Q, "當前目標:調查房間");
        keyTextMap.Add(KeyCode.W, "當前目標:找尋鑰匙");
        keyTextMap.Add(KeyCode.E, "當前目標:周遭尋找線索");
        keyTextMap.Add(KeyCode.R, "當前目標:找出聲音來源");
        keyTextMap.Add(KeyCode.T, "當前目標:回到客廳");
        keyTextMap.Add(KeyCode.Y, "當前目標:調查廚房的人影");
    }

    private void Update()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        foreach (var kvp in keyTextMap)
        {
            if (Input.GetKeyDown(kvp.Key))
            {
                myText.text = kvp.Value;
                break; // 停止检查其他键
            }
        }
    }
}
