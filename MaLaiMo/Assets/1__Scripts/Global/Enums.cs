// 主要紀錄全部 Enum

public enum LevelTypeID
{
    BeginScene = 0,
    Introduce,
    Lv1_GrandmaHouse,
    Lv2_OutSideDoor,
}

public enum GameEventID
{
    None = 0,

    // Lv1 : Scene_1
    Lv1_TalkToPackage = 1,
    Lv1_GrandmaRoomDoorSwitch,
    Lv1_FirstTalkToMom,
    Lv1_ClipBoard,
    Lv1_GoOutSide,
    Lv1_LotusPaper,
    Lv1_Piano,
    Lv1_Event_HoldFinishLotusPaper,

    // Lv2 : Scene_2
    Lv2_Begin = 101,
    Lv2_GoInside,
    Lv2_TalkToMom ,
    Lv2_CheckPaper,
    Lv2_PutLotusPaper,
    LV2_10Dollar,
    Lv2_FlowerCircle
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
    Lv1_OpenRoomDoor,
    Lv1_FirstTalkToMom,
    Lv1_ClipBoard,
    Lv1_Item_GoOutSide,
    Lv1_Item_LotusPaper,
    Lv1_Piano,
    Lv1_Item_FinishedLotus,

    // Lv2 : Scene_2
    Lv2_Begin = 101,
    Lv2_OutSideDoor,
    Lv2_Mom ,
    Lv2_Paper,
    Lv2_Table,
    LV2_10Dollar,
    Lv2_FlowerCircle
}

public enum ObjItemID
{
    // Lv1 : Scene_1室內
    Lv1_Rice_Funeral = 0,
    Lv1_Lotus_Paper,
    Lv1_Photo_Frame,
    Lv1_Photo_Grandma,
    Lv2_Photo_Frame,
    Lv2_Photo_Frame_Floor,

    // Lv2 : Scene_2戶外
    Lv2_Paper = 0,
    Lv2_10Dollar = 1,

}

public enum UIItemID
{
    Empty = 0,

    // Lv1 : Scene_1
    Lv1_UI_LotusPaper = 1,

    // Lv2 : Scene_2
    Lv2_Paper = 0,
    Lv2_Begin,
    Lv2_10Dollar = 2, 
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

public enum Room_Dialogue
{
    Lv_null = 0,

    //LV1
    Lv1_000_FacePackage = 1,
    Lv1_001_HintMove,
    Lv1_002_OpenDoor,
    Lv1_004_E_Coin,
    Lv1_005_CoinPass,
    Lv1_006_CoinNotPass,
    Lv1_007_E_Table,
    Lv1_008_E_FlowerCircle,
    Lv1_009_E_Lotus_Finish,
    Lv1_010_E_Lotus_AlreadyFinish,
    Lv1_011_E_Table,
    Lv1_012_TalkWithMom_NotFinish,
    Lv1_013_TalkWithMom_Finish,
    Lv1_014_E_Wardrobe,
    Lv1_015_E_Painting,
    Lv1_016_E_Door_NotPainting,
    Lv1_017_E_Door_Painting,

    //LV_2
    Lv2_000_Begin = 1,
    Lv2_001_E_FilialPietyCurtain,
    Lv2_002_E_Seat,
    Lv2_003_E_Grandmother,
    Lv2_004_E_Drink,
    Lv2_005_BackLivingRoom,
    Lv2_006_E_Flashlight,

    //LV_4
    Lv4_000_Begin = 0,
    Lv4_001_E_Calendar,
    Lv4_002_DoorOpen,
    Lv4_003_FlushSound,
    Lv4_004_DropIntoWaterSound,
    Lv4_005_TubStrangeSound,
    Lv4_006_E_Door,
    Lv4_007_TearCalendar,
    Lv4_008_NearLivingRoomTable,
    Lv4_09_TearCalendar,
    Lv4_010_E_Telephone,
    Lv4_011_TakeOnPhone,
    Lv4_012_KnockDoorSound,
    Lv4_013_E_Piano,
    Lv4_014_CorrectMelody,
    Lv4_015_E_Amulet,
    Lv4_016_E_Bed,
    Lv4_017_E_Amulet,
    Lv4_018_E_CalendarPaper,
    Lv4_019_AfterAnimation,
    Lv4_020_E_Tub,

    //LV_5
    Lv5_000_E_CalendarBook = 1,
    Lv5_001_E_Door,
    Lv5_002_StartCeremony,
    Lv5_003_ConductCeremony,
    Lv5_004_E_Hug,
    Lv5_005_E_Amulet
}

public enum OutSide_Dialogue
{
    //LV_2
    Lv2_000_E_Mother_First = 0,
    Lv2_001_E_Mother_PaperNotFinish,
    Lv2_002_E_Mother_PaperFinish,
    Lv2_003_E_FlowerCircle,
    Lv2_004_FirstSevenDays_Half,
    Lv2_007_GoOut,
}

public enum Forest_Dialogue
{
    //LV_3
    Lv3_000_LookNote = 0,
    Lv3_001_LeaveForest,
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
}

public enum Lv3_Dialogue
{
    Lv3_000_LookNote = 0,
    Lv3_001_LeaveForest
}

public enum PaperMission
{
    PutLotusOnTable = 0,
    TalkRiceToKitchen,
    LayOutSideCircle,
}
