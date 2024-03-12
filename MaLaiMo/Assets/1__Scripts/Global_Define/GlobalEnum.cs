public enum GameEventID
{
    Close_UI = 0,

    // S1 : Scene_1
    S1_Photo_Frame,
    S1_Grandma_Door_Open,
    S1_Grandma_Room_Door_Lock,
    S1_Lotus_Paper,
    S1_Grandma_Dead_Body,
    S1_White_Tent,
    S1_Photo_Frame_Light_On,
    S1_Grandma_Rush,
    S1_Light_Switch,
    S1_Flashlight,
    S1_Desk_Drawer,
    S1_GrandmaRoomKey,
    S1_Rice_Funeral,
    S1_Rice_Funeral_Spilled,
    S1_Toilet_Door_Lock,
    S1_Toilet_Door_Open,
    S1_Toilet_Ghost_Hide,
    S1_Toilet_Ghost_Hand_Push,
    S1_Photo_Frame_Has_Broken,
    S1_Finished_Lotus_Paper,
    S1_Lotus_Paper_Plate,
    S1_Grandma_Pass_Door_After_RiceFurnel,
    Lv1_CheckFilialPietyCurtain,
    Lv1_Faucet,
    Lv1_Piano,

    // S2 : Scene_2
    S2_Light_Switch = 101,
    S2_Room_Door_Lock,
    S2_FlashLight,
    S2_Side_Table,
    S2_Room_Key,
    S2_Door_Knock_Stop,
    S2_Grandma_Door_Open,
    S2_Grandma_Door_Close,
    S2_Ghost_Pass_Door,
    S2_Rice_Funeral,
    S2_Photo_Frame, // 現在沒再用 可替換
    S2_Toilet_Door,
    Lv2_Ruce_Funeral_Plate,
    Lv2_Boy_Sneaker,
}

public enum HintItemID
{
    Empty = 0,

    // S1 : Scene_1
    S1_Light_Switch,
    S1_Grandma_Room_Door,
    S1_Flashlight,
    S1_Desk_Drawer,
    S1_Grandma_Room_Key,
    S1_Filial_Piety_Curtain,
    S1_Lie_Grandma_Body,
    S1_Rice_Funeral,
    S1_Lotus_Paper,
    S1_Grandma_Room_Door_Lock,
    S1_Rice_Funeral_Spilled,
    S1_Photo_Frame,
    S1_Toilet_Door,
    S1_Toilet_GhostHand_Trigger,
    S1_Photo_Frame_Has_Broken,
    S1_Finished_Lotus_Paper,
    S1_Lotus_Paper_Plate,
    Lv1_Faucet,
    Lv1_Piano,

    // S2 : Scene_2
    S2_Light_Switch = 101,
    S2_Room_Door,
    S2_FlashLight,
    S2_Side_Table,
    S2_Room_Key,
    S2_Grandma_Room_Door_Open,
    S2_Rice_Funeral,
    S2_Photo_Frame,
    S2_Toilet_Door,
    Lv2_Ruce_Funeral_Plate,
    Lv2_Boy_Sneaker,
}

public enum ObjItemID
{
    Empty = 0,

    // S1 : Scene_1
    S1_Rice_Funeral,
    S1_Lotus_Paper,
    S1_Photo_Frame,
    S1_Photo_Grandma,
    S2_Photo_Frame,
    S2_Photo_Frame_Floor
}

public enum UIItemID
{
    Empty = 0,

    // S1 : Scene_1
    S1_Rice_Funeral,
    S1_Photo_Frame,
    S1_Lotus_Paper,
    S1_Grandma_Dead_Body,
    S1_White_Tent,
    S1_Photo_Grandma,
    S2_Photo_Frame
}

public enum ButtonEventID
{
    UI_Back,
    Enter_Game,
}

public enum PlayerAnimateType
{
    Empty,
    Player_Wake_Up,
    Player_Turn_After_Photo_Frame,
    Player_PlayPiano,
}
public enum IntelligenceLevel
{
    Unknown = 0,
    Level1 = 1,
    Level2 = 2,
    Level3 = 3,
    Level4 = 4,
    Level5 = 5
}

public enum Lv1_Dialogue
{
    Begin = 0,
    OpenDoor_Nokey_Lv1,
    OpenLight_Lv1,
    GetKey_Lv1,
    OpenDoor_GetKey_Lv1,
    AfterPlayLotus_Lv1,
    OpenFilialPietyCurtain_Lv1,
    CheckRiceFuneral_OnFloor_Lv1,
    DoorLocked_Lv2,
    OpenDoor_NoFlashLight_Lv1,
    OpenBathRoomDoor_Nokey_Lv1,
    OpenLight_Lv2,
    OpenFlashLight_Lv2,
    CheckRiceFuneral_OnFloor_Lv2,
    WakeUp_Lv2,
    CheckFilialPietyCurtain_Lv1,
    CheckLotus_Lv1,
    HeardBathRoomSound_Lv1,
    Lv1_OpenLight_HasFlashlight,
    Lv2_PhotoFrameFall,
    Lv2_PutPhotoFrameBack,
    Lv2_Boy_Sneaker,
}