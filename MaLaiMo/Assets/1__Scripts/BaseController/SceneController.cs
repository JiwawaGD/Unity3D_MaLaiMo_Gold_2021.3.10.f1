using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

using DG.Tweening;

public partial class SceneController : MonoBehaviour
{
    #region < Field >
    // 確定需要保留的區域
    [SerializeField] LevelTypeID CurrentLevel;
    [SerializeField] [Header("對話程序")] DialogueManager[] DialogueObjects;

    [SerializeField] [Header("Item Canvas Handler")] ItemCanvasHandler _itemCanvasHandler;
    [SerializeField] [Header("Item Canvas Group")] CanvasGroup _itemCanvasGroup;
    [SerializeField] [Header("設定頁面")] GameObject SettingPanel;
    [SerializeField] [Header("UI - 準心")] GameObject CrosshairUI;

    [SerializeField] [Header("轉場黑色過場圖片")] public Image _transitBlackImg;

    /// <summary>
    /// 角色控制器
    /// </summary>
    [HideInInspector] public PlayerController PlayerCtrlr;

    /// <summary>
    /// 音效控制器
    /// </summary>
    [HideInInspector] protected AUDManager AudManager;

    protected Scene CurrentScene;

    [Header("============ 以下待整理 ============\n")]

    [Space]
    [SerializeField] Volume CameraVolume;

    [Space]
    [Header("物件旋轉參數設定")]
    protected bool isMoveingObject = false;    // 是否正在移動物件
    protected Vector3 originalPosition;    // 原始位置
    protected Quaternion originalRotation; // 原始旋轉

    //[Header("物件移動速度")] public float objSpeed;
    [Header("旋轉物件功能")] public bool romanager;
    [Header("全域變數")] public Volume postProcessVolume;
    [Header("物件位置")] public GameObject itemObjTransform;
    [Header("生成後物件")] public GameObject[] RO_OBJ;
    [Header("儲存生成物件")] public int saveRotaObj;
    [Header("攝影棚畫面UI")] public GameObject StudioUI;
    [Header("旋轉物件使用燈關")] public Light Ro_Light;

    #region Static Boolean Zone
    public static bool m_bInUIView = false;
    public static bool m_bShowItemAnimate = false;
    public static bool m_bSetPlayerViewLimit = false;
    public static bool m_bReturnToBegin = false;
    public static bool m_bPlayLotusEnable = false;
    #endregion

    protected bool bIsPaused = false;
    protected bool bIsMouseEnabled = false;
    #endregion

    // 以上未還未整理的程式碼

    #region < Unity Hook >
    public virtual void Awake()
    {
        CurrentScene = SceneManager.GetActiveScene();   // 當前場景

        if (PlayerCtrlr == null)
            PlayerCtrlr = GameObject.Find("_Common_Player/LingLing").GetComponent<PlayerController>();

        AudManager = PlayerCtrlr.GetComponentInChildren<AUDManager>();

        if (_itemCanvasHandler == null)
            _itemCanvasHandler = GameObject.Find("_Common_Canvas/_Item Canvas").GetComponent<ItemCanvasHandler>();

        if (_itemCanvasGroup == null)
            _itemCanvasGroup = GameObject.Find("_Common_Canvas/_Item Canvas").GetComponent<CanvasGroup>();
    }

    public virtual void Start()
    {
        // 預設開啟遊戲準心
        SetCrosshairEnable(true);

        // 預設關閉 Canvas Group
        SetItemCanvasEnable(false);
    }

    public virtual void Update()
    {
        KeyboardCheck();

        if (bIsPaused && bIsMouseEnabled)
            MouseCheck();
    }
    #endregion

    #region < External Virtual >

    /// <summary>
    /// 鍵盤偵測
    /// </summary>
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

    public virtual void ShowHint(LevelTypeID r_SceneTypeID, HintItemID r_ItemID)
    {
        Debug.Log(string.Format("[SHOW HINT] <color=lime><b>{0}</b></color>  Item Active in Scene : {1}", r_ItemID, r_SceneTypeID));
    }

    public virtual void GameEvent(LevelTypeID r_SceneTypeID, GameEventID r_EventID)
    {
        Debug.Log(string.Format("[GAME EVENT] <color=cyan><b>{0}</b></color> Trigger in Scene : {1}", r_EventID, r_SceneTypeID));
    }

    public virtual void TransitFadeOut()
    {
        this._transitBlackImg.color = new Color(0, 0, 0, 255f);

        this._transitBlackImg.DOFade(0, 1f)
                             .OnComplete(() => SetPlayerControl(true));
    }

    public virtual void SetPlayerLocation(Vector3 location)
    {
        this.PlayerCtrlr.transform.localPosition = location;
    }

    public virtual void SetPlayerControl(bool r_bEnable)
    {
        PlayerCtrlr._bCanControl = r_bEnable;
    }
    #endregion

    #region < Base Function >
    public void ProcessPlayerAnimator(string r_sAnimationName)
    {
        Transform tfPlayer = GameObject.Find("_Common_Player/LingLing").transform;
        Animation Am = tfPlayer.GetComponent<Animation>();

        Am.PlayQueued(r_sAnimationName);
        GlobalDeclare.SetPlayerAnimateType(PlayerAnimateType.Empty);
    }

    /// <summary>
    /// 執行物件動畫
    /// </summary>
    /// <param name="r_sObject">動畫物件</param>
    /// <param name="r_sTriggerName">動畫名稱</param>
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

    /// <summary>
    /// 執行 DialogueManager 的事件
    /// </summary>
    /// <param name="index">事件 ID</param>
    public void PlayDialogue(int index)
    {
        StartCoroutine(DialogueObjects[index].StartAction());
    }
    #endregion

    #region < 還不確定要不要保留的程式 >
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

    /// <summary>
    /// 顯示進入旋轉按鈕
    /// </summary>
    /// <param name="O_ItemID"></param>
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

    public void ProcessItemAnimator(string r_strObject, string r_strTriggerName)
    {
        if (r_strObject.Contains("null") || r_strTriggerName.Contains("null"))
            return;

        GameObject obj = GameObject.Find(r_strObject);
        Animator ani = obj.transform.GetComponent<Animator>();
        ani.SetTrigger(r_strTriggerName);
        m_bShowItemAnimate = false;
    }

    /// <summary>
    /// 限制角色視角 (暫無使用)
    /// </summary>
    /// <param name="bLimitRotation"></param>
    /// <param name="fViewLimit"></param>
    public void SetPlayerViewLimit(bool bLimitRotation, float[] fViewLimit)
    {
        m_bSetPlayerViewLimit = false;
        PlayerCtrlr.m_bLimitRotation = bLimitRotation;
        PlayerCtrlr.m_fHorizantalRotationRange.x = fViewLimit[0];
        PlayerCtrlr.m_fHorizantalRotationRange.y = fViewLimit[1];

        if (bLimitRotation)
        {
            PlayerCtrlr.tfTransform.localEulerAngles = Vector3.up * fViewLimit[2];
            Debug.Log("Value : " + fViewLimit[2]);
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

    public void SetGameState()  // 設定遊戲狀態
    {
        PlayerCtrlr.SetCursor();
        bIsPaused = !bIsPaused;
        Time.timeScale = bIsPaused ? 0f : 1f;
        SettingPanel.SetActive(bIsPaused);
        bIsMouseEnabled = bIsPaused;
    }

    public void GameStateCheck()    // 檢查遊戲狀態
    {
        if (!GlobalDeclare.bLotusGameComplete &&
             m_bPlayLotusEnable &&
             CurrentScene.name == "2 Grandma House")
        {

        }
    }

    public bool GegameManager_bInUIView()    // 取得是否在 UI 畫面中
    {
        return m_bInUIView;
    }

    public void StopReadding()  // 停止閱讀查看物件
    {
        PlayerCtrlr._bCanControl = false;
        PlayerCtrlr.m_bLimitRotation = true;
        StartCoroutine(ChangeVignetteIntensity());
    }

    public IEnumerator ChangeVignetteIntensity()  // 改變電影模式Vignette強度
    {
        yield return new WaitForSeconds(11f);
        PlayerCtrlr._bCanControl = true;
        PlayerCtrlr.m_bLimitRotation = false;
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
        //QRCodeUI.SetActive(false);
        PlayerCtrlr.SetCursor();
        SceneManager.LoadScene(0);
    }

    public IEnumerator PlayerToAniPos(Vector3 r_V3TargetPos, Quaternion r_PlayerRotation, Quaternion r_CameraRotation)
    {
        PlayerCtrlr._bCanControl = false;
        PlayerCtrlr.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        PlayerCtrlr.gameObject.GetComponent<Rigidbody>().useGravity = false;

        // 移動玩家
        float fTotalMoveTime = 1.0f;
        float fCurrengameManageroveTime = 0.0f;

        while (fCurrengameManageroveTime < fTotalMoveTime)
        {
            PlayerCtrlr.transform.localPosition = Vector3.Lerp(PlayerCtrlr.transform.localPosition, r_V3TargetPos, fCurrengameManageroveTime / (fTotalMoveTime * 5f));
            PlayerCtrlr.transform.localRotation = Quaternion.Slerp(PlayerCtrlr.transform.localRotation, r_PlayerRotation, fCurrengameManageroveTime / (fTotalMoveTime * 5f));

            fCurrengameManageroveTime += Time.deltaTime;

            yield return null;
        }

        PlayerCtrlr.transform.localPosition = r_V3TargetPos;
        PlayerCtrlr.transform.localRotation = r_PlayerRotation;

        // 移動玩家 Camera
        float fTotalViewTime = 1.0f;
        float fCurrentViewTime = 0.0f;

        while (fCurrentViewTime < fTotalViewTime)
        {
            PlayerCtrlr.tfPlayerCamera.localRotation = Quaternion.Slerp(PlayerCtrlr.tfPlayerCamera.localRotation, r_CameraRotation, fCurrentViewTime / (fTotalViewTime * 5f));

            fCurrentViewTime += Time.deltaTime;

            yield return null;
        }

        PlayerCtrlr.tfPlayerCamera.localRotation = r_CameraRotation;
    }

    public void GameQuit()
    {
        Debug.Log("GameQuit");
        Application.Quit();
    }

    /// <summary>
    /// 執行玩家移動到指定區域
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public IEnumerator ProcessPlayerSetPianoAni(int index)
    {
        //bIsPlayingPiano = true;
        //Transform tfPianoPos = GameObject.Find("PianoTarget").GetComponent<Transform>();
        //Transform tfCameraPos = tfPianoPos.GetChild(0);

        //yield return StartCoroutine(PlayerToAniPos(Targers[index].position, tfPianoPos.rotation, tfCameraPos.rotation));
        yield return null;

        //if (bIsPlayingPiano == true)
        //    PianoUI.SetActive(true);
    }

    /// <summary>
    /// 延遲動作
    /// </summary>
    /// <returns></returns>
    public IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(2.5f);
    }
    #endregion

    #region < 有使用到的 Method >
    /// <summary>
    /// Item Canvas State
    /// </summary>
    /// <param name="r_ItemID"> Item 的 ID</param>
    /// <param name="r_bEnable"> 開關狀態</param>
    /// <param name="r_bNeedSubTitle"> 是否打開下方小 Info</param>
    public void UIState(UIItemID r_ItemID, bool r_bEnable, bool r_bNeedSubTitle = false)
    {
        int iItemID = (int)r_ItemID;

        SetItemCanvasEnable(r_bEnable);

        m_bInUIView = r_bEnable;
        PlayerCtrlr.SetCursor();

        _itemCanvasHandler._txtTopTitle.text = GlobalDeclare.item_Title[iItemID];
        _itemCanvasHandler._txtMainInfo.text = GlobalDeclare.item_MainInfo[iItemID];
        _itemCanvasHandler._txtBottonInfo.text = r_bNeedSubTitle ? GlobalDeclare.item_BottonInfo[iItemID] : "";
    }

    /// <summary>
    /// 滑鼠檢查MouseButtonDown(0)
    /// </summary>
    public void MouseCheck()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        // 在此處理滑鼠點擊事件
        //}
    }

    public void SetCrosshairEnable(bool bEnable)
    {
        CrosshairUI.SetActive(bEnable);
    }

    public void SetItemCanvasEnable(bool r_bEnable)
    {
        _itemCanvasGroup.alpha = r_bEnable ? 1 : 0;
    }
    #endregion
}