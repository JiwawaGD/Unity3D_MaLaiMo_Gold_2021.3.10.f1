public partial class GlobalDeclare
{
    #region 玩家狀態
    public class PlayerState
    {
        public static PlayerAnimateType aniType;

        public PlayerState()
        {
            aniType = PlayerAnimateType.Empty;
        }
    }

    public static void SetPlayerAnimateType(PlayerAnimateType r_AniType)
    {
        PlayerState.aniType = r_AniType;
    }

    public static PlayerAnimateType GetPlayerAnimateType()
    {
        return PlayerState.aniType;
    }
    #endregion

    #region 物件動畫
    public class ItemAnimate
    {
        public static string aniObject;
        public static string aniName;

        public ItemAnimate()
        {
            aniObject = "Empty";
            aniName = "Empty";
        }
    }

    public static void SetItemAniObject(string r_AniObject)
    {
        ItemAnimate.aniObject = r_AniObject;
    }

    public static string GetItemAniObject()
    {
        return ItemAnimate.aniObject;
    }

    public static void SetItemAniName(string r_AniName)
    {
        ItemAnimate.aniName = r_AniName;
    }

    public static string GetItemAniName()
    {
        return ItemAnimate.aniName;
    }
    #endregion

    #region 玩家視角限制
    public class PlayerCameraLimit
    {
        public static float fMinView;
        public static float fMaxView;
        public static float fCurrentEulerAngles;

        public PlayerCameraLimit()
        {
            fMinView = 0f;
            fMaxView = 0f;
            fCurrentEulerAngles = 0f;
        }

        public static void SetPlayerCameraLimit(float r_fMinView, float r_fMaxView, float r_fLocalEulerAngles)
        {
            fMinView = r_fMinView;
            fMaxView = r_fMaxView;
            fCurrentEulerAngles = r_fLocalEulerAngles;
        }

        public static float[] GetPlayerCameraLimit()
        {
            float[] TempFloats = new float[3];

            TempFloats[0] = fMinView;
            TempFloats[1] = fMaxView;
            TempFloats[2] = fCurrentEulerAngles;

            return TempFloats;
        }

        public static void ClearValue()
        {
            fMinView = 0;
            fMaxView = 0;
            fCurrentEulerAngles = 0;
        }
    }

    #endregion

    public static byte byCurrentDialogIndex;

    public readonly static string[] StoryMessage = new string[]
    {
        "我的阿嬤，在我很小的時候就過世了",
        "那時候的我，還不知道我看到的是甚麼",
    };

    public readonly static string[] UITitle = new string[]
    {
        "",
        "腳尾飯",
        "阿嬤的照片",
        "蓮花摺紙",
        "阿嬤",
        "孝簾",
        "阿嬤的照片",
        "阿嬤的照片"

    };

    public readonly static string[] TxtInstructionsmage = new string[]
    {
        "",
        "Esc 返回\r\n"+
        "滑鼠 旋轉",
        "Esc 返回\r\n"+
        "滑鼠 旋轉",
        "Esc 返回\r\n"+
        "滑鼠 旋轉",
        "Esc 返回\r\n"+
        "滑鼠 旋轉",
        "Esc 返回\r\n"+
        "滑鼠 旋轉",
        "Esc 返回\r\n"+
        "滑鼠 旋轉",
        "Esc 返回\r\n"+
        "滑鼠 旋轉",
    };

    public readonly static string[] UIIntroduce = new string[]
    {
        "",

        "民間習俗中拜腳尾飯,\r\n" +
        "是於亡者腳邊放置白米飯一碗，\r\n"+
        "飯上放一顆熟鴨(雞)蛋，\r\n" +
        "並直插筷子一雙。\r\n" +
        "其意是供亡靈食用，\r\n" +
        "讓亡靈吃飽飯好趕赴陰間報到，以入冥籍。\r\n",

        "阿嬤生日時爸爸幫奶奶拍的照片。",

        "佛教信仰中，紙蓮花具有祝福消業障之意，\r\n" +
        "希望往生親人能夠乘坐於蓮花上，\r\n" +
        "順利地前往西方極樂世界。\r\n",

        "阿嬤身上有慢性病，\r\n" +
        "前幾年出了一場車禍讓原先身體\r\n" +
        "本來就有慢性病的身體更不好了。\r\n" +
        "車禍後的傷造成又有後遺症，\r\n" +
        "維持了幾年直到前幾天突然就走了。",

        "親人氣絕，孝眷哭畢，\r\n" +
        "即以一匹白布彎九彎，\r\n" +
        "懸掛於屍床之周圍，稱為「吊九條」，\r\n" +
        "俗稱「孝簾」。\r\n" +
        "吊九條目的，在遮日月光線照射，\r\n" +
        "以免死者變成僵屍。\r\n" +
        "除圍九條外，廳門尚須闔一扉，\r\n" +
        "屍在廳左闔左扉，在右闔右扉；\r\n" +
        "又男喪闔左扉，女喪闔右扉，\r\n" +
        "九條與門扉須圍闔至出殯為止。\r\n" +
        "今者以黃布代替白布，旨在隔離內外，防人惡之。",
        "",
        "爸爸幫阿嬤拍的照片\r\n"
    };
    //public readonly static string[] ScheduleText = new string[]
    //{
    //     "當前目標:找尋鑰匙",
    //     "當前目標:周遭尋找線索",
    //     "當前目標:找出聲音來源",
    //     "當前目標:回到客廳",
    //     "當前目標:調查廚房的人影",
    //};
}