using UnityEngine;

public partial class GameEventController
{
    #region - Game Event -
    void Lv1_TalkToPackage()
    {
        Transform tfPlayer = GameObject.Find("_Player/LingLing").transform;
        Transform tfTalkToPackagePos = GameObject.Find("_Location/TalkToPackagePos").transform;

        tfPlayer.localPosition = tfTalkToPackagePos.localPosition;
        tfPlayer.localEulerAngles = new Vector3(0f, 275f, 0f);

        PlayerCtrlr.DefaultCursorState();
        PlayerCtrlr.m_bCanControl = true;

        ProcessPlayerAnimator(PlayerAnimateType.FacePackageStandUp.ToString());
        GlobalDeclare.SetDialogueEvent((byte)Lv1_Dialogue.HintMove);
        GlobalDeclare.SetPlayerMovable(true);
        DialogueObjects[(byte)Lv1_Dialogue.FacePackage].CallAction(false);

        SceneCtrlr.RecEventCallback(LevelTypeID.Lv1_GrandmaHouse, (int)Lv1_EventCallBackID.Lv1_TalkToPackage);
    }

    void Lv1_GrandmaRoomDoorOpen()
    {

    }
    #endregion
}
