using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SubTitleController : MonoBehaviour
{
    public SceneController SceneCtrlr;
    public GameEventController GameEventCtrlr;

    public string CurrentDialogue;

    public bool m_bIsPlayingCannotMove;

    public void CheckHasDealDelay()
    {
        if (GlobalDeclare.GetDialogueEvent() != 0)
        {
            GameEventCtrlr.DialogueObjects[GlobalDeclare.GetDialogueEvent()].CallAction(GlobalDeclare.GetPlayerMovable());
            GlobalDeclare.SetDialogueEvent((byte)Lv1_Dialogue.Empty);
        }
    }
}
