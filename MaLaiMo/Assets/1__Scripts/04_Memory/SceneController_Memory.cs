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
    public GrandmaRoom_Player Player;
    [Header("房間門")] 
    public Transform badroomDoor;
    public GameObject LotusPaper;
    private static bool FilialPietyCurtain_IsOpen = false;
    private static bool canSwitchDoor = true;
    public GameObject calendarObject;
    private Animation calendarAnim;
    private int BathRoomDoorStep = 0;
    private static bool FirstE_Amulet = false;

    public GameObject FemaleGhost;

    public GameObject PSbloodrunning;
    public GameObject BloodWater;
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

        if (GlobalDeclare._checkList01_holdLotus)
        {
            TakingObjects[0].SetActive(true);
        }
        else if (GlobalDeclare._checkList02_holdRice)
        {
            ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_OfferingPlace);
            TakingObjects[1].SetActive(true);
        }

        if (!GlobalDeclare._firstStartGameLevel_1)
        {
            GlobalDeclare._firstStartGameLevel_1 = true;

            // 室內場景的第一個可互動物件
            GameEvent(LevelTypeID.Lv1_GrandmaHouse, GameEventID.Lv1_TalkToPackage);

            // *TODO* 以下為暫時設定的程式 > 待實際遊歷流程串接
            GlobalDeclare._holdingRiceFuneral = true;
            //ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_LotusPaper);
        }
        else
        {
            // 設定玩家傳送座標
            SetPlayerLocation(this._outsideGoInTransitPos.localPosition);

            // 非第一次進場場景 > 轉場圖片 Fade Out
            TransitFadeOut();
        }

        if (FilialPietyCurtain_IsOpen)
        {
            FilialPietyCurtain_Ani.SetTrigger("Filial_piety_curtain Open");
        }
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
        if(canSwitchDoor == false) return;
        Transform tfRoomDoor = GameObject.Find("_Scene01_InteractItems/Lv1_Door/Lv1_Grandma_Room_Door").transform;
        Animation AniRoomDoor = badroomDoor.GetComponent<Animation>();

        if (badroomDoor.localRotation.z > 0.49 || badroomDoor.localRotation.z == 0)
        {
            string strPlayAniName = badroomDoor.localRotation.z == 0 ? "Door_Open" : "Door_Close";

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
        Animation AniRoomDoor = badroomDoor.GetComponent<Animation>();
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
    void Bathtub_blood_search()
    {
        //女鬼出現

        if (FemaleGhost != null)
        {
            FemaleGhost.gameObject.SetActive(true);
            StartCoroutine(Lv4_Delayed_Rise_Of_The_Female_Ghost());
        }
    }
    IEnumerator Lv4_Delayed_Rise_Of_The_Female_Ghost()
    {
        yield return new WaitForSeconds(6f);
        Vector3 pos = FemaleGhost.transform.position;
        pos.y = -1.091f;
        FemaleGhost.transform.position = pos;
        // 使用 DOTween 讓 Y 緩慢升高到 0.866
        FemaleGhost.transform.DOMoveY(0.866f, 1f);
        // 設定起始 Y 位置

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
        if (FirstE_Amulet == false) {
            PlayDialogue((int)Room_Dialogue.Lv4_015_E_Amulet);
            FirstE_Amulet = true;
        }else{
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
    #endregion
}
