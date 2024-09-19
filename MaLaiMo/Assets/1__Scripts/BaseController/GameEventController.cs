using UnityEngine;

public partial class GameEventController : MonoBehaviour
{
    #region - Basic Function -
    public void RecGameEvent(LevelTypeID r_SceneTypeID, GameEventID r_EventID)
    {
        switch (r_SceneTypeID)
        {
            case LevelTypeID.BeginScene:
                break;
            case LevelTypeID.Introduce:
                break;
            case LevelTypeID.Lv1_GrandmaHouse:
                Lv1_Event(r_EventID);
                break;
            case LevelTypeID.Lv2_GrandmaHouse:
                Lv2_Event(r_EventID);
                break;
        }
    }

    void Lv1_Event(GameEventID r_EventID)
    {
        switch (r_EventID)
        {
            case GameEventID.Lv1_TalkToPackage:
                Lv1_TalkToPackage();
                break;
            case GameEventID.Lv1_GrandmaRoomDoorOpen:
                Lv1_GrandmaRoomDoorOpen();
                break;
            default:
                Debug.LogError(string.Format("[Lv1_Event] Error GameEventID : {0}", r_EventID));
                break;
        }
    }

    void Lv2_Event(GameEventID r_EventID)
    {
        switch (r_EventID)
        {
            default:
                Debug.LogError(string.Format("[Lv2_Event] Error GameEventID : {0}", r_EventID));
                break;
        }
    }
    #endregion
}