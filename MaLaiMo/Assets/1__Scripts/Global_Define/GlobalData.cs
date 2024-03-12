public partial class GlobalDeclare
{
    #region Game Setting
    public static float fMusic;          // 音樂音量
    public static float fSoundEffect;    // 音效音量
    public static float fQuality;        // 畫質
    public static float fSensitivity;    // 靈敏度
    public static bool bCrossHairEnable; // 準心開關
    #endregion

    public static bool bLotusGameComplete = false;  // 蓮花遊戲是否完成
    public static bool bCanControl = true;  // 是否可以控制 (用於遊戲暫停)
}
