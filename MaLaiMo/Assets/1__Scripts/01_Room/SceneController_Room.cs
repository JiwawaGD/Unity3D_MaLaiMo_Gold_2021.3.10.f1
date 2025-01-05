using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController_Room : SceneController
{
    #region - Internal Override -
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        GameEvent(LevelTypeID.Lv1_GrandmaHouse, GameEventID.Lv1_TalkToPackage);
    }
    #endregion

    #region - External Override -
    public override void GameEvent(LevelTypeID r_SceneTypeID, GameEventID r_EventID)
    {
        switch (r_SceneTypeID)
        {
            case LevelTypeID.Lv1_GrandmaHouse:
                Lv1_Event(r_EventID);
                break;
            case LevelTypeID.BeginScene:
            case LevelTypeID.Introduce:
            case LevelTypeID.Lv2_GrandmaHouse:
                Debug.LogError(string.Format("[SceneCtrlr - Room] Error SceneTypeID"));
                break;
        }
    }

    public override void KeyboardTrigger() { }

    public override void ShowHint(HintItemID r_ItemID)
    {
        base.ShowHint(r_ItemID);
    }
    #endregion

    #region - Basic Function -
    void Lv1_Event(GameEventID r_EventID)
    {
        Debug.LogWarning(string.Format("[Lv1_Event] GameEventID : {0}", r_EventID));

        switch (r_EventID)
        {
            case GameEventID.Lv1_TalkToPackage:
                Lv1_TalkToPackage();
                break;
            case GameEventID.Lv1_GrandmaRoomDoorSwitch:
                Lv1_GrandmaRoomDoorSwitch();
                break;
            default:
                Debug.LogError(string.Format("[Lv1_Event] Error GameEventID : {0}", r_EventID));
                break;
        }
    }

    // 執行玩家移動到指定區域
    public IEnumerator ProcessPlayerSetPianoAni(int index)
    {
        //bIsPlayingPiano = true;
        //Transform tfPianoPos = GameObject.Find("PianoTarget").GetComponent<Transform>();
        //Transform tfCameraPos = tfPianoPos.GetChild(0);

        //yield return StartCoroutine(PlayerToAniPos(Targers[index].position, tfPianoPos.rotation, tfCameraPos.rotation));
        yield return null;

        //if (bIsPlayingPiano == true)
        //    PianoUI.SetActive(true);
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

        ShowHint(HintItemID.Lv1_OpenRoomDoor);

        PlayDialogue((int)Room_Dialogue.Lv1_000_FacePackage);
    }

    void Lv1_GrandmaRoomDoorSwitch()
    {
        Transform tfRoomDoor = GameObject.Find("__ITEMS/__Level_1/Lv1_Door/Lv1_Grandma_Room_Door").transform;
        Animation AniRoomDoor = tfRoomDoor.GetComponent<Animation>();

        if (tfRoomDoor.localRotation.z > 0.49 || tfRoomDoor.localRotation.z == 0)
        {
            string strPlayAniName = tfRoomDoor.localRotation.z == 0 ? "Door_Open" : "Door_Close";

            AniRoomDoor[strPlayAniName].time = 0f;
            AniRoomDoor.PlayQueued(strPlayAniName);
        }
    }
    #endregion
}