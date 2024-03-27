// 主要紀錄全部 Enum

public enum SceneTypeID
{
    BeginScene = 0,
    Introduce,
    Lv1_GrandmaHouse,
    Lv2_GrandmaHouse,
}

public enum GameEventID
{
    Close_UI = 0,

    // Lv1 : Scene_1
    Lv1_Photo_Frame,
    Lv1_Grandma_Door_Open,
    Lv1_Grandma_Room_Door_Lock,
    Lv1_Lotus_Paper,
    Lv1_Grandma_Dead_Body,
    Lv1_Filial_Piety_Curtain,
    Lv1_Photo_Frame_Light_On,                   // *TODO* 無使用可替換
    Lv1_Grandma_Rush,                           // *TODO* 無使用可替換
    Lv1_Light_Switch,
    Lv1_Flashlight,
    Lv1_Desk_Drawer,
    Lv1_GrandmaRoomKey,
    Lv1_Rice_Funeral,
    Lv1_Rice_Funeral_Spilled,
    Lv1_Toilet_Door_Lock,
    Lv1_Toilet_Door_Open,
    Lv1_Toilet_Ghost_Hide,                      // *TODO* 無使用可替換
    Lv1_Toilet_Ghost_Hand_Push,
    Lv1_Photo_Frame_Has_Broken,
    Lv1_Finished_Lotus_Paper,
    Lv1_Lotus_Paper_Plate,
    Lv1_Grandma_Pass_Door_After_RiceFurnel,
    Lv1_CheckFilialPietyCurtain,
    Lv1_Faucet,
    Lv1_Piano,

    // Lv2 : Scene_2
    Lv2_Light_Switch = 101,
    Lv2_Room_Door_Lock,
    Lv2_FlashLight,
    Lv2_Side_Table,
    Lv2_Room_Key,
    Lv2_Door_Knock_Stop,
    Lv2_Grandma_Door_Open,
    Lv2_Grandma_Door_Close,
    Lv2_Ghost_Pass_Door,
    Lv2_Rice_Funeral,
    Lv2_Photo_Frame, // 現在沒再用 可替換
    Lv2_Toilet_Door,
    Lv2_Piano_Stool,
    Lv2_Boy_Sneaker,
}

public enum HintItemID
{
    Empty = 0,

    // Lv1 : Scene_1
    Lv1_Light_Switch,
    Lv1_Grandma_Room_Door,
    Lv1_Flashlight,
    Lv1_Desk_Drawer,
    Lv1_Grandma_Room_Key,
    Lv1_Filial_Piety_Curtain,
    Lv1_Lie_Grandma_Body,
    Lv1_Rice_Funeral,
    Lv1_Lotus_Paper,
    Lv1_Grandma_Room_Door_Lock,
    Lv1_Rice_Funeral_Spilled,
    Lv1_Photo_Frame,
    Lv1_Toilet_Door,
    Lv1_Toilet_GhostHand_Trigger,
    Lv1_Photo_Frame_Has_Broken,
    Lv1_Finished_Lotus_Paper,
    Lv1_Lotus_Paper_Plate,
    Lv1_Faucet,
    Lv1_Piano,

    // Lv2 : Scene_2
    Lv2_Light_Switch = 101,
    Lv2_Room_Door,
    Lv2_FlashLight,
    Lv2_Side_Table,
    Lv2_Room_Key,
    Lv2_Grandma_Room_Door_Open,
    Lv2_Rice_Funeral,
    Lv2_Photo_Frame,
    Lv2_Toilet_Door,
    Lv2_Ruce_Funeral_Plate,
    Lv2_Boy_Sneaker,
}

public enum ObjItemID
{
    Empty = 0,

    // Lv1 : Scene_1
    Lv1_Rice_Funeral,
    Lv1_Lotus_Paper,
    Lv1_Photo_Frame,
    Lv1_Photo_Grandma,
    Lv2_Photo_Frame,
    Lv2_Photo_Frame_Floor
}

public enum UIItemID
{
    Empty = 0,

    // Lv1 : Scene_1
    Lv1_Rice_Funeral,
    Lv1_Photo_Frame,
    Lv1_Lotus_Paper,
    Lv1_Grandma_Dead_Body,
    Lv1_White_Tent,
    Lv1_Photo_Grandma,
    Lv2_Photo_Frame
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