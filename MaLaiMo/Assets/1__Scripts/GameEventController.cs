using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    #region - 場景一 Event -
    void S1_PhotoFrameEvent()
    {
        Debug.Log("場景1 ==> 正常相框 (S1_Photo_Frame)");

        ProcessAnimator("Lv2_Photo_Frame", "Lv2_PutPhotoFrameBack");

        DialogueObjects[(byte)Lv1_Dialogue.Lv2_PutPhotoFrameBack].CallAction();

        Invoke(nameof(IvkLv2_BrokenPhotoFrameEnable), 2.5f);
    }

    void S1_PhotoFrameHasBroken()
    {
        Debug.Log("場景1 ==> 破碎相框 (S1_Photo_Frame_Has_Broken)");

        audManager.Play(1, "get_Item_Sound", false);
        UIState(UIItemID.S1_Photo_Frame, true);
        ProcessRoMoving(2);
        ShowObj(ObjItemID.S1_Photo_Frame);

        Lv2_BrotherShoe_Obj.transform.localPosition = new Vector3(-4.4f, 0f, 48.8f);

        //GlobalDeclare.byCurrentDialogIndex = (byte)Lv1_Dialogue.Lv2_Boy_Sneaker;
        //bNeedShowDialog = true;

        ShowHint(HintItemID.Lv2_Boy_Sneaker);
    }

    void S1_GrandmaDoorOpen()
    {
        Debug.Log("場景1 ==> 打開阿嬤房間門 (S1_Grandma_Door_Open)");

        ProcessAnimator("Lv1_Grandma_Room_Door", "DoorOpen");
        ShowHint(HintItemID.S1_Rice_Funeral);
        DialogueObjects[(byte)Lv1_Dialogue.OpenDoor_GetKey_Lv1].CallAction();
    }

    void S1_LotusPaper()
    {
        Debug.Log("場景1 ==> 點擊桌上的蓮花紙 (S1_Lotus_Paper)");

        ProcessRoMoving(1);
        UIState(UIItemID.S1_Lotus_Paper, true);
        ShowEnterGame(true);
        ShowObj(ObjItemID.S1_Lotus_Paper);
        audManager.Play(1, "gold_Paper", false);
        DialogueObjects[(byte)Lv1_Dialogue.CheckLotus_Lv1].CallAction();
    }

    void S1_FinishedLotusPaper()
    {
        Debug.Log("場景1 ==> 拿起摺好的蓮花 (S1_Finished_Lotus_Paper)");

        S1_Finished_Lotus_Paper_Obj.GetComponent<BoxCollider>().enabled = false;
        S1_Finished_Lotus_Paper_Obj.transform.parent = playerCtrlr.transform;
        S1_Finished_Lotus_Paper_Obj.transform.localPosition = new Vector3(0, 0.05f, 0.6f);
        S1_Finished_Lotus_Paper_Obj.transform.localRotation = new Quaternion(0, 0, 0, 0);

        ShowHint(HintItemID.S1_Lotus_Paper_Plate);
    }

    void S1_LotusPaperPlate()
    {
        Debug.Log("場景1 ==> 放紙蓮花到盤子上 (S1_Lotus_Paper_Plate)");

        S1_Finished_Lotus_Paper_Obj.transform.parent = S1_Lotus_Paper_Plate_Obj.transform;
        S1_Finished_Lotus_Paper_Obj.transform.localPosition = new Vector3(0, 0.05f, 0);
        S1_Finished_Lotus_Paper_Obj.transform.localRotation = new Quaternion(0, 0, 0, 0);
        S1_Finished_Lotus_Paper_Obj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        TempItem = GameObject.Find("Lv1_Toilet_Door_Ghost").GetComponent<ItemController>();
        TempItem.bAlwaysActive = false;
        TempItem.eventID = GameEventID.S1_Toilet_Door_Open;
        audManager.FlushSound.Play();
        DialogueObjects[(byte)Lv1_Dialogue.HeardBathRoomSound_Lv1].CallAction();
    }

    void S1_GrandmaDeadBody()
    {
        Debug.Log("場景1 ==> 跟孝濂內的阿嬤互動 (S1_Grandma_Dead_Body)");

        S1_Rice_Funeral_Obj.SetActive(false);
        DialogueObjects[(byte)Lv1_Dialogue.OpenFilialPietyCurtain_Lv1].CallAction();
        StopReadding();

        // 生成摔壞的腳尾飯
        UnityEngine.Object RiceFuneralSpilled = Resources.Load<GameObject>("Prefabs/Rice_Funeral_Spilled");
        GameObject RiceFuneralSpilledObj = Instantiate(RiceFuneralSpilled) as GameObject;
        RiceFuneralSpilledObj.transform.parent = GameObject.Find("===== ITEMS =====").transform;
        RiceFuneralSpilledObj.transform.position = new Vector3(-4.4f, 0.006f, 11.8f);
        RiceFuneralSpilledObj.name = "Rice_Funeral_Spilled";
        ShowHint(HintItemID.S1_Rice_Funeral_Spilled);
    }

    void S1_WhiteTent()
    {
        Debug.Log("場景1 ==> 觸發孝濂 (S1_White_Tent)");

        audManager.Play(1, "filial_Piety_Curtain", false);
        ProcessAnimator("Lv1_Filial_Piety_Curtain", "Filial_piety_curtain Open");
        TempBoxCollider = GameObject.Find("Lv1_Filial_Piety_Curtain").GetComponent<BoxCollider>();
        TempBoxCollider.enabled = false;
        ShowHint(HintItemID.S1_Lie_Grandma_Body);
    }

    void Lv1_CheckFilialPietyCurtain()
    {
        Debug.Log("場景1 ==> 先去看看其他地方吧 (Lv1_CheckFilialPietyCurtain)");

        DialogueObjects[(byte)Lv1_Dialogue.CheckFilialPietyCurtain_Lv1].CallAction();
    }

    void Lv1_CheckPiano()
    {
        Debug.Log("場景1 ==> 查看鋼琴 (Lv1_CheckPiano)");
        StartCoroutine(ProcessPlayerSetPianoAni(0));
    }

    void S1_LightSwitch()
    {
        Debug.Log("場景1 ==> 觸發阿嬤房間燈開關 (S1_Light_Switch)");

        if (bLv1_HasFlashlight)
            DialogueObjects[(byte)Lv1_Dialogue.Lv1_OpenLight_HasFlashlight].CallAction();
        else
            DialogueObjects[(byte)Lv1_Dialogue.OpenLight_Lv1].CallAction();

        audManager.Play(1, "light_Switch_Sound", false);
    }

    void S1_Flashlight()
    {
        Debug.Log("場景1 ==> 觸發阿嬤房間手電筒 (S1_Flashlight)");

        bLv1_HasFlashlight = true;

        Light playerFlashlight = playerCtrlr.tfPlayerCamera.GetComponent<Light>();
        playerFlashlight.enabled = true;
        audManager.Play(1, "light_Switch_Sound", false);
        GameObject FlashLight = GameObject.Find("Lv1_Flashlight");
        Destroy(FlashLight);

        if (bLv1_HasGrandmaRoomKey)
            ShowHint(HintItemID.S1_Grandma_Room_Door);
    }

    void S1_DeskDrawer()
    {
        Debug.Log("場景1 ==> 阿嬤房間抽屜 (S1_Desk_Drawer)");

        audManager.Play(1, "drawer_Opening_Sound", false);
        TempBoxCollider = Lv1_Desk_Drawer_Item.GetComponent<BoxCollider>();
        //TempBoxCollider = GameObject.Find("grandpa_desk/Desk_Drawer").GetComponent<BoxCollider>();
        TempBoxCollider.enabled = false;
        ProcessAnimator("grandpa_desk/Lv1_Desk_Drawer", "DrawerWithKey_Open");
        TempGameObject = GameObject.Find("Grandma_Room_Key");
        TempGameObject.GetComponent<Animation>().Play();
        Invoke(nameof(IvkShowDoorKey), 1.2f);
    }

    void S1_GrandmaRoomKey()
    {
        Debug.Log("場景1 ==> 阿嬤房間抽屜鑰匙 (S1_GrandmaRoomKey)");

        bLv1_HasGrandmaRoomKey = true;
        DialogueObjects[(byte)Lv1_Dialogue.GetKey_Lv1].CallAction();
        GameObject GrandmaRoomKeyObj = GameObject.Find("Grandma_Room_Key");
        Destroy(GrandmaRoomKeyObj);

        if (bLv1_HasFlashlight)
            ShowHint(HintItemID.S1_Grandma_Room_Door);
    }

    void S1_GrandmaRoomDoorLock()
    {
        Debug.Log("場景1 ==> 阿嬤房間門鎖住 (S1_Grandma_Room_Door_Lock)");

        // (無key) ==> 鎖住了
        if (!bLv1_HasGrandmaRoomKey)
        {
            DialogueObjects[(byte)Lv1_Dialogue.OpenDoor_Nokey_Lv1].CallAction();
            return;
        }

        if (!bLv1_HasFlashlight)
            DialogueObjects[(byte)Lv1_Dialogue.OpenDoor_NoFlashLight_Lv1].CallAction();

        //audManager.Play(1, "the_door_is_locked_and_cannot_be_opened_with_sound_effects", false);
    }

    void S1_RiceFuneralSpilled()
    {
        Debug.Log("場景1 ==> 腳尾飯打翻 (S1_Rice_Funeral_Spilled)");

        ShowHint(HintItemID.S1_Lotus_Paper);
        m_bPlayLotusEnable = true;
        DialogueObjects[(byte)Lv1_Dialogue.CheckRiceFuneral_OnFloor_Lv1].CallAction();

        // 移動蓮花紙 & 蠟燭座標
        S1_Lotus_Paper_Obj.transform.localPosition = new Vector3(-3.9f, 0.6f, -2.4f);
        S1_Lotus_Candle_Obj.transform.localPosition = new Vector3(-4f, 0.58f, -2.1f);
    }

    void S1_RiceFuneral()
    {
        Debug.Log("場景1 ==> 桌上的腳尾飯 (S1_Rice_Funeral)");

        bLv1_TriggerRiceFuneral = true;

        audManager.Play(1, "get_Item_Sound", false);
        ShowHint(HintItemID.S1_Filial_Piety_Curtain);
        UIState(UIItemID.S1_Rice_Funeral, true);
        ShowObj(ObjItemID.S1_Rice_Funeral);
        ProcessRoMoving(0);

        TempGameObject = GameObject.Find("S1_Grandma_Pass_Door_Trigger");
        TempGameObject.transform.localPosition = new Vector3(-5f, 0.5f, 8f);
    }

    void S1_GrandmaPassDoorAfterRiceFurnel()
    {
        Debug.Log("場景1 ==> 鬼阿嬤從門前衝過 (S1_GrandmaPassDoorAfterRiceFurnel)");

        S2_Grandma_Ghost_Obj.GetComponent<Animator>().applyRootMotion = false;
        S2_Grandma_Ghost_Obj.GetComponent<Animator>().SetTrigger("S1_Grandma_Pass_Door");
        Invoke(nameof(IvkS1_SetGrandmaGhostPosition), 2.2f);
    }

    void S1_ToiletDoorLock()
    {
        Debug.Log("場景1 ==> 廁所門鎖住了 (S1_Toilet_Door_Lock)");

        audManager.Play(1, "the_door_is_locked_and_cannot_be_opened_with_sound_effects", false);
        DialogueObjects[(byte)Lv1_Dialogue.OpenBathRoomDoor_Nokey_Lv1].CallAction();
    }

    void S1_ToiletDoorOpen()
    {
        Debug.Log("場景1 ==> 打開廁所門 (S1_Toilet_Door_Open)");

        audManager.Play(1, "the_toilet_door_opens", false);
        ProcessAnimator("Lv1_Toilet_Door_Ghost", "Toilet_Door_Open");
        BoxCollider ToiletDoorCollider = GameObject.Find("Lv1_Toilet_Door_Ghost").GetComponent<BoxCollider>();
        ToiletDoorCollider.enabled = false;
        Lv1_Faucet_Flush_Obj.SetActive(true);
        ShowHint(HintItemID.Lv1_Faucet);
    }

    void S1_ToiletGhostHide()
    {
        // 暫時沒用到
        return;

        Debug.Log("場景1 ==> 廁所鬼頭縮回門後 (S1_Toilet_Ghost_Hide)");

        ProcessAnimator("Lv1_Toilet_Door_Ghost", "Toilet_Door_Ghost_Out");
        m_bToiletGhostHasShow = false;
        ShowHint(HintItemID.S1_Toilet_GhostHand_Trigger);

        Invoke(nameof(IvkS1_SetGhostHandPosition), 2f);

        GameObject GhostHandTriggerObj = GameObject.Find("Ghost_Hand_Trigger");
        GhostHandTriggerObj.transform.position = new Vector3(-8.5f, 0.1f, 6.1f);
    }

    void S1_ToiletGhostHandPush()
    {
        Debug.Log("場景1 ==> 廁所鬼手推玩家 (S1_Toilet_Ghost_Hand_Push)");

        Transform tfToiletPos = GameObject.Find("Lv1_Player_Toilet_Pos").GetComponent<Transform>();
        Transform tfCameraPos = tfToiletPos.GetChild(0);
        StartCoroutine(PlayerToAniPos(tfToiletPos.position, tfToiletPos.rotation, tfCameraPos.rotation));

        Invoke(nameof(IvkProcessGhostHandPushAnimator), 2f);
    }

    void Lv1_Faucet()
    {
        Debug.Log("場景1 ==> 水龍頭 (Lv1_Faucet)");

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

        Invoke(nameof(IvkS1_SetGhostHandPosition), 3f);
    }

    public void Lv1_ShowTVWhiteNoise()
    {
        GameObject Lv1_TVObj = GameObject.Find("Lv1_TV");
        Lv1_TVObj.transform.GetChild(1).GetComponent<MeshRenderer>().material = Lv1_matTVWhiteNoise;
        Lv1_TVObj.transform.GetChild(2).GetComponent<Light>().enabled = true;
    }

    public void Lv1_DollTurnAround()
    {
        Lv1_Doll_Ani.SetBool("TurnHead", true);
    }

    public void Lv1_CandleFall()
    {
        ProcessAnimator("LV1_Lotus_Candles/Lv1_Falled_Candle", "Candle_Fall");
    }
    #endregion

    #region - 場景二 Event -
    void S2_LightSwitch()
    {
        Debug.Log("場景2 ==> 房間燈開關 (S2_Light_Switch)");

        audManager.Play(1, "light_Switch_Sound", false);
        DialogueObjects[(byte)Lv1_Dialogue.OpenLight_Lv2].CallAction();
    }

    void S2_RoomDoorLock()
    {
        Debug.Log("場景2 ==> 房間門鎖住了 (S2_Room_Door_Lock)");

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

    void S2_FlashLight()
    {
        Debug.Log("場景2 ==> 手電筒 (S2_FlashLight)");

        bLv2_HasFlashlight = true;

        audManager.Play(1, "light_Switch_Sound", false);
        DialogueObjects[(byte)Lv1_Dialogue.OpenFlashLight_Lv2].CallAction();

        GameObject S2_FlashLightObj = GameObject.Find("S2_FlashLight");
        Destroy(S2_FlashLightObj);

        if (bLv2_HasGrandmaRoomKey)
            ShowHint(HintItemID.S2_Grandma_Room_Door_Open);
    }

    void S2_SideTable()
    {
        Debug.Log("場景2 ==> 開門旁邊的小桌抽屜 (S2_Side_Table)");

        ProcessAnimator("Lv2_Side_Table", "S2_Side_Table_Open_01");
        GameObject RoomKeyObj = GameObject.Find("S2_Grandma_Room_Key");
        RoomKeyObj.GetComponent<Animation>().Play();
        audManager.Play(1, "drawer_Opening_Sound", false);
        Invoke(nameof(IvkShowS2DoorKey), 1.25f);
    }

    void S2_RoomKey()
    {
        Debug.Log("場景2 ==> 門旁邊小桌抽屜內的鑰匙 (S2_Room_Key)");

        bLv2_HasGrandmaRoomKey = true;
        audManager.Play(1, "tet_Sound_Of_Get_The_Key", false);

        BoxCollider S2_Door_Knock_Trigger = GameObject.Find("S2_Door_Knock_Trigger").GetComponent<BoxCollider>();
        S2_Door_Knock_Trigger.enabled = true;

        GameObject GrandMaRoomKeyObj = GameObject.Find("S2_Grandma_Room_Key");
        Destroy(GrandMaRoomKeyObj);

        if (bLv2_HasFlashlight)
            ShowHint(HintItemID.S2_Grandma_Room_Door_Open);
    }

    void S2_DoorKnockStop()
    {
        Debug.Log("場景2 ==> 門的撞擊聲暫停 (S2_Door_Knock_Stop)");

        audManager.Play(1, "emergency_Knock_On_The_Door", false);
        ShowHint(HintItemID.S2_Grandma_Room_Door_Open);
    }

    void S2_GrandmaDoorOpen()
    {
        Debug.Log("場景2 ==> 房間門打開 (S2_Grandma_Door_Open)");

        ProcessAnimator("Lv2_Grandma_Room_Door", "S2_Grandma_Room_Door_Open");
        audManager.Play(1, "the_sound_of_the_eyes_opening_the_door", false);
    }

    void S2_GrandmaDoorClose()
    {
        Debug.Log("場景2 ==> 阿嬤房門用力關閉 (S2_Grandma_Door_Close)");

        audManager.Play(1, "door_Close", false);
        ProcessAnimator("Lv2_Grandma_Room_Door", "S2_Grandma_Room_Door_Close");
    }

    void S2_GhostPassDoor()
    {
        Debug.Log("場景2 ==> 鬼阿嬤從門前衝過 (S2_Ghost_Pass_Door)");

        audManager.Play(1, "Girl_laughing", false);
        S2_Grandma_Ghost_Obj.GetComponent<Animator>().applyRootMotion = false;
        S2_Grandma_Ghost_Obj.GetComponent<Animator>().SetTrigger("S2_Grandma_Pass_Door");
        S2_Grandma_Cry_Audio_Obj.SetActive(true);
        Invoke(nameof(IvkS2_Grandma_Pass_Door), 1.5f);
    }

    void S2_ToiletDoor()
    {
        Debug.Log("場景2 ==> 看廁所鬼阿嬤動畫 (S2_Toilet_Door)");

        Transform tfToiletPos = GameObject.Find("Lv2_Player_Toilet_Pos").GetComponent<Transform>();
        Transform tfCameraPos = tfToiletPos.GetChild(0);
        StartCoroutine(PlayerToAniPos(tfToiletPos.position, tfToiletPos.rotation, tfCameraPos.rotation));

        Invoke(nameof(Lv2DelayChangeObjectPos), 2f);
    }

    void S2_Rice_Funeral()
    {
        Debug.Log("場景2 ==> 地上的腳尾飯 (S2_Rice_Funeral)");

        audManager.Play(1, "get_Item_Sound", false);
        DialogueObjects[(byte)Lv1_Dialogue.CheckRiceFuneral_OnFloor_Lv2].CallAction();

        Lv2_Rice_Funeral_Obj.GetComponent<BoxCollider>().enabled = false;
        Lv2_Rice_Funeral_Obj.transform.parent = playerCtrlr.transform;
        Lv2_Rice_Funeral_Obj.transform.localPosition = new Vector3(0, 0.05f, 0.6f);
        Lv2_Rice_Funeral_Obj.transform.localRotation = new Quaternion(0, 0, 0, 0);

        ShowHint(HintItemID.Lv2_Ruce_Funeral_Plate);
    }

    void S2_Photo_Frame()
    {
        Debug.Log("場景2 ==> 地上的相框接續到最後 (S2_Photo_Frame)");

        audManager.Play(1, "At_the_end_it_is_found_that_Acuan_has_mostly_disappeared_and_Acuan_has_climbed_up", false);
        ProcessRoMoving(4);
        UIState(UIItemID.S2_Photo_Frame, true);
        ShowObj(ObjItemID.S2_Photo_Frame_Floor);

        Lv2_BrotherShoe_Obj.transform.localPosition = new Vector3(-4.4f, 0f, 48.8f);
    }

    void Lv2_RuceFuneralPlate()
    {
        Debug.Log("場景2 ==> 放腳尾飯到凳子上 (Lv2_RuceFuneralPlate)");

        Lv2_Rice_Funeral_Obj.transform.parent = Lv2_Piano_Stool_Item.transform;
        Lv2_Rice_Funeral_Obj.transform.localPosition = new Vector3(0, 1f, 0);
        Lv2_Rice_Funeral_Obj.transform.localRotation = new Quaternion(0, 0, 0, 0);
        Lv2_Rice_Funeral_Obj.transform.localScale = new Vector3(1f, 2f, 1f);

        audManager.Play(1, "mirror_Breaking_Sound", false);
        DialogueObjects[(byte)Lv1_Dialogue.Lv2_PhotoFrameFall].CallAction();
        ProcessAnimator("Lv2_Photo_Frame", "Lv2_PhotoFrameFall");
        ShowHint(HintItemID.S1_Photo_Frame);
        S1_Photo_Frame_Obj.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
    }

    void Lv2_BoySneaker()
    {
        Debug.Log("場景2 ==> 哥哥的鞋子 (Lv2_BoySneaker)");

        Transform tfPlayingLotusPos = GameObject.Find("Lv2_Player_CheckSneaker_Pos").GetComponent<Transform>();
        Transform tfCameraPos = tfPlayingLotusPos.GetChild(0);
        StartCoroutine(PlayerToAniPos(tfPlayingLotusPos.position, tfPlayingLotusPos.rotation, tfCameraPos.rotation));

        Invoke(nameof(DelayBoySneakerDialog), 2f);
        Invoke(nameof(DelayCheckBoySneaker), 4f);
    }
    #endregion
}