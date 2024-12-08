using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

using DG.Tweening;

public class SceneController_Room : SceneController
{
    [SerializeField] LevelTypeID CurrentLevel;

    #region - 待整理的欄位 -
    [Header("遊戲結束畫面UI")] public GameObject FinalUI;
    [SerializeField] [Header("洗手台的水")] GameObject WaterSurfaceObj;

    [SerializeField] [Header("鋼琴提示介面")] GameObject PianoUI;
    [SerializeField] [Header("電視 White noise 材質球")] Material Lv1_matTVWhiteNoise;

    [SerializeField] [Header("阿嬤收尾嚇人影片 UI")] RawImage RawImgGrandmaUI;
    #endregion

    #region Light Zone
    public GameObject goPhotoFrameLight;
    #endregion

    #region - All Scene Items -
    [Header("場景一物件")]
    [SerializeField] [Header("Lv1_阿嬤的房間門")] ItemController Lv1_Grandma_ROOM_Door_Item;
    [SerializeField] [Header("Lv1_房間燈開關")] ItemController Lv1_Light_Switch_Item;
    [SerializeField] [Header("Lv1_手電筒")] ItemController Lv1_FlashLight_Item;
    [SerializeField] [Header("Lv1_水龍頭")] ItemController Lv1_Faucet_Item;
    [SerializeField] [Header("Lv1_水龍頭水粒子")] GameObject Lv1_Faucet_Flush_Obj;
    [SerializeField] [Header("Lv1_廁所門")] ItemController Lv1_Toilet_Door_Item;
    [SerializeField] [Header("Lv1_鋼琴")] ItemController Lv1_Piano_Item;
    [SerializeField] [Header("Lv1_娃娃 Ani")] Animator Lv1_Doll_Ani;

    [SerializeField] [Header("Lv1_還沒摺的蓮花紙")] GameObject Lv1_Lotus_Paper_Obj;
    [SerializeField] [Header("Lv1_蓮花紙旁的蠟燭")] GameObject Lv1_Lotus_Candle_Obj;
    [SerializeField] [Header("Lv1_摺好的紙蓮花")] GameObject Lv1_Finished_Lotus_Paper_Obj;
    [SerializeField] [Header("Lv1_放紙蓮花的盤子")] GameObject Lv1_Lotus_Paper_Plate_Obj;

    [Header("場景二物件")]
    [SerializeField] [Header("Lv2_手電筒")] ItemController Lv2_FlashLight_Item;
    [SerializeField] [Header("Lv2_小邊桌")] ItemController Lv2_SideTable_Item;
    [SerializeField] [Header("Lv2_阿嬤房間門")] ItemController Lv2_Grandma_Room_Door_Item;

    [SerializeField] [Header("Lv2_鬼阿嬤")] GameObject Lv2_Grandma_Ghost_Obj;
    [SerializeField] [Header("Lv2_廚房物件_狀態一")] GameObject Lv2_Furniture_State_1_Obj;
    [SerializeField] [Header("Lv2_廚房物件_狀態二")] GameObject Lv2_Furniture_State_2_Obj;
    [SerializeField] [Header("Lv2_廁所鬼頭")] GameObject Lv2_Toilet_Door_GhostHead_Obj;
    [SerializeField] [Header("Lv2_阿嬤哭聲撥放器")] GameObject Lv2_Grandma_Cry_Audio_Obj;
    [SerializeField] [Header("Lv2_走廊門框")] GameObject Lv2_Corridor_Door_Frame_Obj;
    [SerializeField] [Header("Lv2_取代走廊門框的牆壁")] GameObject Lv2_Wall_Replace_Door_Frame_Obj;
    #endregion

    #region Game Point
    bool bLv1_HasGrandmaRoomKey = false;
    bool bLv1_HasFlashlight = false;
    bool bLv1_TriggerRiceFuneral = false;

    bool bLv2_HasGrandmaRoomKey = false;
    bool bLv2_HasFlashlight = false;
    bool bLv2_TriggerLastAnimateAfterPhotoFrame = false;

    bool bIsPlayingPiano = false;
    bool bIsPlayingLotus = false;
    bool bIsGameEnd = false;
    bool bHasTriggerLotus = false;
    int m_iGrandmaRushCount;
    #endregion

    #region - External Override -
    public override void Awake()
    {
        //base.Awake(); 
    }

    public override void Start()
    {
        //RegisterButton();
        //base.Start();

        GameEventCtrlr.RecGameEvent(LevelTypeID.Lv1_GrandmaHouse, GameEventID.Lv1_TalkToPackage);
    }

    public override void KeyboardCheck() { }

    public override void Lv1_EventCallBack(int r_EventID)
    {
        switch ((Lv1_EventCallBackID)r_EventID)
        {
            case Lv1_EventCallBackID.none:
                break;
            case Lv1_EventCallBackID.Lv1_TalkToPackage:
                ShowHint(HintItemID.Lv1_OpenRoomDoor);
                break;
            default:
                Debug.LogError(string.Format("[Lv1_EventCallBack] Error GameEventID : {0}", r_EventID));
                break;
        }
    }

    public override void ShowHint(HintItemID r_ItemID)
    {
        base.ShowHint(r_ItemID);
    }
    #endregion

    #region - Basic Function -
    void RegisterButton()
    {
        // 返回
        ExitBtn.onClick.AddListener(() => ButtonFunction(ButtonEventID.UI_Back));

        // 進入蓮花遊戲
        EnterGameBtn.onClick.AddListener(() => ButtonFunction(ButtonEventID.Enter_Game));
    }

    // 執行玩家移動到指定區域
    public IEnumerator ProcessPlayerSetPianoAni(int index)
    {
        bIsPlayingPiano = true;
        Transform tfPianoPos = GameObject.Find("PianoTarget").GetComponent<Transform>();
        Transform tfCameraPos = tfPianoPos.GetChild(0);

        yield return StartCoroutine(PlayerToAniPos(Targers[index].position, tfPianoPos.rotation, tfCameraPos.rotation));

        if (bIsPlayingPiano == true)
            PianoUI.SetActive(true);
    }

    void QuitPiano()
    {
        bIsPlayingPiano = false;
        PianoUI.SetActive(false);
        playerCtrlr.m_bCanControl = true;
        playerCtrlr.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        playerCtrlr.gameObject.GetComponent<Rigidbody>().useGravity = true;
    }

    void ShowQRCode()
    {
        bIsGameEnd = false;
        m_bReturnToBegin = true;
        FinalUI.SetActive(false);
        QRCodeUI.SetActive(true);
    }

    // 延遲載入大廳場景
    IEnumerator DelayLodelobby()
    {
        audManager.Play(1, "Opening_Scene", false);
        FinalUI.SetActive(true);
        SceneManager.LoadScene(0);
        yield return null;
    }

    void QuitLotusGame()
    {
        bIsPlayingLotus = false;
        playerCtrlr.tfPlayerCamera.gameObject.SetActive(true);
        Lv1_Lotus_Paper_Obj.transform.localPosition = new Vector3(-3.9f, 0.6f, -2.4f);

        playerCtrlr.transform.localPosition = new Vector3(-3, 0.8f, -2.5f);
        playerCtrlr.m_bCanControl = true;
        playerCtrlr.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        playerCtrlr.gameObject.GetComponent<Rigidbody>().useGravity = true;

        LotusGameManager LotusCtrlr = GameObject.Find("LotusGameController").GetComponent<LotusGameManager>();
        LotusCtrlr.SendMessage("SetLotusCanvasEnable", false);

        LotusGameManager.bIsGamePause = true;
    }

    // 離開蓮花遊戲
    public void ExitLotusGame()
    {
        m_bPlayLotusEnable = false;
        bIsPlayingLotus = false;
        playerCtrlr.m_bCanControl = true;
        playerCtrlr.tfPlayerCamera.gameObject.SetActive(true);
        playerCtrlr.transform.localPosition = new Vector3(-3, 0.8f, -2.5f);

        playerCtrlr.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        playerCtrlr.gameObject.GetComponent<Rigidbody>().useGravity = true;

        SceneManager.UnloadSceneAsync(3);

        Lv1_Lotus_Paper_Obj.transform.localPosition = new Vector3(-3.9f, -2f, -2.4f);
        Lv1_Finished_Lotus_Paper_Obj.transform.localPosition = new Vector3(-3.9f, 0.6f, -2.4f);
    }

    public void ButtonFunction(ButtonEventID _eventID)
    {
        switch (_eventID)
        {
            case ButtonEventID.UI_Back:
                break;
            case ButtonEventID.Enter_Game:
                if (bIsUIOpen)
                {
                    RestoreItemLocation();
                    bIsPlayingLotus = true;

                    Transform tfPlayingLotusPos = GameObject.Find("Lv1_Playing_Lotus_Pos").GetComponent<Transform>();
                    Transform tfCameraPos = tfPlayingLotusPos.GetChild(0);
                    StartCoroutine(PlayerToAniPos(tfPlayingLotusPos.position, tfPlayingLotusPos.rotation, tfCameraPos.rotation));
                }
                break;
        }
    }
    #endregion
}