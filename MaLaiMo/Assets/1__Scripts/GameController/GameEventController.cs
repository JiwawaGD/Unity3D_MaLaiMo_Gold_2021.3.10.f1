using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    #region - 場景一 Event -
    void Lv1_PhotoFrameEvent()
    {
        Debug.Log("場景1 ==> 正常相框 (Lv1_Photo_Frame)");

        ProcessAnimator("Lv2_Photo_Frame", "Lv2_PutPhotoFrameBack");

        DialogueObjects[(byte)Lv1_Dialogue.Lv2_PutPhotoFrameBack].CallAction();

        Invoke(nameof(IvkLv2_BrokenPhotoFrameEnable), 2.5f);
    }

    void Lv1_PhotoFrameHasBroken()
    {
        Debug.Log("場景1 ==> 破碎相框 (Lv1_Photo_Frame_Has_Broken)");

        audManager.Play(1, "get_Item_Sound", false);
        UIState(UIItemID.Lv1_Photo_Frame, true);
        ProcessRoMoving(2);
        ShowObj(ObjItemID.Lv1_Photo_Frame);

        Lv2_BrotherShoe_Obj.transform.localPosition = new Vector3(-4.4f, 0f, 48.8f);

        //GlobalDeclare.byCurrentDialogIndex = (byte)Lv1_Dialogue.Lv2_Boy_Sneaker;
        //bNeedShowDialog = true;

        ShowHint(HintItemID.Lv2_Boy_Sneaker);
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
        TempItem.eventID = GameEventID.Lv1_Toilet_Door_Open;
        audManager.FlushSound.Play();
        DialogueObjects[(byte)Lv1_Dialogue.HeardBathRoomSound_Lv1].CallAction();
    }

    void Lv1_GrandmaDeadBody()
    {
        Debug.Log("場景1 ==> 跟孝濂內的阿嬤互動 (Lv1_Grandma_Dead_Body)");

        Lv1_Rice_Funeral_Obj.SetActive(false);
        DialogueObjects[(byte)Lv1_Dialogue.OpenFilialPietyCurtain_Lv1].CallAction();
        StopReadding();

        // 生成摔壞的腳尾飯
        UnityEngine.Object RiceFuneralSpilled = Resources.Load<GameObject>("Prefabs/Rice_Funeral_Spilled");
        GameObject RiceFuneralSpilledObj = Instantiate(RiceFuneralSpilled) as GameObject;
        RiceFuneralSpilledObj.transform.parent = GameObject.Find("===== ITEMS =====").transform;
        RiceFuneralSpilledObj.transform.position = new Vector3(-4.4f, 0.006f, 11.8f);
        RiceFuneralSpilledObj.name = "Rice_Funeral_Spilled";
        ShowHint(HintItemID.Lv1_Rice_Funeral_Spilled);
    }

    void Lv1_WhiteTent()
    {
        Debug.Log("場景1 ==> 觸發孝濂 (Lv1_White_Tent)");

        audManager.Play(1, "filial_Piety_Curtain", false);
        ProcessAnimator("Lv1_Filial_Piety_Curtain", "Filial_piety_curtain Open");
        TempBoxCollider = GameObject.Find("Lv1_Filial_Piety_Curtain").GetComponent<BoxCollider>();
        TempBoxCollider.enabled = false;
        ShowHint(HintItemID.Lv1_Lie_Grandma_Body);
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

    void Lv1_LightSwitch()
    {
        Debug.Log("場景1 ==> 觸發阿嬤房間燈開關 (Lv1_Light_Switch)");

        if (bLv1_HasFlashlight)
            DialogueObjects[(byte)Lv1_Dialogue.Lv1_OpenLight_HasFlashlight].CallAction();
        else
            DialogueObjects[(byte)Lv1_Dialogue.OpenLight_Lv1].CallAction();

        audManager.Play(1, "light_Switch_Sound", false);
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

    void Lv1_DeskDrawer()
    {
        Debug.Log("場景1 ==> 阿嬤房間抽屜 (Lv1_Desk_Drawer)");

        audManager.Play(1, "drawer_Opening_Sound", false);
        TempBoxCollider = Lv1_Desk_Drawer_Item.GetComponent<BoxCollider>();
        //TempBoxCollider = GameObject.Find("grandpa_desk/Desk_Drawer").GetComponent<BoxCollider>();
        TempBoxCollider.enabled = false;
        ProcessAnimator("grandpa_desk/Lv1_Desk_Drawer", "DrawerWithKey_Open");
        TempGameObject = GameObject.Find("Grandma_Room_Key");
        TempGameObject.GetComponent<Animation>().Play();
        Invoke(nameof(IvkShowDoorKey), 1.2f);
    }

    void Lv1_GrandmaRoomKey()
    {
        Debug.Log("場景1 ==> 阿嬤房間抽屜鑰匙 (Lv1_GrandmaRoomKey)");

        bLv1_HasGrandmaRoomKey = true;
        DialogueObjects[(byte)Lv1_Dialogue.GetKey_Lv1].CallAction();
        GameObject GrandmaRoomKeyObj = GameObject.Find("Grandma_Room_Key");
        Destroy(GrandmaRoomKeyObj);

        if (bLv1_HasFlashlight)
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

        //audManager.Play(1, "the_door_is_locked_and_cannot_be_opened_with_sound_effects", false);
    }

    void Lv1_RiceFuneralSpilled()
    {
        Debug.Log("場景1 ==> 腳尾飯打翻 (Lv1_Rice_Funeral_Spilled)");

        ShowHint(HintItemID.Lv1_Lotus_Paper);
        m_bPlayLotusEnable = true;
        DialogueObjects[(byte)Lv1_Dialogue.CheckRiceFuneral_OnFloor_Lv1].CallAction();

        // 移動蓮花紙 & 蠟燭座標
        Lv1_Lotus_Paper_Obj.transform.localPosition = new Vector3(-3.9f, 0.6f, -2.4f);
        Lv1_Lotus_Candle_Obj.transform.localPosition = new Vector3(-4f, 0.58f, -2.1f);
    }

    void Lv1_RiceFuneral()
    {
        Debug.Log("場景1 ==> 桌上的腳尾飯 (Lv1_Rice_Funeral)");

        bLv1_TriggerRiceFuneral = true;

        audManager.Play(1, "get_Item_Sound", false);
        ShowHint(HintItemID.Lv1_Filial_Piety_Curtain);
        UIState(UIItemID.Lv1_Rice_Funeral, true);
        ShowObj(ObjItemID.Lv1_Rice_Funeral);
        ProcessRoMoving(0);

        TempGameObject = GameObject.Find("Lv1_Grandma_Pass_Door_Trigger");
        TempGameObject.transform.localPosition = new Vector3(-5f, 0.5f, 8f);
    }

    void Lv1_GrandmaPassDoorAfterRiceFurnel()
    {
        Debug.Log("場景1 ==> 鬼阿嬤從門前衝過 (Lv1_GrandmaPassDoorAfterRiceFurnel)");

        Lv2_Grandma_Ghost_Obj.GetComponent<Animator>().applyRootMotion = false;
        Lv2_Grandma_Ghost_Obj.GetComponent<Animator>().SetTrigger("Lv1_Grandma_Pass_Door");
        Invoke(nameof(IvkLv1_SetGrandmaGhostPosition), 2.2f);
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

    void Lv1_ToiletGhostHide()
    {
        // 暫時沒用到
        return;

        Debug.Log("場景1 ==> 廁所鬼頭縮回門後 (Lv1_Toilet_Ghost_Hide)");

        ProcessAnimator("Lv1_Toilet_Door_Ghost", "Toilet_Door_Ghost_Out");
        m_bToiletGhostHasShow = false;
        ShowHint(HintItemID.Lv1_Toilet_GhostHand_Trigger);

        Invoke(nameof(IvkLv1_SetGhostHandPosition), 2f);

        GameObject GhostHandTriggerObj = GameObject.Find("Ghost_Hand_Trigger");
        GhostHandTriggerObj.transform.position = new Vector3(-8.5f, 0.1f, 6.1f);
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

    void Lv2_SideTable()
    {
        Debug.Log("場景2 ==> 開門旁邊的小桌抽屜 (Lv2_Side_Table)");

        ProcessAnimator("Lv2_Side_Table", "Lv2_Side_Table_Open_01");
        GameObject RoomKeyObj = GameObject.Find("Lv2_Grandma_Room_Key");
        RoomKeyObj.GetComponent<Animation>().Play();
        audManager.Play(1, "drawer_Opening_Sound", false);
        Invoke(nameof(IvkShowLv2DoorKey), 1.25f);
    }

    void Lv2_RoomKey()
    {
        Debug.Log("場景2 ==> 門旁邊小桌抽屜內的鑰匙 (Lv2_Room_Key)");

        bLv2_HasGrandmaRoomKey = true;
        audManager.Play(1, "tet_Sound_Of_Get_The_Key", false);

        BoxCollider Lv2_Door_Knock_Trigger = GameObject.Find("Lv2_Door_Knock_Trigger").GetComponent<BoxCollider>();
        Lv2_Door_Knock_Trigger.enabled = true;

        GameObject GrandMaRoomKeyObj = GameObject.Find("Lv2_Grandma_Room_Key");
        Destroy(GrandMaRoomKeyObj);

        if (bLv2_HasFlashlight)
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

    void Lv2_GhostPassDoor()
    {
        Debug.Log("場景2 ==> 鬼阿嬤從門前衝過 (Lv2_Ghost_Pass_Door)");

        audManager.Play(1, "Girl_laughing", false);
        Lv2_Grandma_Ghost_Obj.GetComponent<Animator>().applyRootMotion = false;
        Lv2_Grandma_Ghost_Obj.GetComponent<Animator>().SetTrigger("Lv2_Grandma_Pass_Door");
        Lv2_Grandma_Cry_Audio_Obj.SetActive(true);
        Invoke(nameof(IvkLv2_Grandma_Pass_Door), 1.5f);
    }

    void Lv2_ToiletDoor()
    {
        Debug.Log("場景2 ==> 看廁所鬼阿嬤動畫 (Lv2_Toilet_Door)");

        Transform tfToiletPos = GameObject.Find("Lv2_Player_Toilet_Pos").GetComponent<Transform>();
        Transform tfCameraPos = tfToiletPos.GetChild(0);
        StartCoroutine(PlayerToAniPos(tfToiletPos.position, tfToiletPos.rotation, tfCameraPos.rotation));

        Invoke(nameof(Lv2DelayChangeObjectPos), 2f);
    }

    void Lv2_Rice_Funeral()
    {
        Debug.Log("場景2 ==> 地上的腳尾飯 (Lv2_Rice_Funeral)");

        audManager.Play(1, "get_Item_Sound", false);
        DialogueObjects[(byte)Lv1_Dialogue.CheckRiceFuneral_OnFloor_Lv2].CallAction();

        Lv2_Rice_Funeral_Obj.GetComponent<BoxCollider>().enabled = false;
        Lv2_Rice_Funeral_Obj.transform.parent = playerCtrlr.transform;
        Lv2_Rice_Funeral_Obj.transform.localPosition = new Vector3(0, 0.05f, 0.6f);
        Lv2_Rice_Funeral_Obj.transform.localRotation = new Quaternion(0, 0, 0, 0);

        ShowHint(HintItemID.Lv2_Ruce_Funeral_Plate);
    }

    void Lv2_Photo_Frame()
    {
        Debug.Log("場景2 ==> 地上的相框接續到最後 (Lv2_Photo_Frame)");

        audManager.Play(1, "At_the_end_it_is_found_that_Acuan_has_mostly_disappeared_and_Acuan_has_climbed_up", false);
        ProcessRoMoving(4);
        UIState(UIItemID.Lv2_Photo_Frame, true);
        ShowObj(ObjItemID.Lv2_Photo_Frame_Floor);

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
        ShowHint(HintItemID.Lv1_Photo_Frame);
        Lv1_Photo_Frame_Obj.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
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