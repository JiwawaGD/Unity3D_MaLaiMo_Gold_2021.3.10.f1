// 主要記錄全部 Class

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

    public class ItemEvent
    {
        public SceneTypeID sceneTypeID;

        public ItemEvent()
        {
            switch (sceneTypeID)
            {
                case SceneTypeID.BeginScene:
                    break;
                case SceneTypeID.Introduce:
                    break;
                case SceneTypeID.Lv1_GrandmaHouse:
                    break;
                case SceneTypeID.Lv2_GrandmaHouse:
                    break;
            }
        }
    }
}