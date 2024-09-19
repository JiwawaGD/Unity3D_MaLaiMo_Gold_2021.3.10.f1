using UnityEngine;

public class Scene_01_Controller : MonoBehaviour
{
    [SerializeField] LevelTypeID CurrentLevel;

    [SerializeField] GameEventController GameEventCtrlr;

    void Start()
    {
        GameEventCtrlr.RecGameEvent(LevelTypeID.Lv1_GrandmaHouse, GameEventID.Lv1_TalkToPackage);
    }
}
