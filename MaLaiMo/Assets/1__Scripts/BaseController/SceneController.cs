using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

using DG.Tweening;
using System.Collections.Generic;

public partial class SceneController : MonoBehaviour
{
    #region < Field >
    [SerializeField] LevelTypeID CurrentLevel;
    [SerializeField] [Header("對話程序")] DialogueManager[] DialogueObjects;

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

    #region < ============ UI 相關 ============ >
    [Space(10)]
    [Header("============ UI 相關 ============")]
    [SerializeField] [Header("Item Canvas Handler")] 
    protected ItemCanvasHandler _itemCanvasHandler;

    [SerializeField] [Header("Item Canvas Group")]
    CanvasGroup _itemCanvasGroup;
    #endregion

    #region < ============ 攝影棚相關 ============ >
    [Space(10)]
    [Header("============ 攝影棚相關 ============")]

    [Header("物件 Raw Image"), Tooltip("掛 ItemCanvas 中的 ItemRawImage")]
    public GameObject _itemRawImage;

    [Header("攝影棚光線"), Tooltip("掛 *攝影棚_環境* 中的 Area Light")]
    public Light _itemRawImageLight;

    [Header("Item 物件池"), Tooltip("掛 *攝影棚_物件* 中的 所有的物件")]
    public GameObject[] _itemObjsForRawImage;

    [Header("玩家攝影機 Vulume"), Tooltip("掛 *LingLing* 中的 Player Camera")]
    [SerializeField] Volume _playerCameraVolume;

    [HideInInspector]
    public int _currentItemIndex;          //當前的 Item ID
    private Vector3 originalPosition;     // 原始位置
    private Quaternion originalRotation;  // 原始旋轉
    #endregion

    [Space(10)]
    [Header("============ 以下待整理 ============\n")]
    [Header("全域變數")] public Volume postProcessVolume;

    #region Static Boolean Zone
    public static bool[] paperMissionFinsih = new bool[] { false, false, false };
    public static bool takeLotus = false;

    public static bool m_bInUIView = false;
    public static bool m_bShowItemAnimate = false;
    public static bool m_bSetPlayerViewLimit = false;
    public static bool m_bReturnToBegin = false;
    public static bool m_bPlayLotusEnable = false;
    #endregion

    protected bool bIsPaused = false;
    protected bool bIsMouseEnabled = false;
    #endregion
    
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
        SetItemCanvasState(false);
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
        if (Input.GetKeyDown(KeyCode.F1))
        {
            // 關閉 UI 畫面
            if (m_bInUIView)
            {
                PlayerCtrlr._bCanControl = true;
                PlayerCtrlr.SetCursor();
                UIState((int)UIItemID.Empty, false);
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

    public virtual void UIState(int r_ItemID, bool r_bEnable, bool r_bNeedSubTitle = false)
    {
        m_bInUIView = r_bEnable;

        PlayerCtrlr.SetCursor();

        SetItemCanvasState(r_bEnable);

        if (r_bEnable)
        {
            ProcessItemMoving(r_ItemID);
        }
        else
        {
            RestoreItemLocation();
        }
    }

    public virtual void MoveItem(UIItemID r_ItemID) { }
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

    public void SetGameState()  // 設定遊戲狀態
    {
        PlayerCtrlr.SetCursor();
        bIsPaused = !bIsPaused;
        Time.timeScale = bIsPaused ? 0f : 1f;
        SettingPanel.SetActive(bIsPaused);
        bIsMouseEnabled = bIsPaused;
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

    public void SetItemCanvasState(bool r_bEnable)
    {
        this._itemRawImage.SetActive(r_bEnable);

        this._itemCanvasGroup.alpha = r_bEnable ? 1 : 0;
    }

    // 旋轉物件 (物件ID)
    public void ProcessItemMoving(int itemIndex)
    {
        if (_itemObjsForRawImage[itemIndex] == null)
            return;

        this._playerCameraVolume.enabled = true;
        this._itemRawImageLight.enabled = true;

        this._currentItemIndex = itemIndex;
        this.originalPosition = _itemObjsForRawImage[this._currentItemIndex].transform.position;  // 儲存物件位置
        this.originalRotation = _itemObjsForRawImage[this._currentItemIndex].transform.rotation;  // 儲存物件旋轉

        this.MoveItem((UIItemID)itemIndex);
    }

    public void RestoreItemLocation()
    {
        this._playerCameraVolume.enabled = false;
        this._itemRawImageLight.enabled = false;

        //恢復物件位置
        _itemObjsForRawImage[this._currentItemIndex].transform.DOMove(originalPosition, 0.1f);

        //恢復物件角度
        _itemObjsForRawImage[this._currentItemIndex].transform.DORotate(originalRotation.eulerAngles, 0.1f);
    }
    #endregion
}
