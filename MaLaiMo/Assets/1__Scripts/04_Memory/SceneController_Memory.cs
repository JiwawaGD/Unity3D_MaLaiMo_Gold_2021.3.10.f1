using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;
using System;
using TMPro;
using UnityEngine.UIElements;

public class SceneController_Memory : SceneController
{
    #region < Property >
    [Space(10)]
    [Header("============ By Scene 各場景使用欄位 ============")]
    [Header("物件池")] public ObjectController_Room _objectCtrlr;

    [Header("室外傳至室內的角色座標")] public Transform _outsideGoInTransitPos;
    [Header("蓮花遊戲控制器")] public LotusGameManager _lotusGameManager;

    [Header("電視")] public GameObject _tvObject;
    [Header("電視雜訊的材質球")] public Material _tvNoiseMaterial;
    [Header("孝濂動畫")] public Animator FilialPietyCurtain_Ani;
    [Header("媽媽控制器")] public Mom_Controler_Room Mom_Control;
    [Header("女鬼頭部目標")] public Transform ghostHeadTarget;
    [Header("限時任務設定")]
    [Header("玩家要跑向的目標點")]public Transform escapeTarget;
    [Header("限制時間")]public float taskTimeLimit = 20f;
    [Header("判斷到達的距離")] public float arrivalDistance = 1.5f;
    [Header("任務是否進行中")] private bool isTaskActive = false;
    [Header("房間門")]
    public Transform badroomDoor;
    public GrandmaRoom_Player Player;
    public GameObject LotusPaper;
    private static bool FilialPietyCurtain_IsOpen = false;
    private static bool FirstE_Amulet = false;
    private static bool canSwitchDoor = true;
    public GameObject calendarObject;
    private Animation calendarAnim;
    private int BathRoomDoorStep = 0;
    public GameObject FemaleGhost;

    public GameObject PSbloodrunning;
    public GameObject BloodWater;

    private bool test4_3 = false;
    #endregion

    #region < ByScene Flag >
    private bool _hasTriggerGraffiti = false;
    #endregion

    #region < Unity Hook >
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        calendarAnim = calendarObject.GetComponent<Animation>();
        if(test4_3)
        {
            // 設定玩家傳送座標
            SetPlayerLocation(new Vector3(-8.894f, 1.607f, -14.418f));
            Lv4_StartPlayingGetAmulet();
        }
        else Player._bCanControl = true;
        // 非第一次進場場景 > 轉場圖片 Fade Out
        //TransitFadeOut();
    }

    public override void Update()
    {
        base.Update();
        // 偵測 Ctrl+P
        if (Input.GetKey(KeyCode.O) && Input.GetKeyDown(KeyCode.P))
        {
            ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv4_GiftBox);
            Lv4_Restroom_MovePlayerToMousePosition();
        }
    }
    #endregion

    #region < Override Function >
    public override void GameEvent(LevelTypeID r_SceneTypeID, GameEventID r_EventID)
    {
        base.GameEvent(r_SceneTypeID, r_EventID);

        switch (r_SceneTypeID)
        {
            case LevelTypeID.Lv1_GrandmaHouse:
            case LevelTypeID.Lv3_GrandmaHouse_Memory:
                Lv1_Event(r_EventID);
                break;

            default:
                Debug.LogError(string.Format("[SceneCtrlr - Room] Error SceneTypeID : {0}", r_SceneTypeID));
                break;
        }
    }

    public override void KeyboardCheck()
    {
        // 正在摺蓮花中 Scene Controller 暫停 Update
        if (GlobalDeclare._playingLotusGame)
            return;

        base.KeyboardCheck();

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (GlobalDeclare._waitingPlayLotusPaper)
            {
                GlobalDeclare._playingLotusGame = true;
                UIState((int)UIItemID.Empty, false);
                this._lotusGameManager.SetHintPosition();
            }
        }
    }

    public override void ShowHint(LevelTypeID r_SceneTypeID, HintItemID r_ItemID)
    {
        base.ShowHint(r_SceneTypeID, r_ItemID);

        try
        {
            ItemController NextItem = null;

            switch (r_SceneTypeID)
            {
                case LevelTypeID.Lv1_GrandmaHouse:
                    string itemName = "";

                    switch (r_ItemID)
                    {
                        case HintItemID.Lv4_001_E_Calendar:
                            itemName = "_Scene02_InteractItems/Lv4_Calendar";
                            break;
                        case HintItemID.Lv4_Calendar_paper_anim:
                            itemName = "_Scene02_InteractItems/Lv4_Calendar_paper_anim";
                            break;
                        case HintItemID.Lv4_GiftBox:
                            itemName = "_Scene02_InteractItems/giftbox and bear";
                            break;
                        case HintItemID.Lv1_Item_OpenRoomDoor:
                            itemName = "_Scene01_InteractItems/Lv1_Door/Lv1_Grandma_Room_Door";
                            break;
                        case HintItemID.Lv1_Item_LotusPaper:
                            itemName = "_Scene01_InteractItems/__Level_1/Lv1_Lotus_Handler";
                            break;
                        case HintItemID.Lv1_Item_Piano:
                            itemName = "_Scene01_InteractItems/__Level_1_TODO/Lv1_Piano";
                            break;
                        case HintItemID.Lv1_Item_FoldedLotusPaper:
                            itemName = "_Scene01_InteractItems/__Level_1/Lv1_Finished_Lotus_Paper";
                            break;
                        case HintItemID.Lv1_Item_OfferingPlace:
                            itemName = "_Scene01_InteractItems/__Level_1/Item_PlaceToPutRice";
                            break;
                        case HintItemID.Lv1_Item_GrandmaRoomCloset:
                            itemName = "_Scene01_InteractItems/__Level_1/Lv1_Wardrobe";
                            break;
                        case HintItemID.Lv1_Item_ClothesInCloset:
                            itemName = "_Scene01_InteractItems/__Level_1/Lv1_5Clothes";
                            break;
                        case HintItemID.Lv1_Item_Grafitti:
                            itemName = "_Scene01_InteractItems/__Level_1/Lv1_Crayon";
                            break;
                        case HintItemID.Lv4_Item_RoomDoorClock:
                            itemName = "_Scene01_InteractItems/Lv1_Door/Lv1_Grandma_Room_Door";
                            break;
                        case HintItemID.Lv4_Item_RoomDoorOpen:
                            itemName = "_Scene01_InteractItems/Lv1_Door/Lv1_Grandma_Room_Door";
                            // 取得門物件
                            var doorObj = GameObject.Find(itemName);
                            if (doorObj != null)
                            {
                                var ani = doorObj.GetComponent<Animation>();
                                if (ani != null)
                                {
                                    ani.PlayQueued("Door_Open");
                                }
                            }
                            break;

                        case HintItemID.Lv4_Toilet:
                            itemName = "_Scene02_InteractItems/Lv4_Toilet";
                            break;
                        //case HintItemID.Lv4_Skin:
                        //    itemName = "_Scene02_InteractItems/Lv4_Handsink";
                        //    break;
                        case HintItemID.Lv4_Dam_MomPupptery:
                            itemName = "_Scene02_InteractItems/Lv4_Dam_MomPupptery";
                            break;
                        case HintItemID.Lv4_Bathtub_null:
                            itemName = "_Scene02_InteractItems/Lv4_Bathtub_null";
                            break;
                        case HintItemID.Lv4_Bathtub:
                            itemName = "_Scene02_InteractItems/Lv4_Bathtub";
                            break;
                        case HintItemID.Lv4_blood_water:
                            itemName = "_Scene02_InteractItems/Lv4_blood_water";
                            break;
                        default:
                            Debug.LogError(string.Format("[ERROR] [ShowHint] [Lv1_GrandmaHouse] Error Item ID :: {0}", r_ItemID));
                            break;
                    }


                    NextItem = GameObject.Find(itemName).GetComponent<ItemController>();
                    break;
                default:
                    Debug.LogError(string.Format("[ERROR] [ShowHint] [Lv1_GrandmaHouse] Error Scene ID :: {0}", r_SceneTypeID));
                    break;
            }

            NextItem.bActive = true;
            NextItem.SetHintable(true);
        }
        catch (System.Exception exception)
        {
            Debug.LogError(string.Format("[ERROR] Show Hint <{0}> Error :: {1}", r_ItemID, exception.Message));
            throw;
        }
    }

    public override void TransitFadeOut()
    {
        base.TransitFadeOut();
    }

    public override void SetPlayerLocation(Vector3 location)
    {
        base.SetPlayerLocation(location);
    }

    public override void UIState(int r_ItemID, bool r_bEnable, bool r_bNeedSubTitle = false)
    {
        base.UIState(r_ItemID, r_bEnable, r_bNeedSubTitle);

        _itemCanvasHandler._txtTopTitle.text = r_bEnable ? GlobalDeclare.item_Title_Room[r_ItemID] : "";
        _itemCanvasHandler._txtMainInfo.text = r_bEnable ? GlobalDeclare.item_MainInfo_Room[r_ItemID] : "";
        _itemCanvasHandler._txtBottonInfo.text = r_bEnable ? (r_bNeedSubTitle ? GlobalDeclare.item_BottonInfo_Room[r_ItemID] : "") : "";
    }

    public override void MoveItem(UIItemID r_ItemID)
    {
        // *TODO* 因為攝影棚在 -50 的位置，因此 Y 需要設定 -50 (待優化)
        switch (r_ItemID)
        {
            case UIItemID.Lv1_UI_LotusPaper:
                this._itemObjsForRawImage[this._currentItemIndex].transform.DOMove(
                    new Vector3(0f, -50f, 0f), 0.5f);
                break;
        }
    }

    public override void ChangeItemGameEventID(ItemController item, GameEventID newGameEventID)
    {
        base.ChangeItemGameEventID(item, newGameEventID);
    }

    public override void SetItemAlwaysActive(ItemController item, bool alwaysActive)
    {
        base.SetItemAlwaysActive(item, alwaysActive);
    }
    #endregion

    #region < API >
    // Call From Dialogue System 
    public override void SetPlayerControl(bool r_bEnable)
    {
        base.SetPlayerControl(r_bEnable);
    }
    /// <summary>
    /// 設定日曆動畫播放
    /// </summary>
    public void Lv4_Event_SetCalender()
    {
        if (calendarAnim == null)
        {
            Debug.LogError("[Lv4_Event_SetCalender] calendarAnim 是 null，請確認 calendarObject 是否正確指派並含有 Animation 元件！");
            return;
        }

        if (!calendarAnim.GetClip("calendar_paper_falling_down"))
        {
            Debug.LogError("[Lv4_Event_SetCalender] 找不到動畫片段：calendar_paper_falling_down");
            return;
        }

        calendarAnim["calendar_paper_falling_down"].time = 0f;
        calendarAnim.PlayQueued("calendar_paper_falling_down");
    }

    public void SetTVNoise()
    {
        MeshRenderer tvRender = this._tvObject.transform.Find("Screen").GetComponent<MeshRenderer>();
        tvRender.material = this._tvNoiseMaterial;
    }

    public void LotusGameFinish()
    {
        this._lotusGameManager.enabled = false;

        Vector3 playerLocation = new(-3.2f, 0.68f, -1.8f);
        Vector3 playerRotation = new(0f, 270f, 0f);
        Vector3 cameraRotation = new(-47f, 0f, 0f);

        PlayerCtrlr.SetToTargetLocation(playerLocation, playerRotation, cameraRotation);

        PlayerCtrlr._bCanControl = true;
        PlayerCtrlr._rig.useGravity = true;
        PlayerCtrlr._collider.enabled = true;

        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_FoldedLotusPaper);
    }
    #endregion

    #region < Basic Function >
    void Lv1_Event(GameEventID r_EventID)
    {
        try
        {
            switch (r_EventID)
            {
                case GameEventID.Lv4_GrandmaRoomDoorSwitchClock:
                    Lv4_GrandmaRoomDoorSwitch_Calendar();
                    break;
                case GameEventID.Lv4_007_Calendar:
                    Lv4_Event_SetCalender();
                    break;
                case GameEventID.Lv4_Calendar_paper_anim:
                    Lv4_TearCalendarDialogue();
                    break;
                case GameEventID.Lv4_FlushToilet:
                    Lv4_FlushToilet();
                    break;
                case GameEventID.Lv4_E_Sink:
                    //洗手台
                    //Lv4_E_Skin();
                    break;
                case GameEventID.Lv4_E_BathRoomDoor:
                    StartCoroutine(Lv4_E_BathRoomDoor());
                    break;
                case GameEventID.Lv4_E_GrandMaRoomDoor:
                    Lv4_E_GrandMaRoomDoor();
                    break;
                case GameEventID.Lv4_GiftBox:
                    Lv4_PlayGiftBoxAnimation();
                    break;
                case GameEventID.Lv4_Dam_MomPupptery:
                    Lv4_Room_MovePlayerToMousePosition();
                    break;
                case GameEventID.Lv4_E_FaucetSwitch:
                    StartCoroutine(PlayTubAnim());
                    break;
                case GameEventID.Lv4_E_Tub:
                    StartCoroutine(PlayTubAnim());
                    break;
                case GameEventID.Lv4_Bathtub_null:
                    Lv4_Bathtub_null();
                    break;
                case GameEventID.Lv4_Bathtub:
                    Lv4_Bathtub();
                    break;
                case GameEventID.Lv4_blood_water:
                    Bathtub_blood_search();
                    break;
                case GameEventID.Lv4_Item_RoomDoorOpen:
                    Transform tfRoomDoor = GameObject.Find("_Scene01_InteractItems/__Level_1/Lv1_Door/Lv1_Grandma_Room_Door").transform;
                    Animation AniRoomDoor = tfRoomDoor.GetComponent<Animation>();
                    AniRoomDoor.PlayQueued("Door_Open");
                    break;
                case GameEventID.LV4_E_Telephone:
                    Lv4_E_Telephone();
                    break;
                case GameEventID.LV4_E_Piano:
                    Lv4_E_Piano();
                    break;
                case GameEventID.LV4_E_Flower:
                    Lv4_E_Piano();
                    break;
                case GameEventID.Lv4_E_Amulet:
                    Lv4_E_Amulet();
                    break;
                case GameEventID.Lv4_E_Bed:
                    Lv4_E_Bed();
                    break;
                case GameEventID.Lv4_E_CalendarPaper:
                    Lv4_E_CalendarPaper();
                    break;
                case GameEventID.Lv4_E_GrandmaDoor:
                    Lv4_E_GrandmaDoor();
                    break;
                default:
                    Debug.LogError(string.Format("<color=red><b>[Error]</b></color> [Lv1_Event] Error Event ID :: {0}", r_EventID));
                    break;
            }
        }
        catch (System.Exception exception)
        {
            Debug.LogError(string.Format("[<color=red><b>Error</b></color>] [Lv1_Event] Event  <color=red><b>{0}</b></color>  Error  ::  {1}", r_EventID, exception.Message));
            throw;
        }
    }
    #endregion

    #region < Game Event >
    void Lv4_GrandmaRoomDoorSwitch()
    {
        Transform tfRoomDoor = GameObject.Find("_Scene01_InteractItems/Lv1_Door/Lv1_Grandma_Room_Door").transform;
        Animation AniRoomDoor = tfRoomDoor.GetComponent<Animation>();

        if (tfRoomDoor.localRotation.z > 0.49 || tfRoomDoor.localRotation.z == 0)
        {
            string strPlayAniName = tfRoomDoor.localRotation.z == 0 ? "Door_Open" : "Door_Close";

            AniRoomDoor[strPlayAniName].time = 0f;
            AniRoomDoor.PlayQueued(strPlayAniName);
            //Lv4_Event_SetCalender();
        }
    }
    void Lv4_GrandmaRoomDoorSwitch_Calendar()
    {

        Lv4_Event_SetCalender();
        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv4_Calendar_paper_anim);
    }

    void Lv1_GoOutSide()
    {
        this._transitBlackImg.DOFade(1f, 1)
                             .OnComplete(() => SceneManager.LoadScene(GlobalDeclare.Lv2_Grandma_OutSide));
    }

    void Lv1_LotusPaperCheck()
    {
        Vector3 v3TargetPos = new(-3.3f, 0.35f, -1.9f);
        Vector3 v3PlayerEndRotation = new(0f, 0f, 0f);
        Vector3 v3PlayerCamEndRotation = new(25f, 275f, 0f);

        PlayerCtrlr.MoveToTargetPosition(v3TargetPos, v3PlayerEndRotation, v3PlayerCamEndRotation, 2f, () =>
        {
            GlobalDeclare._waitingPlayLotusPaper = true;
            UIState((int)UIItemID.Lv1_UI_LotusPaper, true, true);
            this._lotusGameManager.enabled = true;
            this._lotusGameManager.SetPaperLocation();
        });
    }

    void Lv4_Restroom_MovePlayerToMousePosition()
    {
        // 移動玩家
        if (Player != null)
        {
            Player.transform.position = new Vector3(-3.82f, 0.8f, -1.029f);
            Player.transform.eulerAngles = new Vector3(0f, 360f, 0f);
        }
    }
    void Lv4_Room_MovePlayerToMousePosition()
    {
        StartCoroutine(MovePlayerAfterDelayToRoom());
    }
    void Lv4_TearCalendarDialogue()
    {
        // 撕下日曆紙對話內容
        PlayDialogue((int)Room_Dialogue.Lv4_002_DoorOpen);
        StartCoroutine(MovePlayerAfterDelay());

    }
    IEnumerator MovePlayerAfterDelay()
    {
        Lv4_GrandmaRoomDoorSwitch();
        // 先延遲2秒
        yield return new WaitForSeconds(2f);
        // 黑幕淡入（1秒）
        if (_transitBlackImg != null)
        {
            _transitBlackImg.DOFade(1f, 1f);
            yield return new WaitForSeconds(1f);
        }

        // 等待2秒
        yield return new WaitForSeconds(3f);

        // 移動玩家
        if (Player != null)
        {
            Player.transform.position = new Vector3(-9.696f, 0.8f, 7.158f);
            Player.transform.eulerAngles = new Vector3(0f, 360f, 0f);
            ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv4_Toilet);
        }

        // 黑幕淡出（1秒）
        if (_transitBlackImg != null)
        {
            _transitBlackImg.DOFade(0f, 1f);
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator MovePlayerAfterDelayToRoom()
    {
        Lv4_GrandmaRoomDoorSwitch();
        // 先延遲2秒
        yield return new WaitForSeconds(2f);
        // 黑幕淡入（1秒）
        if (_transitBlackImg != null)
        {
            _transitBlackImg.DOFade(1f, 1f);
            yield return new WaitForSeconds(1f);
        }

        // 等待2秒
        yield return new WaitForSeconds(3f);

        // 移動玩家
        if (Player != null)
        {
            Player.transform.position = new Vector3(0f, 0f, 0f);
            Player.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }

        // 黑幕淡出（1秒）
        if (_transitBlackImg != null)
        {
            _transitBlackImg.DOFade(0f, 1f);
            yield return new WaitForSeconds(1f);
        }
    }

    void Lv4_FlushToilet()
    {
        //閃爍
        PlayDialogue((int)Room_Dialogue.Lv4_004_DropIntoWaterSound);
        print("馬桶沖水聲");
        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv4_Bathtub_null);
    }

    //void Lv4_E_Skin()
    //{
    //    print("洗手台");
    //    ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv4_Bathtub);
    //    // 啟用粒子效果和單純物件
    //    if (PSbloodrunning != null) PSbloodrunning.SetActive(true);
    //    if (blood_water != null) blood_water.SetActive(true);
    //    StartCoroutine(RotatePlayerToTarget("Scene02_InteractItems/Lv4_Bathtub", 1.5f));
    //}
    IEnumerator RotatePlayerToTarget(string targetObjectName, float duration)
    {
        if (Player == null) yield break;

        // 禁止玩家操作
        Player._bCanControl = false;

        GameObject targetObj = GameObject.Find("_" + targetObjectName);
        if (targetObj == null)
        {
            Debug.LogError("找不到目標物件：" + targetObjectName);
            Player._bCanControl = true;
            yield break;
        }

        Vector3 targetPos = targetObj.transform.position;
        Vector3 playerPos = Player.transform.position;
        Vector3 direction = (targetPos - playerPos).normalized;

        Quaternion startRot = Player.transform.rotation;
        Quaternion endRot = Quaternion.LookRotation(direction, Vector3.up);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            Player.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        // 最終角度修正
        Player.transform.rotation = endRot;

        // 恢復玩家操作
        Player._bCanControl = true;
    }
    void Lv4_Bathtub_null()
    {
        print("水沒有流出來");
        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv4_Bathtub);
    }
    void Lv4_Bathtub()
    {
        print("浴缸動畫");
        PlayDialogue((int)Room_Dialogue.Lv4_007_TearCalendar);
        // 關閉房門
        Transform tfRoomDoor = GameObject.Find("_Scene01_InteractItems/Lv1_Door/Lv1_Grandma_Room_Door").transform;
        Animation AniRoomDoor = tfRoomDoor.GetComponent<Animation>();
        AniRoomDoor.PlayQueued("Door_Close");
        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv4_Item_RoomDoorOpen);
        // 啟用粒子效果和單純物件
        if (PSbloodrunning != null) PSbloodrunning.SetActive(true);
        if (BloodWater != null)
        {
            BloodWater.SetActive(true);
            // 設定起始 Y 位置
            Vector3 pos = BloodWater.transform.position;
            pos.y = 0.245f;
            BloodWater.transform.position = pos;
            // 使用 DOTween 讓 Y 緩慢升高到 0.855
            BloodWater.transform.DOMoveY(0.855f, .1f);
            ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv4_blood_water);
        }
        StartCoroutine(RotatePlayerToTarget("Scene02_InteractItems/Lv4_Bathtub", 1.5f));
        //// 回到玩家初始進入位置
        //Player.transform.position = new Vector3(-7.5f, 0.65f, -13.1f);
        //Player.transform.eulerAngles = new Vector3(0f, 258.332f, 0f);
        //ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv4_GiftBox);
        //Lv4_Restroom_MovePlayerToMousePosition();
    }
    public void Bathtub_blood_search()
    {
        if (FemaleGhost != null)
        {
            FemaleGhost.gameObject.SetActive(true);
            // 【優化】提早啟動注視：在 6 秒等待一開始就鎖定視角
            // 這裡設定總時間為：原本的 6 秒等待 + 上升 1 秒 + 停留 1 秒 = 8 秒
            StartCoroutine(LockViewOnGhostHead(ghostHeadTarget, 8f));
            StartCoroutine(Lv4_Delayed_Rise_Of_The_Female_Ghost());
        }
    }

    IEnumerator Lv4_Delayed_Rise_Of_The_Female_Ghost()
    {
        // 1. 原本的 6 秒等待
        yield return new WaitForSeconds(6f);

        // 2. 設定起始位置
        Vector3 pos = FemaleGhost.transform.position;
        pos.y = -1.091f;
        FemaleGhost.transform.position = pos;

        // 3. 執行上升動畫
        FemaleGhost.transform.DOMoveY(0.866f, 1f);

        // 4. 【新增】啟動注視目標物 5 秒的邏輯
        // 這裡我們直接呼叫你寫好的 LockViewOnGhostHead
        yield return StartCoroutine(LockViewOnGhostHead(ghostHeadTarget, 5f));
    }
    private IEnumerator LockViewOnGhostHead(Transform target, float duration)
    {
        if (target == null) yield break;

        Player._bCanControl = false;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector3 direction = target.position - Player.tfPlayerCamera.position;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Player.tfPlayerCamera.rotation = Quaternion.Slerp(Player.tfPlayerCamera.rotation, targetRotation, Time.deltaTime * 3f);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 平滑恢復控制 (假設你已加入 Sync 邏輯)
        // SyncPlayerRotationWithCamera(); 
        Player._bCanControl = true;

        // 【核心改動】恢復控制後，立即開始限時任務
        StartCoroutine(StartEscapeTask());
    }
    IEnumerator StartEscapeTask()
    {
        Debug.Log("任務開始：請在 20 秒內抵達目標！");
        isTaskActive = true;
        float timer = taskTimeLimit;
        bool reached = false;

        while (timer > 0)
        {
            // 1. 計算玩家與目標的距離 (只計算水平距離 X, Z，避免高度差影響)
            float distance = Vector3.Distance(
                new Vector3(Player.transform.position.x, 0, Player.transform.position.z),
                new Vector3(escapeTarget.position.x, 0, escapeTarget.position.z)
            );

            // 2. 判斷是否抵達
            if (distance <= arrivalDistance)
            {
                reached = true;
                break; // 提前跳出迴圈
            }

            timer -= Time.deltaTime;

            // (選擇性) 如果你有 UI，可以在這裡更新倒數文字
            // uiTimerText.text = timer.ToString("F1"); 

            yield return null;
        }

        isTaskActive = false;

        if (reached)
        {
            // 成功
            Debug.Log("成功抵達！");
            // 你原本要求的 print (t/6e/j ) 
            print("t/6e/j");

            // 這裡可以觸發下一個劇情進度
        }
        else
        {
            // 失敗
            Debug.Log("時間到，逃跑失敗！");
            print("失敗");

            // 這裡可以觸發死亡動畫或是重新載入存盤點
        }
    }
    // 用於同步旋轉數值的輔助函式
    private void SyncPlayerRotationWithCamera()
    {
        // 取得當前攝影機的世界座標歐拉角
        Vector3 currentEuler = Player.tfPlayerCamera.eulerAngles;

        // 將這些角度同步給玩家控制器的旋轉變數 (假設你的 PlayerController 是透過 m_fRotationX 等變數控制)
        // 這部分需要確保你的 GrandmaRoom_Player 有開放這些變數修改
        // Player.m_fRotationY = currentEuler.y;
        // Player.m_fRotationX = currentEuler.x;
    }
    IEnumerator Lv4_E_BathRoomDoor()
    {
        if (BathRoomDoorStep == 0)
        {
            BathRoomDoorStep = 1;
            //窺視動畫，看見爸媽模糊的影子
            yield return new WaitForSeconds(5f);
            PlayDialogue((int)Room_Dialogue.Lv4_E_BathroomDoor);
            yield return new WaitForSeconds(10f);
            ShowHint(LevelTypeID.Lv3_GrandmaHouse_Memory, HintItemID.Lv4_BathRoomDoor);
        }
        else if (BathRoomDoorStep == 1)
        {
            //門關上後，場景暗掉
            //回到房間躺著的視角
        }
    }
    public void Lv4_PlayGiftBoxAnimation()
    {
        GameObject giftBoxObj = GameObject.Find("_Scene02_InteractItems/giftbox and bear");
        if (giftBoxObj == null)
        {
            Debug.LogError("找不到禮物盒物件！");
            return;
        }

        Animator animator = giftBoxObj.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("禮物盒物件沒有 Animator 組件！");
            return;
        }
        animator.enabled = true;
        animator.Play("Take 001", 0, 0f);
        animator.Update(0f);
        ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv4_Dam_MomPupptery);
    }

    void Lv4_E_GrandMaRoomDoor()
    {
        //客廳正中間桌子亮起
    }

    IEnumerator PlayTubAnim()
    {
        //播放浴缸動畫
        yield return new WaitForSeconds(10f);
        PlayDialogue((int)Room_Dialogue.Lv4_019_AfterTubAnimation);
    }

    IEnumerator Lv4_E_Tub()
    {
        //播放拿出護身符靠近浴缸動畫
        yield return new WaitForSeconds(10f);
        PlayDialogue((int)Room_Dialogue.Lv4_020_E_Tub);
    }
    public void PlayBabyAnim()
    {

    }

    void Lv4_E_Telephone()
    {
        PlayDialogue((int)Room_Dialogue.Lv4_010_E_Telephone);
        //接電話動畫
    }

    void Lv4_E_Piano()
    {
        PlayDialogue((int)Room_Dialogue.Lv4_010_E_Telephone);
        //接電話動畫
    }

    IEnumerator Lv4_E_Flower()
    {
        //捧起花，靠近嗅了一下動畫
        yield return new WaitForSeconds(10f);
        //回到房間躺著得視角
    }
    void Lv4_E_Amulet()
    {
        if (FirstE_Amulet == false)
        {
            PlayDialogue((int)Room_Dialogue.Lv4_015_E_Amulet);
            FirstE_Amulet = true;
        }
        else
        {
            //日曆紙掉下了一張，日期圈起來並在旁邊畫著嬰兒出生的圖案
            PlayDialogue((int)Room_Dialogue.Lv4_017_E_Amulet);
        }

    }
    IEnumerator Lv4_E_Bed()
    {
        PlayDialogue((int)Room_Dialogue.Lv4_016_E_Bed);
        //躺到床上，眨了幾下眼睛後閉上眼動畫
        yield return new WaitForSeconds(10f);
        //黑幕動畫
        yield return new WaitForSeconds(2f);
        //睜開眼睛，看到黑影坐在床角並看著門口
    }
    void Lv4_E_CalendarPaper()
    {
        PlayDialogue((int)Room_Dialogue.Lv4_018_E_CalendarPaper);
    }
    void Lv4_E_GrandmaDoor()
    {
        Lv4_GrandmaRoomDoorSwitch();
        badroomDoor.GetComponent<ItemController>().bAlwaysActive = false;
        badroomDoor.GetComponent<ItemController>().ItemDisable();
    }
    void Lv4_StartPlayingGetAmulet()
    {
        Player._bCanControl = false;
        Player.eyeStates = "open";
        Player.tfPlayerCamera.localPosition = new Vector3(0, -0.225f, 0);
        Player.tfPlayerCamera.eulerAngles = new Vector3(0, 90, 0);
    }
    #endregion
}
