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

    void Lv1_GrandmaRoomDoorSwitch()
    {
        Transform tfRoomDoor = GameObject.Find("__ITEMS/__Level_1/Lv1_Door/Lv1_Grandma_Room_Door").transform;
        Animation AniRoomDoor = tfRoomDoor.GetComponent<Animation>();

        if (tfRoomDoor.localRotation.z == 90 || tfRoomDoor.localRotation.z == 0)
        {
            string strPlayAniName = tfRoomDoor.localRotation.z == 0 ? "Door_Open" : "Door_Close";

            AniRoomDoor[strPlayAniName].time = 0f;
            AniRoomDoor.PlayQueued(strPlayAniName);
        }
    }
    #endregion
}
