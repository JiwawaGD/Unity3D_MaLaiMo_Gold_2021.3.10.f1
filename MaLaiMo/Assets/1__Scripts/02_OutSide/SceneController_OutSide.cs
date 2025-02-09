using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneController_OutSide : SceneController
{
    public override void Start()
    {
        PlayDialogue((byte)OutSide_Dialogue.Lv2_007_GoOut);
    }
}