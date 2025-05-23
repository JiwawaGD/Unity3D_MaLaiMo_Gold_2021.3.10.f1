public partial class GlobalDeclare
{
    // 主要紀錄 Static 項目
    #region Game Setting
    public static float fMusic;          // 音樂音量
    public static float fSoundEffect;    // 音效音量
    public static float fQuality;        // 畫質
    public static float fSensitivity;    // 靈敏度
    public static bool bCrossHairEnable; // 準心開關
    #endregion

    public static bool bCanControl = true;  // 是否可以控制 (用於遊戲暫停)

    public static byte byCurrentDialogIndex;

    #region Strings
    public readonly static string[] StoryMessage = new string[]
    {
        "",
    };

    public readonly static string[] item_Title_Room = new string[]
    {
        "",
        "蓮花摺紙",
        "紙條"
    };

    public readonly static string[] item_Title_OutSide = new string[]
    {
        "紙條",
        "",
        "兩枚十元硬幣"
    };

    public readonly static string[] item_Title_Forest = new string[]
    {

    };

    public readonly static string[] item_MainInfo_Room = new string[]
    {
        "",
        "佛教信仰中，紙蓮花具有祝福消業障之意，\r\n" +
        "希望往生親人能夠乘坐於蓮花上，\r\n" +
        "順利地前往西方極樂世界。\r\n",
    };

    public readonly static string[] item_MainInfo_OutSide = new string[]
    {
        "紙條上是媽媽的手寫字跡，寫著希望我幫忙做的事情。\r\n " +
        "1.準備九朵紙蓮花，放到靈堂的桌上\r\n" +
        "2.把給阿嬤的拜飯拿去廚房\r\n" +
        "3.擺好靈堂的花圈",
        "在道教信仰中，信眾會擲筊請求神明指示，\r\n" +
        "當出現「一凸一平」，表示神明同意，為「聖杯」；\r\n "+
        "出現「兩凸面朝上」，表示不同意，為「無杯」；\r\n"+
        "如果擲到「兩平面朝上」，則表示神明沒有確切答覆，為「笑杯」，需要再重擲一次。\r\n"+
        "在許多老一輩的觀念中，會用硬幣取代擲筊與亡者溝通。\r\n "+
        "文字代表凸面，人頭代表平面。\r\n",
        "10元代替",
    };

    public readonly static string[] item_MainInfo_Forest = new string[]
    {

    };


    public readonly static string[] item_BottonInfo_Room = new string[]
    {
        "",
        "按 <size=32><color=red><b>R</b></color></size> 開始摺蓮花\r\n(Press *R* Origami Lotus Paper)",
    };

    public readonly static string[] item_BottonInfo_OutSide = new string[]
    {
       "", // 紙條
        "按 <size=32><color=red><b>R</b></color></size> 繼續",
        "按 <size=32><color=red><b>R</b></color></size> 可互動\r\n " +
        "會有擲硬幣動畫",
    };

    public readonly static string[] item_BottonInfo_Forest = new string[]
    {

    };
    #endregion
}
