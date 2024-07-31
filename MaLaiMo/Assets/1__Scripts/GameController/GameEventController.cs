using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    #region - 通用 Function -
    void CloseUI()
    {
        UIState(UIItemID.Empty, false);
        ShowEnterGame(false);
        audManager.Play(1, "ui_Context", false);

        // UI 返回後執行玩家動畫
        if (m_bShowPlayerAnimate)
            ProcessPlayerAnimator(GlobalDeclare.GetPlayerAnimateType().ToString());

        // UI 返回後執行 Item 動畫
        if (m_bShowItemAnimate)
            ProcessAnimator(GlobalDeclare.GetItemAniObject(), GlobalDeclare.GetItemAniName());

        // 鎖定玩家視角旋轉
        if (m_bSetPlayerViewLimit)
            SetPlayerViewLimit(true, GlobalDeclare.PlayerCameraLimit.GetPlayerCameraLimit());

        if (bNeedShowDialog)
        {
            DialogueObjects[GlobalDeclare.byCurrentDialogIndex].CallAction();
            bNeedShowDialog = false;
        }
    }
    #endregion

    #region - 場景一 Event -
    void Lv1_PhotoFrameEvent()
    {
        ProcessAnimator("Lv2_Photo_Frame", "Lv2_PutPhotoFrameBack");


        Invoke(nameof(Delay_Lv2_BrokenPhotoFrameEnable), 2.5f);
    }

    void Lv1_GrandmaDoorOpen()
    {
        Debug.Log("場景1 ==> 打開阿嬤房間門 (Lv1_Grandma_Door_Open)");

        ProcessAnimator("Lv1_Grandma_Room_Door", "DoorOpen");
        ShowHint(HintItemID.Lv1_Rice_Funeral);
        DialogueObjects[(byte)Lv1_Dialogue.OpenDoor_GetKey_Lv1].CallAction();
    }

    void Lv1_LotusPaper()
    {
        Debug.Log("場景1 ==> 點擊桌上的蓮花紙 (Lv1_Lotus_Paper)");

        ProcessRoMoving(1);
        UIState(UIItemID.Lv1_Lotus_Paper, true);
        ShowEnterGame(true);
        ShowObj(ObjItemID.Lv1_Lotus_Paper);
        audManager.Play(1, "gold_Paper", false);
        DialogueObjects[(byte)Lv1_Dialogue.CheckLotus_Lv1].CallAction();
    }

    void Lv1_FinishedLotusPaper()
    {
        Debug.Log("場景1 ==> 拿起摺好的蓮花 (Lv1_Finished_Lotus_Paper)");

        Lv1_Finished_Lotus_Paper_Obj.GetComponent<BoxCollider>().enabled = false;
        Lv1_Finished_Lotus_Paper_Obj.transform.parent = playerCtrlr.transform;
        Lv1_Finished_Lotus_Paper_Obj.transform.localPosition = new Vector3(0, 0.05f, 0.6f);
        Lv1_Finished_Lotus_Paper_Obj.transform.localRotation = new Quaternion(0, 0, 0, 0);

        ShowHint(HintItemID.Lv1_Lotus_Paper_Plate);
    }

    void Lv1_LotusPaperPlate()
    {
        Debug.Log("場景1 ==> 放紙蓮花到盤子上 (Lv1_Lotus_Paper_Plate)");

        Lv1_Finished_Lotus_Paper_Obj.transform.parent = Lv1_Lotus_Paper_Plate_Obj.transform;
        Lv1_Finished_Lotus_Paper_Obj.transform.localPosition = new Vector3(0, 0.05f, 0);
        Lv1_Finished_Lotus_Paper_Obj.transform.localRotation = new Quaternion(0, 0, 0, 0);
        Lv1_Finished_Lotus_Paper_Obj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        TempItem = GameObject.Find("Lv1_Toilet_Door_Ghost").GetComponent<ItemController>();
        TempItem.bAlwaysActive = false;
        TempItem.EventID = GameEventID.Lv1_Toilet_Door_Open;
        audManager.FlushSound.Play();
        DialogueObjects[(byte)Lv1_Dialogue.HeardBathRoomSound_Lv1].CallAction();
    }


    void Lv1_CheckFilialPietyCurtain()
    {
        Debug.Log("場景1 ==> 先去看看其他地方吧 (Lv1_CheckFilialPietyCurtain)");

        DialogueObjects[(byte)Lv1_Dialogue.CheckFilialPietyCurtain_Lv1].CallAction();
    }

    void Lv1_Piano()
    {
        Debug.Log("場景1 ==> 查看鋼琴 (Lv1_CheckPiano)");
        StartCoroutine(ProcessPlayerSetPianoAni(0));
    }

    void Lv1_Flashlight()
    {
        Debug.Log("場景1 ==> 觸發阿嬤房間手電筒 (Lv1_Flashlight)");

        bLv1_HasFlashlight = true;

        Light playerFlashlight = playerCtrlr.tfPlayerCamera.GetComponent<Light>();
        playerFlashlight.enabled = true;
        audManager.Play(1, "light_Switch_Sound", false);
        GameObject FlashLight = GameObject.Find("Lv1_Flashlight");
        Destroy(FlashLight);

        if (bLv1_HasGrandmaRoomKey)
            ShowHint(HintItemID.Lv1_Grandma_Room_Door);
    }


    void Lv1_GrandmaRoomDoorLock()
    {
        Debug.Log("場景1 ==> 阿嬤房間門鎖住 (Lv1_Grandma_Room_Door_Lock)");

        // (無key) ==> 鎖住了
        if (!bLv1_HasGrandmaRoomKey)
        {
            DialogueObjects[(byte)Lv1_Dialogue.OpenDoor_Nokey_Lv1].CallAction();
            return;
        }

        if (!bLv1_HasFlashlight)
            DialogueObjects[(byte)Lv1_Dialogue.OpenDoor_NoFlashLight_Lv1].CallAction();

    }


    void Lv1_ToiletDoorLock()
    {
        Debug.Log("場景1 ==> 廁所門鎖住了 (Lv1_Toilet_Door_Lock)");

        audManager.Play(1, "the_door_is_locked_and_cannot_be_opened_with_sound_effects", false);
        DialogueObjects[(byte)Lv1_Dialogue.OpenBathRoomDoor_Nokey_Lv1].CallAction();
    }

    void Lv1_ToiletDoorOpen()
    {
        Debug.Log("場景1 ==> 打開廁所門 (Lv1_Toilet_Door_Open)");

        audManager.Play(1, "the_toilet_door_opens", false);
        ProcessAnimator("Lv1_Toilet_Door_Ghost", "Toilet_Door_Open");
        BoxCollider ToiletDoorCollider = GameObject.Find("Lv1_Toilet_Door_Ghost").GetComponent<BoxCollider>();
        ToiletDoorCollider.enabled = false;
        Lv1_Faucet_Flush_Obj.SetActive(true);
        ShowHint(HintItemID.Lv1_Faucet);
    }

    void Lv1_ToiletGhostHandPush()
    {
        Debug.Log("場景1 ==> 廁所鬼手推玩家 (Lv1_Toilet_Ghost_Hand_Push)");

        Transform tfToiletPos = GameObject.Find("Lv1_Player_Toilet_Pos").GetComponent<Transform>();
        Transform tfCameraPos = tfToiletPos.GetChild(0);
        StartCoroutine(PlayerToAniPos(tfToiletPos.position, tfToiletPos.rotation, tfCameraPos.rotation));

        Invoke(nameof(IvkProcessGhostHandPushAnimator), 2f);
    }

    void Lv1_Faucet()
    {
        Debug.Log("場景1 ==> 水龍頭 (Lv1_Faucet)");
        audManager.Play(2, "CloseFaucet", false);
        audManager.FlushSound.Stop();
        Lv1_Faucet_Flush_Obj.SetActive(false);

        Material matFaucetWaterSurface = WaterSurfaceObj.GetComponent<MeshRenderer>().material;
        matFaucetWaterSurface.SetFloat("_RefractionSpeed", 0.01f);
        matFaucetWaterSurface.SetFloat("_RefractionScale", 0.1f);
        matFaucetWaterSurface.SetFloat("_RefractionStrength", 0.1f);
        matFaucetWaterSurface.SetFloat("_FoamAmount", 0.01f);
        matFaucetWaterSurface.SetFloat("_FoamCutOff", 1.2f);
        matFaucetWaterSurface.SetFloat("_FoamSpeed", 0.1f);
        matFaucetWaterSurface.SetFloat("_FoamScale", 0.3f);

        ProcessAnimator("Lv1_Toilet_Door_Ghost", "Toilet_Door_Close");
        audManager.Play(1, "door_Close", false);

        Invoke(nameof(IvkLv1_SetGhostHandPosition), 3f);
    }

    public void Lv1_ShowTVWhiteNoise()
    {
        GameObject Lv1_TVObj = GameObject.Find("Lv1_TV");
        audManager.Play(2, "TV_Noise", false);
        Lv1_TVObj.transform.GetChild(1).GetComponent<MeshRenderer>().material = Lv1_matTVWhiteNoise;
        Lv1_TVObj.transform.GetChild(2).GetComponent<Light>().enabled = true;
    }

    public void Lv1_DollTurnAround()
    {
        Lv1_Doll_Ani.SetBool("TurnHead", true);
        audManager.Play(2, "CandleFallDollTurnAround", false);
    }

    public void Lv1_CandleFall()
    {
        ProcessAnimator("LV1_Lotus_Candles/Lv1_Falled_Candle", "Candle_Fall");
        audManager.Play(2, "CandleFall", false);
    }
    #endregion

    #region - 場景二 Event -
    void Lv2_LightSwitch()
    {
        Debug.Log("場景2 ==> 房間燈開關 (Lv2_Light_Switch)");

        audManager.Play(1, "light_Switch_Sound", false);
        DialogueObjects[(byte)Lv1_Dialogue.OpenLight_Lv2].CallAction();
    }

    void Lv2_RoomDoorLock()
    {
        Debug.Log("場景2 ==> 房間門鎖住了 (Lv2_Room_Door_Lock)");

        audManager.Play(1, "the_door_is_locked_and_cannot_be_opened_with_sound_effects", false);

        if (!bLv2_HasGrandmaRoomKey)
        {
            DialogueObjects[(byte)Lv1_Dialogue.DoorLocked_Lv2].CallAction();
            return;
        }

        if (!bLv2_HasFlashlight)
        {
            DialogueObjects[(byte)Lv1_Dialogue.OpenDoor_NoFlashLight_Lv1].CallAction();
        }

    }
    void Lv2_FlashLight()
    {
        Debug.Log("場景2 ==> 手電筒 (Lv2_FlashLight)");

        bLv2_HasFlashlight = true;

        audManager.Play(1, "light_Switch_Sound", false);
        DialogueObjects[(byte)Lv1_Dialogue.OpenFlashLight_Lv2].CallAction();

        GameObject Lv2_FlashLightObj = GameObject.Find("Lv2_FlashLight");
        Destroy(Lv2_FlashLightObj);

        if (bLv2_HasGrandmaRoomKey)
            ShowHint(HintItemID.Lv2_Grandma_Room_Door_Open);
    }

    void Lv2_DoorKnockStop()
    {
        Debug.Log("場景2 ==> 門的撞擊聲暫停 (Lv2_Door_Knock_Stop)");

        audManager.Play(1, "emergency_Knock_On_The_Door", false);
        ShowHint(HintItemID.Lv2_Grandma_Room_Door_Open);
    }

    void Lv2_GrandmaDoorOpen()
    {
        Debug.Log("場景2 ==> 房間門打開 (Lv2_Grandma_Door_Open)");

        ProcessAnimator("Lv2_Grandma_Room_Door", "Lv2_Grandma_Room_Door_Open");
        audManager.Play(1, "the_sound_of_the_eyes_opening_the_door", false);
    }

    void Lv2_GrandmaDoorClose()
    {
        Debug.Log("場景2 ==> 阿嬤房門用力關閉 (Lv2_Grandma_Door_Close)");

        audManager.Play(1, "door_Close", false);
        ProcessAnimator("Lv2_Grandma_Room_Door", "Lv2_Grandma_Room_Door_Close");
    }
    #endregion
}