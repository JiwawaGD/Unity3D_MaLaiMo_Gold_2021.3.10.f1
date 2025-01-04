using UnityEngine;

public partial class GameEventController
{
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

        DialogueObjects[(byte)Room_Dialogue.Lv1_000_FacePackage].StartAction();
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
