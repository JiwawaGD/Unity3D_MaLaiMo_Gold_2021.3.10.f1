using UnityEngine;

public partial class GameEventController
{
    void Lv1_TalkToPackage()
    {
        Transform tfPlayer = GameObject.Find("_Player/LingLing").transform;
        Transform tfTalkToPackagePos = GameObject.Find("_Location/TalkToPackagePos").transform;

        tfPlayer.localPosition = tfTalkToPackagePos.localPosition;
        tfPlayer.localEulerAngles = new Vector3(0f, 275f, 0f);

        PlayerCtrlr.DefaultCursorState();
        PlayerCtrlr.m_bCanControl = true;

        ProcessPlayerAnimator(PlayerAnimateType.FacePackageStandUp.ToString());
        GlobalDeclare.SetDialogueEvent((byte)Lv1_Dialogue.KnockDoor);
        GlobalDeclare.SetPlayerMovable(true);
        DialogueObjects[(byte)Lv1_Dialogue.FacePackage].CallAction(false);
    }

    void Lv1_GrandmaRoomDoorOpen()
    {

    }
}