using UnityEngine;

public partial class GameEventController
{
    void Lv1_TalkToPackage()
    {
        Transform tfPlayer = GameObject.Find("_Player/LingLing").transform;

        Transform tfTalkToPackagePos = GameObject.Find("_Location/TalkToPackagePos").transform;

        tfPlayer.localPosition = tfTalkToPackagePos.localPosition;
        tfPlayer.localEulerAngles = new Vector3(0f, 275f, 0f);

        PlayerController PlayerCtrlr = tfPlayer.GetComponent<PlayerController>();
        PlayerCtrlr.SetCursor();
    }

    void Lv1_GrandmaRoomDoorOpen()
    {

    }
}