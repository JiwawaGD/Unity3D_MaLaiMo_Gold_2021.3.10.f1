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
    Close_UI = 0,

    // Lv1 : Scene_1
    Lv1_TalkToPackage = 1,
    Lv1_GrandmaRoomDoorOpen,

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
    Empty = 0,
    Player_Wake_Up,
    Player_Turn_After_Photo_Frame,
    Player_PlayPiano,
}

public enum Lv1_Dialogue
{
    Empty = 0,
}