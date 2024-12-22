// 主要紀錄全部 Enum

public enum LevelTypeID
{
    BeginScene = 0,
    Introduce,
    Lv1_GrandmaHouse,
    Lv2_GrandmaHouse,
}

public enum GameEventID
{
    None = 0,

    // Lv1 : Scene_1
    Lv1_TalkToPackage = 1,
    Lv1_GrandmaRoomDoorSwitch,

    // Lv2 : Scene_2
    Lv2_Begin = 101,
}

public enum Lv1_EventCallBackID
{
    none = 0,
    Lv1_TalkToPackage = 1,
}

public enum HintItemID
{
    Empty = 0,

    // Lv1 : Scene_1
    Lv1_Begin = 1,
    Lv1_OpenRoomDoor = 2,

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
    Empty = 0,
    FacePackageStandUp,
}

public enum Lv1_Dialogue
{
    Empty = 0,
    FacePackage = 1,
    HintMove = 2,
}

public enum Lv2_Dialogue
{
    Lv2_000_Begin = 0,
    Lv2_001_E_FilialPietyCurtain,
    Lv2_002_E_Seat,
    Lv2_003_E_Grandmother,
    Lv2_004_E_Drink,
    Lv2_005_BackLivingRoom,
    Lv2_006_E_Flashlight,
    Lv2_007_GoOut,
    Lv2_008_LookNote,
    Lv2_009_LeaveForest
}

public enum Lv3_Dialogue
{
    Lv3_000_LookNote = 0,
    Lv3_001_LeaveForest
}
