using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmaRoom_Player : PlayerController
{
    public static SceneController_Room Instance;
    private void OnTriggerEnter(Collider other)
    {
        // 檢查碰到的物件名稱是否為 "setNightDiaObject"
        if (other.name == "setNightDiaObject")
        {
            other.gameObject.SetActive(false);
            Instance.PlayDialogue((byte)Room_Dialogue.Lv2_000_Begin);
            Instance.ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_FilialPietyCurtain);
            Debug.Log("已成功關閉 setNightDiaObject 物件！");
        }
        else if (other.name == "setBackLivingRoomObject") {
            other.gameObject.SetActive(false);
            Instance.PlayDialogue((byte)Room_Dialogue.Lv2_005_BackLivingRoom);
            Instance.ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv2_FlashLight);
        }
    }
}
