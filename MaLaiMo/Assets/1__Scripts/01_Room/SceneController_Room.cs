using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using DG.Tweening;

public class SceneController_Room : SceneController
{
    #region - Internal -
    [Header("=== By Scene 各場景使用欄位 ===\r\n")]
    public Image _toOutSideBlackImg;
    #endregion

    #region - Override -
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        // 預設讓大門是可以互動狀態
        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_GoOutSide);

        // 室內場景的第一個可互動物件
        GameEvent(LevelTypeID.Lv1_GrandmaHouse, GameEventID.Lv1_TalkToPackage);

        // *TODO* 以下為暫時設定的程式 > 待實際遊歷流程串接
        SetItemCanvasEnable(false);
        GlobalDeclare._bHoldingRiceFuneral = true;
        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_LotusPaper);
    }
    #endregion

    #region - External Override -
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

    public override void KeyboardTrigger() { }

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
                            itemName = "_Scene01_InteractItems/__Level_1/Lv1_Lotus_Paper";
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
    #endregion

    #region - Basic Function -
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

    public void ButtonFunction(ButtonEventID _eventID)
    {
        switch (_eventID)
        {
            case ButtonEventID.UI_Back:
                break;
            case ButtonEventID.Enter_Game:
                //if (bIsUIOpen)
                //{
                //    RestoreItemLocation();
                //    bIsPlayingLotus = true;

                //    Transform tfPlayingLotusPos = GameObject.Find("Lv1_Playing_Lotus_Pos").GetComponent<Transform>();
                //    Transform tfCameraPos = tfPlayingLotusPos.GetChild(0);
                //    StartCoroutine(PlayerToAniPos(tfPlayingLotusPos.position, tfPlayingLotusPos.rotation, tfCameraPos.rotation));
                //}
                break;
        }
    }
    #endregion

    #region - Game Event -
    void Lv1_TalkToPackage()
    {
        Transform tfPlayer = GameObject.Find("_Common_Player/LingLing").transform;
        Transform tfTalkToPackagePos = GameObject.Find("_Scene01_MoveLocation/TalkToPackagePos").transform;

        tfPlayer.localPosition = tfTalkToPackagePos.localPosition;
        tfPlayer.localEulerAngles = new Vector3(0f, 275f, 0f);

        PlayerCtrlr.DefaultCursorState();
        PlayerCtrlr.m_bCanControl = true;

        ProcessPlayerAnimator(PlayerAnimateType.FacePackageStandUp.ToString());
        GlobalDeclare.SetDialogueEvent((byte)Room_Dialogue.Lv1_001_HintMove);
        GlobalDeclare.SetPlayerMovable(true);

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
        PlayDialogue((int)Room_Dialogue.Lv1_003_E_Mother);
    }

    void Lv1_GoOutSide()
    {
        PlayerCtrlr.m_bCanControl = true;

        _toOutSideBlackImg.DOFade(1f, 1)
                          .OnComplete(() => SceneManager.LoadScene("4 Outdoor_Scene"));
    }

    void Lv1_LotusPaperCheck()
    {
        Vector3 v3TargetPos = new Vector3(-3.1f, 0.35f, -1.9f);
        Vector3 v3PlayerEndRotation = new Vector3(0f, 0f, 0f);
        Vector3 v3PlayerCamEndRotation = new Vector3(0f, 275f, 0f);

        PlayerCtrlr.MoveToTargetPosition(v3TargetPos, v3PlayerEndRotation, v3PlayerCamEndRotation, 2f, () =>
        {
            UIState(UIItemID.Lv1_UI_LotusPaper, true, true);
        });
    }
    #endregion
}
