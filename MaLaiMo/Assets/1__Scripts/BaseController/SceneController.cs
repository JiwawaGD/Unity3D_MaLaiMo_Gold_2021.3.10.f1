using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

using DG.Tweening;

public partial class SceneController : MonoBehaviour
{
    // 確定需要保留的區域
    [SerializeField] protected GameEventController GameEventCtrlr;
    //

    public static SceneController instance;
    public string CurrentDialogue;

    [Space]
    [SerializeField] Volume CameraVolume;

    [Header("Volume參數設定")]
    [SerializeField]
    protected float fTargetIntensity = 1f;
    readonly float fChangeSpeed = 1f;

    [SerializeField] GameObject[] taskListUi;
    [Space]
    [Header("物件旋轉參數設定")]
    protected bool isMoveingObject = false;    // 是否正在移動物件
    protected Vector3 originalPosition;    // 原始位置
    protected Quaternion originalRotation; // 原始旋轉

    [SerializeField] protected AUDManager audManager;
    // 音效管理器

    [Header("物件移動速度")] public float objSpeed;
    [Header("旋轉物件功能")] public bool romanager;
    [Header("全域變數")] public Volume postProcessVolume;
    [Header("物件位置")] public GameObject itemObjTransform;
    [Header("生成後物件")] public GameObject[] RO_OBJ;
    [Header("儲存生成物件")] public int saveRotaObj;
    [Header("攝影棚畫面UI")] public GameObject StudioUI;
    [Header("旋轉物件使用燈關")] public Light Ro_Light;
    [Header("玩家")] public PlayerController playerCtrlr;
    [SerializeField] [Header("對話程序")] public DialogueManager[] DialogueObjects;
    [SerializeField] [Header("設定頁面")] public GameObject settingObjects;
    [SerializeField] [Header("Video 撥放器")] public VideoPlayer videoPlayer;
    [SerializeField] [Header("QRCode UI")] public GameObject QRCodeUI;
    [SerializeField] [Header("準心 UI")] public GameObject CrosshairUI;
    [SerializeField] [Header("追蹤物件位置")] protected Transform[] Targers;
    protected Scene currentScene;

    ItemController TempItem;

    #region Canvas Zone
    public GameObject goCanvas;
    Image imgUIBackGround;
    Text txtTitle;

    protected Image imgInstructions;
    protected Text txtInstructions;
    protected Text txtIntroduce;

    protected Button ExitBtn;
    protected Text txtEnterGameHint;
    protected Button EnterGameBtn;
    #endregion

    #region Static Boolean Zone
    public static bool m_bInUIView = false;
    public static bool m_bIsEnterGameView = false;
    public static bool m_bShowPlayerAnimate = false;
    public static bool m_bShowItemAnimate = false;
    public static bool m_bShowDialog = false;
    public static bool m_bSetPlayerViewLimit = false;
    public static bool m_bGrandmaRush = false;
    public static bool m_bReturnToBegin = false;
    public static bool m_bPlayLotusEnable = false;
    public static bool m_bToiletGhostHasShow = false;
    #endregion

    #region - Empty Field => For Memory -
    public BoxCollider TempBoxCollider;
    public GameObject TempGameObject;
    #endregion

    protected bool bIsPaused = false;
    protected bool bIsMouseEnabled = false;
    protected bool bIsUIOpen = false;
    protected bool bNeedShowDialog = false;

    // 以上未還未整理的程式碼

    #region - External Virtual -
    public virtual void Awake()
    {
        if (playerCtrlr == null)
            playerCtrlr = GameObject.Find("_Player/LingLing").GetComponent<PlayerController>();

        audManager = playerCtrlr.GetComponentInChildren<AUDManager>();

        if (goCanvas == null)
            goCanvas = GameObject.Find("_Canvas/_UICanvas");

        imgUIBackGround = goCanvas.transform.GetChild(0).GetComponent<Image>();     // 背景
        txtTitle = goCanvas.transform.GetChild(2).GetComponent<Text>();             // 標題

        imgInstructions = goCanvas.transform.GetChild(3).GetComponent<Image>();             // 說明圖示
        txtInstructions = goCanvas.transform.GetChild(3).GetComponentInChildren<Text>();    // 說明文字
        txtIntroduce = goCanvas.transform.GetChild(4).GetComponentInChildren<Text>();       // 介紹文字

        ExitBtn = goCanvas.transform.GetChild(5).GetComponent<Button>();            // 返回按鈕

        txtEnterGameHint = goCanvas.transform.GetChild(6).GetComponent<Text>();     // 進入遊戲提示
        EnterGameBtn = goCanvas.transform.GetChild(7).GetComponent<Button>();       // 進入遊戲按鈕

        TempItem = null;    // 暫存物件
        currentScene = SceneManager.GetActiveScene();   // 當前場景
        Ro_Light.enabled = false;   // 旋轉物件使用燈關
        StudioUI.SetActive(false);  // 攝影棚畫面UI
    }

    public virtual void Start()
    {
        SetCrosshairEnable(true);

        // 尚未完成前情提要的串接，因此先在 Start 的地方跑動畫
        //playerCtrlr.gameObject.GetComponent<Animation>().PlayQueued("Player_Wake_Up");
    }

    /// <summary>
    /// 使物件顯示眼睛圖案 & 可互動
    /// </summary>
    /// <param name="r_ItemID">物件的 ID</param>
    public virtual void ShowHint(HintItemID r_ItemID)
    {
        ItemController NextItem = null;

        switch (r_ItemID)
        {
            case HintItemID.Empty:
                break;
            case HintItemID.Lv1_Begin:
                break;
            case HintItemID.Lv1_OpenRoomDoor:
                NextItem = GameObject.Find("__ITEMS/__Level_1/Lv1_Door/Lv1_Grandma_Room_Door").GetComponent<ItemController>();
                break;
            case HintItemID.Lv2_Begin:
                break;
            default:
                break;
        }

        NextItem.bActive = true;
        NextItem.SetHintable(true);
    }

    public virtual void GameEvent(LevelTypeID r_SceneTypeID, GameEventID r_EventID)
    {
        GameEventCtrlr.RecGameEvent(r_SceneTypeID, r_EventID);
    }

    public virtual void RecEventCallback(LevelTypeID r_SceneTypeID, int r_iCallBackID)
    {
        switch (r_SceneTypeID)
        {
            case LevelTypeID.BeginScene:
                break;
            case LevelTypeID.Introduce:
                break;
            case LevelTypeID.Lv1_GrandmaHouse:
                Lv1_EventCallBack(r_iCallBackID);
                break;
            case LevelTypeID.Lv2_GrandmaHouse:
                break;
        }
    }

    public virtual void Lv1_EventCallBack(int r_EventID) { }
    #endregion

    public void Update()
    {
        KeyboardCheck();

        if (bIsPaused && bIsMouseEnabled)
            MouseCheck();
    }

    public void SetGameSetting()
    {
        SetCrosshairEnable(GlobalDeclare.bCrossHairEnable);
    }


    // 旋轉物件 (物件ID)
    public void ProcessRoMoving(int iIndex)
    {
        if (RO_OBJ[saveRotaObj] == null)
            return;

        Ro_Light.enabled = true;
        CameraVolume.enabled = true;
        isMoveingObject = true;  // 正在移動物件
        saveRotaObj = iIndex;   // 儲存物件  
        originalPosition = RO_OBJ[saveRotaObj].transform.position;  // 儲存物件位置
        originalRotation = RO_OBJ[saveRotaObj].transform.rotation;  // 儲存物件旋轉
        romanager = RO_OBJ[saveRotaObj].GetComponent<RotateObjDetect>().enabled = true; // 啟用旋轉物件碰撞器
    }

    // 顯示進入旋轉遊戲按鈕
    public void ShowObj(ObjItemID O_ItemID)
    {
        StudioUI.SetActive(true);

        switch (O_ItemID)
        {
            case ObjItemID.Lv1_Rice_Funeral:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(-28f, 1.85f, 8.32354f), 0.5f);
                break;
            case ObjItemID.Lv1_Lotus_Paper:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(-27.8f, 1.8f, 8.745541f), 0.5f);
                break;
            case ObjItemID.Lv1_Photo_Frame:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(-28f, 1.85f, 8.55254f), 0.5f);
                break;
            case ObjItemID.Lv2_Photo_Frame:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(-28f, 1.85f, 8.55254f), 0.5f);
                break;
            case ObjItemID.Lv2_Photo_Frame_Floor:
                RO_OBJ[saveRotaObj].transform.DOMove(
                    new Vector3(-27.762f, 1.801f, 8.55254f), 0.5f);
                break;
        }
    }

    // 旋轉物件UI畫面
    public void UIState(UIItemID r_ItemID, bool r_bEnable)
    {
        m_bInUIView = r_bEnable;
        playerCtrlr.m_bCanControl = !r_bEnable;
        playerCtrlr.SetCursor();

        goCanvas.SetActive(r_bEnable);
        ExitBtn.gameObject.SetActive(r_bEnable);
        imgUIBackGround.color = r_bEnable ? new Color(0, 0, 0, 0.60f) : new Color(0, 0, 0, 0.60f);
        imgInstructions.color = r_bEnable ? new Color(0, 0, 0, 1) : new Color(0, 0, 0, 0);
        int iItemID = (int)r_ItemID;

        txtTitle.text = GlobalDeclare.UITitle[iItemID];

        txtIntroduce.text = GlobalDeclare.UIIntroduce[iItemID];
        txtInstructions.text = GlobalDeclare.TxtInstructionsmage[iItemID];

        GegameManager_bInUIView();
    }

    // 執行物件動畫
    public void ProcessAnimator(string r_sObject, string r_sTriggerName)
    {
        if (r_sObject.Contains("null") || r_sTriggerName.Contains("null"))
            return;

        GameObject obj = GameObject.Find(r_sObject);
        Animator ani = obj.transform.GetComponent<Animator>();
        ani.SetTrigger(r_sTriggerName);

        if (obj.transform.GetComponent<ItemController>() != null)
        {
            obj.transform.GetComponent<ItemController>().SetHintable(false);
            obj.transform.GetComponent<ItemController>().bActive = false;
        }

        GlobalDeclare.SetItemAniObject("Empty");
        GlobalDeclare.SetItemAniName("Empty");
        m_bShowItemAnimate = false;
    }

    public void ProcessItemAnimator(string r_strObject, string r_strTriggerName)
    {
        if (r_strObject.Contains("null") || r_strTriggerName.Contains("null"))
            return;

        GameObject obj = GameObject.Find(r_strObject);
        Animator ani = obj.transform.GetComponent<Animator>();
        ani.SetTrigger(r_strTriggerName);
        m_bShowItemAnimate = false;
    }


    // 限制角色視角 (暫無使用)
    public void SetPlayerViewLimit(bool bLimitRotation, float[] fViewLimit)
    {
        m_bSetPlayerViewLimit = false;
        playerCtrlr.m_bLimitRotation = bLimitRotation;
        playerCtrlr.m_fHorizantalRotationRange.x = fViewLimit[0];
        playerCtrlr.m_fHorizantalRotationRange.y = fViewLimit[1];

        if (bLimitRotation)
        {
            playerCtrlr.tfTransform.localEulerAngles = Vector3.up * fViewLimit[2];
            Debug.Log("Value : " + fViewLimit[2]);
        }
    }

    // 顯示進入蓮花遊戲按鈕
    public void ShowEnterGame(bool r_bEnable)
    {
        bIsUIOpen = r_bEnable;
        EnterGameBtn.gameObject.SetActive(r_bEnable);
        txtEnterGameHint.gameObject.SetActive(r_bEnable);
        txtEnterGameHint.text = r_bEnable ? "按 *R* 開始摺紙 \r\n(Press *R* Origami Lotus Paper)" : "";
    }

    // 鍵盤檢查
    public virtual void KeyboardCheck()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 關閉 UI 畫面
            if (m_bInUIView)
            {
                if (isMoveingObject)
                {
                    romanager = false;

                    if (!romanager)
                    {
                        RestoreItemLocation();
                        Ro_Light.enabled = false;
                    }
                }
            }
            else
            {
                // 顯示遊戲狀態
                SetGameState();
            }
        }
    }

    public void RestoreItemLocation()
    {
        CameraVolume.enabled = false;
        romanager = RO_OBJ[saveRotaObj].GetComponent<RotateObjDetect>().enabled = false;
        print(RO_OBJ[saveRotaObj].transform.name);

        //恢復物件位置
        RO_OBJ[saveRotaObj].transform.DOMove(originalPosition, 0.1f);

        //恢復物件角度
        RO_OBJ[saveRotaObj].transform.DORotate(originalRotation.eulerAngles, 0.1f);
        isMoveingObject = false;
        StudioUI.SetActive(false);
    }

    public void MouseCheck()   // 滑鼠檢查MouseButtonDown(0)
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 在此處理滑鼠點擊事件
            // 可以使用 EventSystem 或 Raycasting 等方法進行 UI 按鈕的選擇處理
        }
    }

    public void SetGameState()  // 設定遊戲狀態
    {
        playerCtrlr.SetCursor();
        bIsPaused = !bIsPaused;
        Time.timeScale = bIsPaused ? 0f : 1f;
        settingObjects.SetActive(bIsPaused);
        bIsMouseEnabled = bIsPaused;
    }

    public void GameStateCheck()    // 檢查遊戲狀態
    {
        if (!GlobalDeclare.bLotusGameComplete &&
             m_bPlayLotusEnable &&
             currentScene.name == "2 Grandma House")
        {

        }
    }

    public bool GegameManager_bInUIView()    // 取得是否在 UI 畫面中
    {
        return m_bInUIView;
    }

    public void StopReadding()  // 停止閱讀查看物件
    {
        playerCtrlr.m_bCanControl = false;
        playerCtrlr.m_bLimitRotation = true;
        StartCoroutine(ChangeVignetteIntensity());
    }

    public IEnumerator ChangeVignetteIntensity()  // 改變電影模式Vignette強度
    {
        yield return new WaitForSeconds(11f);
        playerCtrlr.m_bCanControl = true;
        playerCtrlr.m_bLimitRotation = false;
        //VolumeProfile profile = postProcessVolume.sharedProfile;

        //if (profile.TryGet(out Vignette vignette) &&
        //    profile.TryGet(out CloudLayer cloudLayer))
        //{
        //    float currentIntensity = 0.64f;
        //    float elapsedTime = 0f;

        //    while (elapsedTime < 1f)
        //    {
        //        vignette.intensity.value = Mathf.Lerp(0.7669371f,
        //                                            targetIntensity, elapsedTime);
        //        vignette.smoothness.value = Mathf.Lerp(0.3474241f,
        //                                            targetIntensity, elapsedTime);
        //        vignette.roundness.value = Mathf.Lerp(0.3629616f,
        //                                            targetIntensity, elapsedTime);
        //        cloudLayer.opacity.value = Mathf.Lerp(currentIntensity,
        //                                            targetIntensity, elapsedTime);

        //        elapsedTime += Time.deltaTime * changeSpeed;
        //        yield return null;
        //    }
        //    yield return new WaitForSeconds(8f);
        //    vignette.intensity.value = 0.7f;
        //    vignette.smoothness.value = 0.16f;
        //    vignette.roundness.value = 0.18f;
        //    cloudLayer.opacity.value = 0.0f;
        //    playerCtrlr.m_bCanControl = true;
        //    playerCtrlr.m_bLimitRotation = false;
        //}
    }

    public void BackToBaseGame()
    {
        m_bReturnToBegin = false;
        QRCodeUI.SetActive(false);
        playerCtrlr.SetCursor();
        SceneManager.LoadScene(0);
    }

    public void SetCrosshairEnable(bool bEnable)
    {
        CrosshairUI.SetActive(bEnable);
    }

    public IEnumerator PlayerToAniPos(Vector3 r_V3TargetPos, Quaternion r_PlayerRotation, Quaternion r_CameraRotation)
    {
        playerCtrlr.m_bCanControl = false;
        playerCtrlr.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        playerCtrlr.gameObject.GetComponent<Rigidbody>().useGravity = false;

        // 移動玩家
        float fTotalMoveTime = 1.0f;
        float fCurrengameManageroveTime = 0.0f;

        while (fCurrengameManageroveTime < fTotalMoveTime)
        {
            playerCtrlr.transform.localPosition = Vector3.Lerp(playerCtrlr.transform.localPosition, r_V3TargetPos, fCurrengameManageroveTime / (fTotalMoveTime * 5f));
            playerCtrlr.transform.localRotation = Quaternion.Slerp(playerCtrlr.transform.localRotation, r_PlayerRotation, fCurrengameManageroveTime / (fTotalMoveTime * 5f));

            fCurrengameManageroveTime += Time.deltaTime;

            yield return null;
        }

        playerCtrlr.transform.localPosition = r_V3TargetPos;
        playerCtrlr.transform.localRotation = r_PlayerRotation;

        // 移動玩家 Camera
        float fTotalViewTime = 1.0f;
        float fCurrentViewTime = 0.0f;

        while (fCurrentViewTime < fTotalViewTime)
        {
            playerCtrlr.tfPlayerCamera.localRotation = Quaternion.Slerp(playerCtrlr.tfPlayerCamera.localRotation, r_CameraRotation, fCurrentViewTime / (fTotalViewTime * 5f));

            fCurrentViewTime += Time.deltaTime;

            yield return null;
        }

        playerCtrlr.tfPlayerCamera.localRotation = r_CameraRotation;
    }

    public void GameQuit()
    {
        Debug.Log("GameQuit");
        Application.Quit();
    }

    // 延遲動作
    public IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(2.5f);
    }
}