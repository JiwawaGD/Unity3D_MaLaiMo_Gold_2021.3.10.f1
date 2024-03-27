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
            case GameEventID.Lv1_Grandma_Dead_Body:
                Lv1_GrandmaDeadBody();
                break;
            case GameEventID.Lv1_Filial_Piety_Curtain:
                Lv1_FilialPietyCurtain();
                break;
            case GameEventID.Lv1_Photo_Frame_Light_On:
                break;
            case GameEventID.Lv1_Grandma_Rush:
                break;
            case GameEventID.Lv1_Light_Switch:
                Lv1_LightSwitch();
                break;
            case GameEventID.Lv1_Flashlight:
                Lv1_Flashlight();
                break;
            case GameEventID.Lv1_Desk_Drawer:
                Lv1_DeskDrawer();
                break;
            case GameEventID.Lv1_GrandmaRoomKey:
                Lv1_GrandmaRoomKey();
                break;
            case GameEventID.Lv1_Rice_Funeral:
                Lv1_RiceFuneral();
                break;
            case GameEventID.Lv1_Rice_Funeral_Spilled:
                Lv1_RiceFuneralSpilled();
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
            case GameEventID.Lv1_Photo_Frame_Has_Broken:
                Lv1_PhotoFrameHasBroken();
                break;
            case GameEventID.Lv1_Finished_Lotus_Paper:
                Lv1_FinishedLotusPaper();
                break;
            case GameEventID.Lv1_Lotus_Paper_Plate:
                Lv1_LotusPaperPlate();
                break;
            case GameEventID.Lv1_Grandma_Pass_Door_After_RiceFurnel:
                Lv1_GrandmaPassDoorAfterRiceFurnel();
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
            case GameEventID.Lv2_Side_Table:
                Lv2_SideTable();
                break;
            case GameEventID.Lv2_Room_Key:
                Lv2_RoomKey();
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
            case GameEventID.Lv2_Ghost_Pass_Door:
                Lv2_GhostPassDoor();
                break;
            case GameEventID.Lv2_Rice_Funeral:
                Lv2_Rice_Funeral();
                break;
            case GameEventID.Lv2_Photo_Frame:
                Lv2_Photo_Frame();
                break;
            case GameEventID.Lv2_Toilet_Door:
                Lv2_ToiletDoor();
                break;
            case GameEventID.Lv2_Piano_Stool:
                Lv2_PianoStool();
                break;
            case GameEventID.Lv2_Boy_Sneaker:
                Lv2_BoySneaker();
                break;
            default:
                Debug.LogError(string.Format("[Lv2_Event] Error GameEventID : {0}", r_EventID));
                break;
        }
    }
}