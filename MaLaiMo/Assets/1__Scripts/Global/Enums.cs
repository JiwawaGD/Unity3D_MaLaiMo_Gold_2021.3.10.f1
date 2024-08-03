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
    Lv1_Begin = 1,

    // Lv2 : Scene_2
    Lv2_Begin = 101,
}

public enum HintItemID
{
    Empty = 0,

    // Lv1 : Scene_1
    Lv1_Begin = 1,

    // Lv2 : Scene_2
    Lv2_Begin = 101,
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
    Lv1_Begin = 1,

    // Lv2 : Scene_2
    Lv2_Begin = 101,
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