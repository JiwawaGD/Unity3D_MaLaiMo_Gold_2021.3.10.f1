using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class SceneController_Room : SceneController
{
    #region < Property >
    [Space(10)]
    [Header("============ By Scene 各場景使用欄位 ============")]
    [Header("物件池")] public ObjectController_Room _objectCtrlr;

    [Header("玩家控制器")] public GrandmaRoom_Player Player;
    [Header("蓮花遊戲控制器")] public LotusGameManager _lotusGameManager;
    [Header("媽媽控制器")] public Mom_Controler_Room Mom_Control;
    [Header("室外傳至室內的角色座標")] public Transform _outsideGoInTransitPos;

    [Header("電視雜訊的材質球")] public Material _tvNoiseMaterial;
    [Header("阿嬤")] public GameObject GrandMa;
    [Header("護身符")] public GameObject Amulet;
    [Header("觸發天黑對話物件")] public GameObject triggerNightObject;
    [Header("製作和感謝人員名單")] public GameObject _thanksView;
    #endregion

    #region < ByScene Flag >
    public static bool _hasDoorDialogue = false;
    private bool _hasTriggerGraffiti = false;
    private static bool _isFinishLotusGame = false;
    private static bool FilialPietyCurtain_IsOpen = false;
    public static bool grandmaRoom_DoorOpen = false;
    public static bool grandmaRoom_ClosetOpen = false;
    #endregion

    #region < Unity Hook >
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();

        // 預設讓大門是可以互動狀態
        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_GoOutSide);
        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_Piano);
        //ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_GrandmaRoomCloset);

        if (GlobalDeclare._checkList01_holdLotus)
        {
            TakingObjects[0].SetActive(true);
        }
        else if (GlobalDeclare._checkList01_holdLotus == false)
        {
            if (_isFinishLotusGame == false) ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_LotusPaper);
            else ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_FoldedLotusPaper);
        }
        if (GlobalDeclare._checkList02_holdRice)
        {
            ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_OfferingPlace);
            TakingObjects[1].SetActive(true);
        }

        if (!GlobalDeclare._firstStartGameLevel_1)
        {
            GlobalDeclare._firstStartGameLevel_1 = true;

            // 室內場景的第一個可互動物件
            GameEvent(LevelTypeID.Lv1_GrandmaHouse, GameEventID.Lv1_TalkToPackage);

            // *TODO* 以下為暫時設定的程式 > 待實際遊歷流程串接
            GlobalDeclare._holdingRiceFuneral = true;
            //ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_LotusPaper);
        }
        else
        {
            // 設定玩家傳送座標
            SetPlayerLocation(this._outsideGoInTransitPos.localPosition);

            // 非第一次進場場景 > 轉場圖片 Fade Out
            TransitFadeOut();
        }

        if (FilialPietyCurtain_IsOpen)
        {
            this._objectCtrlr._filialPietyCurtain.GetComponent<Animator>().SetTrigger("Filial_piety_curtain Open");
        }

        if (grandmaRoom_DoorOpen) {
            _objectCtrlr._grandmaRoomDoor.transform.eulerAngles = new Vector3(-90, 90 ,0);
        }

        if (SceneController_OutSide.paperMissionFinsih[(int)PaperMission.PutLotusOnTable] &&
            SceneController_OutSide.paperMissionFinsih[(int)PaperMission.TalkRiceToKitchen] &&
            SceneController_OutSide.paperMissionFinsih[(int)PaperMission.LayOutSideCircle] &&
            grandmaRoom_ClosetOpen == false) 
        {
            ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_GrandmaRoomCloset);
        }
    }

    public override void Update()
    {
        base.Update();
    }
    #endregion

    #region < Override Function >
    public override void GameEvent(LevelTypeID r_SceneTypeID, GameEventID r_EventID)
    {
        base.GameEvent(r_SceneTypeID, r_EventID);

        switch (r_SceneTypeID)
        {
            case LevelTypeID.Lv1_GrandmaHouse:
                Lv1_Event(r_EventID);
                break;
            default:
                Debug.LogError(string.Format("[SceneCtrlr - Room] Error SceneTypeID : {0}", r_SceneTypeID));
                break;
        }
    }

    public override void KeyboardCheck()
    {
        // 正在摺蓮花中 Scene Controller 暫停 Update
        if (GlobalDeclare._playingLotusGame)
            return;

        base.KeyboardCheck();

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (GlobalDeclare._waitingPlayLotusPaper)
            {
                GlobalDeclare._playingLotusGame = true;
                UIState((int)UIItemID.Empty, false);
                this._lotusGameManager.SetHintPosition();
            }
        }
    }

    public override void ShowHint(LevelTypeID r_SceneTypeID, HintItemID r_ItemID)
    {
        base.ShowHint(r_SceneTypeID, r_ItemID);

        try
        {
            ItemController itemCtrlr = null;

            switch (r_SceneTypeID)
            {
                case LevelTypeID.Lv1_GrandmaHouse:
                    switch (r_ItemID)
                    {
                        case HintItemID.Lv1_Item_OpenRoomDoor:
                            itemCtrlr = this._objectCtrlr._grandmaRoomDoor.GetComponent<ItemController>();
                            break;
                        case HintItemID.Lv1_Item_TalkToSeatMom:
                            itemCtrlr = this._objectCtrlr._mom.GetComponent<ItemController>();
                            break;
                        case HintItemID.Lv1_Item_GoOutSide:
                            itemCtrlr = this._objectCtrlr._frontDoor.GetComponent<ItemController>();
                            break;
                        case HintItemID.Lv1_Item_LotusPaper:
                            itemCtrlr = this._objectCtrlr._lotusPaper.GetComponent<ItemController>();
                            break;
                        case HintItemID.Lv1_Item_Piano:
                            itemCtrlr = this._objectCtrlr._piano.GetComponent<ItemController>();
                            break;
                        case HintItemID.Lv1_Item_FoldedLotusPaper:
                            itemCtrlr = this._objectCtrlr._foldedLotusPaper.GetComponent<ItemController>();
                            break;
                        case HintItemID.Lv1_Item_OfferingPlace:
                            itemCtrlr = this._objectCtrlr._offeringPlace.GetComponent<ItemController>();
                            break;
                        case HintItemID.Lv1_Item_GrandmaRoomCloset:
                            itemCtrlr = this._objectCtrlr._grandmaRoomCloset.GetComponent<ItemController>();
                            break;
                        case HintItemID.Lv1_Item_ClothesInCloset:
                            itemCtrlr = this._objectCtrlr._clothesInCloset.GetComponent<ItemController>();
                            break;
                        case HintItemID.Lv1_Item_Grafitti:
                            itemCtrlr = this._objectCtrlr._graffitiInCloset.GetComponent<ItemController>();
                            break;
                        case HintItemID.Lv1_Item_Grandma_Dead_Body:
                            itemCtrlr = this._objectCtrlr._grandmaDeadBody.GetComponent<ItemController>();
                            break;
                        case HintItemID.Lv1_FilialPietyCurtain:
                            itemCtrlr = GameObject.Find("===== MAP/Scene01_InteractObject/Lv1_Filial_Piety_Curtain").GetComponent<ItemController>();
                            break;
                        default:
                            Debug.LogError(string.Format("[ERROR] [ShowHint] [Lv1_GrandmaHouse] Error Item ID :: {0}", r_ItemID));
                            break;
                    }
                    break;
                default:
                    Debug.LogError(string.Format("[ERROR] [ShowHint] [Lv1_GrandmaHouse] Error Scene ID :: {0}", r_SceneTypeID));
                    break;
            }

            itemCtrlr.bActive = true;
            itemCtrlr.SetHintable(true);
        }
        catch (System.Exception exception)
        {
            Debug.LogError(string.Format("[ERROR] Show Hint <{0}> Error :: {1}", r_ItemID, exception.Message));
            throw;
        }
    }

    public override void TransitFadeOut()
    {
        base.TransitFadeOut();
    }

    public override void SetPlayerLocation(Vector3 location)
    {
        base.SetPlayerLocation(location);
    }

    public override void UIState(int r_ItemID, bool r_bEnable, bool r_bNeedSubTitle = false)
    {
        base.UIState(r_ItemID, r_bEnable, r_bNeedSubTitle);

        _itemCanvasHandler._txtTopTitle.text = r_bEnable ? GlobalDeclare.item_Title_Room[r_ItemID] : "";
        _itemCanvasHandler._txtMainInfo.text = r_bEnable ? GlobalDeclare.item_MainInfo_Room[r_ItemID] : "";
        _itemCanvasHandler._txtBottonInfo.text = r_bEnable ? (r_bNeedSubTitle ? GlobalDeclare.item_BottonInfo_Room[r_ItemID] : "") : "";
    }

    public override void MoveItem(UIItemID r_ItemID)
    {
        // *TODO* 因為攝影棚在 -50 的位置，因此 Y 需要設定 -50 (待優化)
        switch (r_ItemID)
        {
            case UIItemID.Lv1_UI_LotusPaper:
                this._itemObjsForRawImage[this._currentItemIndex].transform.DOMove(
                    new Vector3(0f, -50f, 0f), 0.5f);
                break;
        }
    }

    public override void ChangeItemGameEventID(ItemController item, GameEventID newGameEventID)
    {
        base.ChangeItemGameEventID(item, newGameEventID);
    }

    public override void SetItemAlwaysActive(ItemController item, bool alwaysActive)
    {
        base.SetItemAlwaysActive(item, alwaysActive);
    }
    #endregion

    #region < API >
    // Call From Dialogue System 
    public override void SetPlayerControl(bool r_bEnable)
    {
        base.SetPlayerControl(r_bEnable);
    }

    public void SetTVNoise()
    {
        MeshRenderer tvRender = this._objectCtrlr._tv.transform.Find("Screen").GetComponent<MeshRenderer>();
        tvRender.material = this._tvNoiseMaterial;
    }

    public void LotusGameFinish()
    {
        this._lotusGameManager.enabled = false;
        _isFinishLotusGame = true;
        Vector3 playerLocation = new(-3.2f, 0.68f, -1.8f);
        Vector3 playerRotation = new(0f, 270f, 0f);
        Vector3 cameraRotation = new(-47f, 0f, 0f);

        PlayerCtrlr.SetToTargetLocation(playerLocation, playerRotation, cameraRotation);

        PlayerCtrlr._bCanControl = true;
        PlayerCtrlr._rig.useGravity = true;
        PlayerCtrlr._collider.enabled = true;

        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_FoldedLotusPaper);
    }
    #endregion

    #region < Basic Function >
    void Lv1_Event(GameEventID r_EventID)
    {
        try
        {
            switch (r_EventID)
            {
                case GameEventID.Lv1_TalkToPackage:
                    Lv1_TalkToPackage();
                    break;
                case GameEventID.Lv1_GrandmaRoomDoorSwitch:
                    Lv1_GrandmaRoomDoorSwitch();
                    break;
                case GameEventID.Lv1_GoOutSide:
                    Lv1_GoOutSide();
                    break;
                case GameEventID.Lv1_LotusPaper:
                    Lv1_LotusPaperCheck();
                    break;
                case GameEventID.Lv1_Piano:
                    break;
                case GameEventID.Lv1_Event_HoldFinishLotusPaper:
                    if (GlobalDeclare._checkList02_holdRice == false) Lv1_Event_HoldFinishLotusPaper();
                    else PlayDialogue((byte)Room_Dialogue.Lv1_018_ShouldPutDown);
                    break;
                case GameEventID.Lv1_Event_PutRiceOnKitchenTable:
                    Lv1_Event_PutRiceOnKitchenTable();
                    break;
                case GameEventID.Lv1_Event_WardrobeInRoom:
                    Lv1_Event_WardrobeInRoom();
                    break;
                case GameEventID.Lv1_Event_Graffiti:
                    Lv1_Event_Graffiti();
                    break;
                case GameEventID.Lv1_Event_RoomDoorAfterGraffiti:
                    Lv1_Event_RoomDoorAfterGraffiti();
                    break;
                case GameEventID.Lv1_Event_5ClothesOnGraffiti:
                    Lv1_Event_5ClothesOnGraffiti();
                    break;
                case GameEventID.Lv1_E_FilialPietyCurtain:
                    Lv1_E_FilialPietyCurtain();
                    break;
                case GameEventID.Lv1_E_GrandmaDeadBody:
                    Lv1_E_GrandmaDeadBody();
                    break;
                case GameEventID.Lv1_E_SeatMom:
                    StartCoroutine(Lv1_E_SeatMom());
                    break;
                case GameEventID.Lv5_E_CalendarBook:
                    Lv5_E_CalendarBook();
                    break;
                case GameEventID.Lv5_E_Grandma:
                    Lv5_E_Grandma();
                    break;
                case GameEventID.Lv5_E_Amulet:
                    Lv5_E_Amulet();
                    break;
                default:
                    Debug.LogError(string.Format("<color=red><b>[Error]</b></color> [Lv1_Event] Error Event ID :: {0}", r_EventID));
                    break;
            }
        }
        catch (System.Exception exception)
        {
            Debug.LogError(string.Format("[<color=red><b>Error</b></color>] [Lv1_Event] Event  <color=red><b>{0}</b></color>  Error  ::  {1}", r_EventID, exception.Message));
            throw;
        }
    }
    #endregion

    #region < Game Event >
    void Lv1_TalkToPackage()
    {
        Transform tfPlayer = this._objectCtrlr._player.transform;
        Transform tfTalkToPackagePos = this._objectCtrlr._playerWakeUpPos.transform;

        tfPlayer.localPosition = tfTalkToPackagePos.localPosition;
        tfPlayer.localEulerAngles = new Vector3(0f, 275f, 0f);

        ProcessPlayerAnimator(PlayerAnimateType.FacePackageStandUp.ToString());
        //GlobalDeclare.SetDialogueEvent((byte)Room_Dialogue.Lv1_001_HintMove);


        PlayDialogue((int)Room_Dialogue.Lv1_000_FacePackage);
    }

    public void Lv1_PlayHintMove()
    {
        PlayDialogue((int)Room_Dialogue.Lv1_001_HintMove);
    }

    public void Lv1_ShowDoorHint()
    {
        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_OpenRoomDoor);
    }

    void Lv1_GrandmaRoomDoorSwitch()
    {
        Transform tfRoomDoor = this._objectCtrlr._grandmaRoomDoor.transform;
        Animation AniRoomDoor = tfRoomDoor.GetComponent<Animation>();

        if (tfRoomDoor.localRotation.z > 0.49 || tfRoomDoor.localRotation.z == 0)
        {
            string strPlayAniName = tfRoomDoor.localRotation.z == 0 ? "Door_Open" : "Door_Close";
            if (strPlayAniName == "Door_Open") grandmaRoom_DoorOpen = true;
            AniRoomDoor[strPlayAniName].time = 0f;
            AniRoomDoor.PlayQueued(strPlayAniName);
        }
        if (_hasDoorDialogue)
        {
            PlayDialogue((int)Room_Dialogue.Lv5_001_E_Door);
            _hasDoorDialogue = false;
        }
    }

    void Lv1_GoOutSide()
    {
        this._transitBlackImg.DOFade(1f, 1)
                             .OnComplete(() => SceneManager.LoadScene(GlobalDeclare.Lv2_Grandma_OutSide));
    }

    void Lv1_LotusPaperCheck()
    {
        Vector3 v3TargetPos = new(-3.3f, 0.35f, -1.9f);
        Vector3 v3PlayerEndRotation = new(0f, 0f, 0f);
        Vector3 v3PlayerCamEndRotation = new(25f, 275f, 0f);

        PlayerCtrlr.MoveToTargetPosition(v3TargetPos, v3PlayerEndRotation, v3PlayerCamEndRotation, 2f, () =>
        {
            GlobalDeclare._waitingPlayLotusPaper = true;
            UIState((int)UIItemID.Lv1_UI_LotusPaper, true, true);
            this._lotusGameManager.enabled = true;
            this._lotusGameManager.SetPaperLocation();
        });
    }

    void Lv1_Event_HoldFinishLotusPaper()
    {
        GlobalDeclare._checkList01_holdLotus = true;
        TakingObjects[0].SetActive(true);
        GameObject foldedLotusPaper = this._objectCtrlr._foldedLotusPaper.gameObject;
        foldedLotusPaper.transform.localPosition = new Vector3(-3.9f, -4f, -2.2f);
    }

    void Lv1_Event_PutRiceOnKitchenTable()
    {
        GameObject riceAndSoup = this._objectCtrlr._riceAndSoup;

        riceAndSoup.GetComponent<MeshRenderer>().enabled = true;

        SceneController_OutSide.paperMissionFinsih[(int)PaperMission.TalkRiceToKitchen] = true;
        TakingObjects[1].SetActive(false);
        GlobalDeclare._checkList02_holdRice = false;
    }

    void Lv1_Event_WardrobeInRoom()
    {
        {   // 開衣櫃
            Animator wardrobeAnim = this._objectCtrlr._grandmaRoomCloset.transform.GetComponent<Animator>();
            wardrobeAnim.SetTrigger("Open");
            grandmaRoom_ClosetOpen = true;
            Debug.Log("<缺> 木頭櫃打開的聲音");
        }

        {   // 關房門
            Animation AniRoomDoor = this._objectCtrlr._grandmaRoomDoor.transform.GetComponent<Animation>();
            AniRoomDoor.PlayQueued("Door_Close");
        }

        {   // 切換房門的 EventID 且重新開啟 Hint
            ItemController itemGrandmaRoomDoor =this._objectCtrlr._grandmaRoomDoor;

            SetItemAlwaysActive(itemGrandmaRoomDoor, true);
            ChangeItemGameEventID(itemGrandmaRoomDoor, GameEventID.Lv1_Event_RoomDoorAfterGraffiti);

            ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_OpenRoomDoor);
        }

        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_ClothesInCloset);
    }

    void Lv1_Event_5ClothesOnGraffiti()
    {
        Animator clothesAnim = this._objectCtrlr._clothesInCloset.transform.GetComponent<Animator>();
        clothesAnim.SetTrigger("Move");

        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_Grafitti);
    }

    void Lv1_Event_Graffiti()
    {
        Debug.Log("<缺> 拿紙的聲音");

        triggerNightObject.SetActive(true);

        //ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_Crayon);
    }

    void Lv1_Event_RoomDoorAfterGraffiti()
    {
        if (this._hasTriggerGraffiti)
        {
            Debug.Log("01 : 琳琳：怎麼都找不到");

            {   // 開房門
                Animation AniRoomDoor = this._objectCtrlr._grandmaRoomDoor.transform.GetComponent<Animation>();
                AniRoomDoor.PlayQueued("Door_Open");

                WallClock wallClock =  this._objectCtrlr._wallClock.GetComponent<WallClock>();
                wallClock.UpdateClock(23,44);
            }
        }
        else
        {
            Debug.Log("01 : 琳琳：再試著找一下吧。");
        }
    }

    void Lv1_E_FilialPietyCurtain()
    {
        this._objectCtrlr._filialPietyCurtain.GetComponent<Animator>().SetTrigger("Filial_piety_curtain Open");
        PlayDialogue((int)Room_Dialogue.Lv1_001_E_FilialPietyCurtain);
        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_TalkToSeatMom);
    }

    IEnumerator Lv1_E_SeatMom()
    {
        Player._bCanControl = false;
        Mom_Control.LookAtPlayer();
        PlayDialogue((int)Room_Dialogue.Lv1_002_E_Mom);
        yield return new WaitForSeconds(9f);
        Mom_Control.GoOut();
        yield return new WaitForSeconds(5f);
        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_Grandma_Dead_Body);
    }

    void Lv1_E_GrandmaDeadBody()
    {
        PlayDialogue((int)Room_Dialogue.Lv1_003_E_Grandmother);
    }

    void Lv5_E_CalendarBook()
    {
        PlayDialogue((int)Room_Dialogue.Lv5_000_E_CalendarBook);
        //顯示出頭七當天的正確日期
        _hasDoorDialogue = true;
    }
    void Lv5_E_Grandma()
    {
        //播放擁抱動畫
        PlayDialogue((int)Room_Dialogue.Lv5_004_E_Hug);
    }
    void GrandmaDissapear()
    {
        GrandMa.SetActive(false);
        Amulet.SetActive(true);
    }
    void Lv5_E_Amulet()
    {
        //低頭看像護身符
        //將護身符拿在手上(手攤開)
        PlayDialogue((int)Room_Dialogue.Lv5_005_E_Amulet);
    }
    public void EndGame()
    {
        //播放謝幕文字
        _thanksView.active = true;
    }
    #endregion
}
