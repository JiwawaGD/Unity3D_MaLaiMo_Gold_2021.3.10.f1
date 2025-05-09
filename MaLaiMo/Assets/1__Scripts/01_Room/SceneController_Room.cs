using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

using DG.Tweening;

public class SceneController_Room : SceneController
{
    #region < Fields >
    [Header("=== By Scene 各場景使用欄位 ===\r\n")] public GameObject _temp;

    [Header("室外傳至室內的角色座標")] public Transform _outsideGoInTransitPos;
    [Header("蓮花遊戲控制器")] public LotusGameManager _lotusGameManager;

    [Header("電視")] public GameObject _tvObject;
    [Header("電視雜訊的材質球")] public Material _tvNoiseMaterial;
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
        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Piano);

        // *TODO* 待確定使用的值
        if (GlobalDeclare._checkList02_holdRice)
        {
            ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_PlaceToPutRice);
        }

        if (!GlobalDeclare._firstStartGameLevel_1)
        {
            GlobalDeclare._firstStartGameLevel_1 = true;

            // 室內場景的第一個可互動物件
            GameEvent(LevelTypeID.Lv1_GrandmaHouse, GameEventID.Lv1_TalkToPackage);

            // *TODO* 以下為暫時設定的程式 > 待實際遊歷流程串接
            GlobalDeclare._holdingRiceFuneral = true;
            ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_LotusPaper);
        }
        else
        {
            // 設定玩家傳送座標
            SetPlayerLocation(this._outsideGoInTransitPos.localPosition);

            // 非第一次進場場景 > 轉場圖片 Fade Out
            TransitFadeOut();
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
            ItemController NextItem = null;

            switch (r_SceneTypeID)
            {
                case LevelTypeID.Lv1_GrandmaHouse:
                    string itemName = "";

                    switch (r_ItemID)
                    {
                        case HintItemID.Lv1_OpenRoomDoor:
                            itemName = "_Scene01_InteractItems/__Level_1/Lv1_Door/Lv1_Grandma_Room_Door";
                            break;
                        case HintItemID.Lv1_FirstTalkToMom:
                            itemName = "_Scene01_Map/Mom";
                            break;
                        case HintItemID.Lv1_ClipBoard:
                            itemName = "_Scene01_InteractItems/__Level_1/Lv1_Clipboard";
                            break;
                        case HintItemID.Lv1_Item_GoOutSide:
                            itemName = "_Scene01_InteractItems/__Level_1/Lv1_inside_BigDoor";
                            break;
                        case HintItemID.Lv1_Item_LotusPaper:
                            itemName = "_Scene01_InteractItems/__Level_1/Lv1_Lotus_Handler";
                            break;
                        case HintItemID.Lv1_Piano:
                            itemName = "_Scene01_InteractItems/__Level_1_TODO/Lv1_Piano";
                            break;
                        case HintItemID.Lv1_Item_FinishedLotus:
                            itemName = "_Scene01_InteractItems/__Level_1/Lv1_Finished_Lotus_Paper";
                            break;
                        case HintItemID.Lv1_Item_PlaceToPutRice:
                            itemName = "_Scene01_InteractItems/__Level_1/Item_PlaceToPutRice";
                            break;
                        default:
                            Debug.LogError(string.Format("[ERROR] [ShowHint] [Lv1_GrandmaHouse] Error Item ID :: {0}", r_ItemID));
                            break;
                    }

                    NextItem = GameObject.Find(itemName).GetComponent<ItemController>();
                    break;
                default:
                    Debug.LogError(string.Format("[ERROR] [ShowHint] [Lv1_GrandmaHouse] Error Scene ID :: {0}", r_SceneTypeID));
                    break;
            }

            NextItem.bActive = true;
            NextItem.SetHintable(true);
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
    #endregion

    #region < API >
    // Call From Dialogue System 
    public override void SetPlayerControl(bool r_bEnable)
    {
        base.SetPlayerControl(r_bEnable);
    }

    public void SetTVNoise()
    {
        MeshRenderer tvRender = this._tvObject.transform.Find("Screen").GetComponent<MeshRenderer>();
        tvRender.material = this._tvNoiseMaterial;
    }

    public void LotusGameFinish()
    {
        this._lotusGameManager.enabled = false;

        Vector3 playerLocation = new(-3.2f, 0.68f, -1.8f);
        Vector3 playerRotation = new(0f, 270f, 0f);
        Vector3 cameraRotation = new(-47f, 0f, 0f);

        PlayerCtrlr.SetToTargetLocation(playerLocation, playerRotation, cameraRotation);

        PlayerCtrlr._bCanControl = true;
        PlayerCtrlr._rig.useGravity = true;
        PlayerCtrlr._collider.enabled = true;

        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_FinishedLotus);
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
                case GameEventID.Lv1_FirstTalkToMom:
                    Lv1_FirstTalkToMom();
                    break;
                case GameEventID.Lv1_GoOutSide:
                    Lv1_GoOutSide();
                    break;
                case GameEventID.Lv1_LotusPaper:
                    Lv1_LotusPaperCheck();
                    break;
                case GameEventID.Lv1_Piano:
                    //Lv1_LotusPaperCheck();
                    break;
                case GameEventID.Lv1_Event_HoldFinishLotusPaper:
                    if (GlobalDeclare._checkList02_holdRice == false) Lv1_Event_HoldFinishLotusPaper();
                    else PlayDialogue((byte)Room_Dialogue.Lv1_018_ShouldPutDown);
                    break;
                case GameEventID.Lv1_Event_PutRiceOnKitchenTable:
                    Lv1_Event_PutRiceOnKitchenTable();
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
        Transform tfPlayer = GameObject.Find("_Common_Player/LingLing").transform;
        Transform tfTalkToPackagePos = GameObject.Find("_Scene01_MoveLocation/TalkToPackagePos").transform;

        tfPlayer.localPosition = tfTalkToPackagePos.localPosition;
        tfPlayer.localEulerAngles = new Vector3(0f, 275f, 0f);

        ProcessPlayerAnimator(PlayerAnimateType.FacePackageStandUp.ToString());
        GlobalDeclare.SetDialogueEvent((byte)Room_Dialogue.Lv1_001_HintMove);

        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_OpenRoomDoor);

        PlayDialogue((int)Room_Dialogue.Lv1_000_FacePackage);
    }

    void Lv1_GrandmaRoomDoorSwitch()
    {
        Transform tfRoomDoor = GameObject.Find("_Scene01_InteractItems/__Level_1/Lv1_Door/Lv1_Grandma_Room_Door").transform;
        Animation AniRoomDoor = tfRoomDoor.GetComponent<Animation>();

        if (tfRoomDoor.localRotation.z > 0.49 || tfRoomDoor.localRotation.z == 0)
        {
            string strPlayAniName = tfRoomDoor.localRotation.z == 0 ? "Door_Open" : "Door_Close";

            AniRoomDoor[strPlayAniName].time = 0f;
            AniRoomDoor.PlayQueued(strPlayAniName);
        }

        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_FirstTalkToMom);
    }

    void Lv1_FirstTalkToMom()
    {
        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_ClipBoard);
        //PlayDialogue((int)Room_Dialogue.Lv1_003_E_Mother);
    }

    void Lv1_GoOutSide()
    {
        _transitBlackImg.DOFade(1f, 1)
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
    }

    void Lv1_Event_PutRiceOnKitchenTable()
    {
        GameObject riceAndSoup = GameObject.Find("_Scene01_Map/Kitchen/3Cuisine_1Soup");

        riceAndSoup.GetComponent<MeshRenderer>().enabled = true;

        paperMissionFinsih[(int)PaperMission.TalkRiceToKitchen] = true;
    }
    #endregion
}
