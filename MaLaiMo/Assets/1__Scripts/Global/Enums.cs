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
    Lv1_002_OpenDoor,

    // Lv2 : Scene_2
    Lv2_Begin = 101,
}

public enum HintItemID
{
    Empty = 0,

    // Lv1 : Scene_1
    Lv1_Begin = 1,
    Lv1_Door,
    Lv1_Mother,
    Lv1_Paper,
    Lv1_Coin,
    Lv1_Rice,
    Lv1__Table,
    Lv1_Wreath,
    Lv1_LotusPaper,
    Lv1_Wardrobe,
    Lv1_Painting,

    // Lv2 : Scene_2
    Lv2_Begin = 101,
    Lv2_FilialPietyCurtain,
    Lv2_Seat,
    Lv2_Grandmother,
    Lv2_Drink,
    Lv2_Flashlight,

    //Lv3 : Scene_3
    Lv3_Begin = 201,
    Lv3_Toilet,
    Lv3_Sink,
    Lv3_GrandmaRoomDoor,
    Lv3_Gift,
    Lv3_Smile,
    Lv3_RoomDoor,
    Lv3_Telephone,
    Lv3_Piano,
    Lv3_Flower,
    Lv3_Diary,
    Lv3_Amulet,
    Lv3_Bed,
    Lv3_CalendarPaper,
    Lv3_Tub,

    //Lv4 : Scene_4
    Lv4_Begin = 301,
    Lv4_Diary,
    Lv4_Door,
    Lv4_Hug,
    Lv4_Amulet,
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
    KnockDoor = 2,
}