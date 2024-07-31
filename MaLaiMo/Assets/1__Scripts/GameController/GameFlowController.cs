using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    void Lv1_Event(GameEventID r_EventID)
    {
        switch (r_EventID)
        {
            case GameEventID.Close_UI:
                CloseUI();
                break;
            case GameEventID.Lv1_Photo_Frame:
                Lv1_PhotoFrameEvent();
                break;
            case GameEventID.Lv1_Grandma_Door_Open:
                Lv1_GrandmaDoorOpen();
                break;
            case GameEventID.Lv1_Grandma_Room_Door_Lock:
                Lv1_GrandmaRoomDoorLock();
                break;
            case GameEventID.Lv1_Lotus_Paper:
                Lv1_LotusPaper();
                break;
            case GameEventID.Lv1_Photo_Frame_Light_On:
                break;
            case GameEventID.Lv1_Grandma_Rush:
                break;
            case GameEventID.Lv1_Toilet_Door_Lock:
                Lv1_ToiletDoorLock();
                break;
            case GameEventID.Lv1_Toilet_Door_Open:
                Lv1_ToiletDoorOpen();
                break;
            case GameEventID.Lv1_Toilet_Ghost_Hide:
                break;
            case GameEventID.Lv1_Toilet_Ghost_Hand_Push:
                Lv1_ToiletGhostHandPush();
                break;
            case GameEventID.Lv1_Finished_Lotus_Paper:
                Lv1_FinishedLotusPaper();
                break;
            case GameEventID.Lv1_Lotus_Paper_Plate:
                Lv1_LotusPaperPlate();
                break;
            case GameEventID.Lv1_CheckFilialPietyCurtain:
                Lv1_CheckFilialPietyCurtain();
                break;
            case GameEventID.Lv1_Faucet:
                Lv1_Faucet();
                break;
            case GameEventID.Lv1_Piano:
                Lv1_Piano();
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
            case GameEventID.Close_UI:
                CloseUI();
                break;
            case GameEventID.Lv2_Light_Switch:
                Lv2_LightSwitch();
                break;
            case GameEventID.Lv2_Room_Door_Lock:
                Lv2_RoomDoorLock();
                break;
            case GameEventID.Lv2_FlashLight:
                Lv2_FlashLight();
                break;
            case GameEventID.Lv2_Door_Knock_Stop:
                Lv2_DoorKnockStop();
                break;
            case GameEventID.Lv2_Grandma_Door_Open:
                Lv2_GrandmaDoorOpen();
                break;
            case GameEventID.Lv2_Grandma_Door_Close:
                Lv2_GrandmaDoorClose();
                break;
            default:
                Debug.LogError(string.Format("[Lv2_Event] Error GameEventID : {0}", r_EventID));
                break;
        }
    }
}