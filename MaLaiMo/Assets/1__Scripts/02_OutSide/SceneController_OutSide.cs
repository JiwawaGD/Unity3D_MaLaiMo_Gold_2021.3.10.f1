using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController_OutSide : SceneController
{
    #region < Fields >
    [Header("=== By Scene 各場景使用欄位 ===\r\n")] public GameObject _temp;

    [Header("室內傳至室外的角色座標")] public Transform _insideGoOutTransitPos;

    public Transform mom;
    public Mom_Controller mom_Controller;
    public RectTransform uiElement;
    public RectTransform arrowIndicator;
    public Camera PlayerCamera;
    public Transform Player;
    public GameObject ForestTP;
    public static string nowMission = "完成紙上任務";
    [Header("代辦事項刪除線")] public GameObject[] paperFinish;
    public GameObject[] interactiopaperFinish;
    public GameObject FlowerCircle;
    public Animation Player_Ani;
    public Animator MomAnimator;
    public GameObject HandAni;
    public GameObject EnvironmentLight;
    public GameObject Rice_Funeral;
    public static bool FinishDollar = false;
    public static bool MomFirstTalk = false;
    public static bool readPaper = false;
    private bool isHandlingCoinEvent = false;
    /// <summary>
    /// 角色控制器
    /// </summary>
    public GameObject[] TakingObjects;
    [SerializeField] private InteractionController interactionController;

    #endregion

    #region < Unity Hook >
    public override void Start()
    {
        base.Start();
        TransitFadeOut();
        GlobalDeclare._firstStartGameLevel_2 = false;
        // 大門 Hint 保持開著
        ShowHint(LevelTypeID.Lv2_OutSideDoor, HintItemID.Lv2_OutSideDoor);

        if(nowMission == "完成紙上任務")
        {
            MomAnimator.gameObject.SetActive(true);
            if (GlobalDeclare._checkList01_holdLotus == true) TakingObjects[0].SetActive(true);
            else if (GlobalDeclare._checkList02_holdRice == true) TakingObjects[1].SetActive(true);
            ShowHint(LevelTypeID.Lv2_OutSideDoor, HintItemID.Lv2_Mom);
            //初次和媽媽說話並且看過代辦事項才可以觸發代辦事件
            if (MomFirstTalk == true && readPaper == true)
            {
                //判斷代辦事項刪除線是否開啟
                for (int i = 0; i < paperMissionFinsih.Length; i++)
                {
                    paperFinish[i].SetActive(paperMissionFinsih[i]);
                    interactiopaperFinish[i].SetActive(paperMissionFinsih[i]);
                    if (paperMissionFinsih[i] == true) break;
                    ShowHint(LevelTypeID.Lv2_OutSideDoor, HintItemID.Lv2_Paper);
                }
                CheckParperMission();
            }
        }
        // 若目前任務為森林事件，就執行媽媽引導玩家
        else if (nowMission == "跟著媽媽去森林")
        {
            ForestTP.SetActive(true);
            mom.gameObject.SetActive(true);
            PlayerCtrlr.tfTransform.LookAt(mom);
            PlayerCtrlr._bCanControl = false;
            // GlobalDeclare._firstStartGameLevel_2 = true;
            PlayDialogue((byte)OutSide_Dialogue.Lv2_007_GoOut);
        }
        // 若目前任務為頭七事件，就執行玩家拜拜動畫
        else if (nowMission == "頭七")
        {
            Player.LookAt(FlowerCircle.transform.position);
            Player_Ani.enabled = true;
            Player_Ani.PlayQueued("Player_GoTOMourningHall");
            StartCoroutine(FirstSevenDay());
        }
    }

    void CheckParperMission()
    {
        if (paperMissionFinsih[(int)PaperMission.LayOutSideCircle] == false)
        {
            FlowerCircle.transform.rotation = Quaternion.Euler(-11.03f, -10f, 0f);
            FlowerCircle.transform.position = new Vector3(FlowerCircle.transform.position.x, FlowerCircle.transform.position.y, 477.286f);
            ShowHint(LevelTypeID.Lv2_OutSideDoor, HintItemID.Lv2_FlowerCircle);
        }

        if (FinishDollar == false)
        {
            ShowHint(LevelTypeID.Lv2_OutSideDoor, HintItemID.LV2_10Dollar);
        }

        if (GlobalDeclare._checkList01_holdLotus == true)
        {
            //放紙蓮花
            ShowHint(LevelTypeID.Lv2_OutSideDoor, HintItemID.Lv2_Table);
        }

        if(GlobalDeclare._checkList02_holdRice == false && paperMissionFinsih[(int)PaperMission.TalkRiceToKitchen] == false) ShowHint(LevelTypeID.Lv2_OutSideDoor, HintItemID.Lv2_Cuisine_Soup);
        else Rice_Funeral.SetActive(false);
    }

    public void MomBackToWrite()
    {
        MomAnimator.SetInteger("Step", 2);
    }

    public override void Update()
    {
        base.Update();

        if (GlobalDeclare._waitingPlay10Dollar && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("觸發 Start10DollarEvent()");
            Start10DollarEvent();
        }
        if (GlobalDeclare._firstStartGameLevel_2 == true || mom_Controller.AFKTimeCount < 1200)
        {
            uiElement.gameObject.SetActive(false);
            arrowIndicator.gameObject.SetActive(false);
            return;
        }

        uiElement.gameObject.SetActive(true);
        arrowIndicator.gameObject.SetActive(true);
        Vector3 npcViewportPos = PlayerCamera.WorldToViewportPoint(mom.position);
        Vector3 npcDirection = (mom.position - Player.position).normalized;

        //是否在玩家視野範圍內
        if (npcViewportPos.z > 0 && npcViewportPos.x > 0 && npcViewportPos.x < 1 && npcViewportPos.y > 0 && npcViewportPos.y < 1)
        {
            uiElement.gameObject.SetActive(true);
            arrowIndicator.gameObject.SetActive(false);
            Vector3 headOffset = new Vector3(0, 1.8f, 0); //標記在頭上
            uiElement.position = PlayerCamera.WorldToScreenPoint(mom.position + headOffset);
        }
        else
        {
            uiElement.gameObject.SetActive(false);
            arrowIndicator.gameObject.SetActive(true);

            Vector3 relativePos = Player.InverseTransformPoint(mom.position);
            float angleToNPC = Mathf.Atan2(relativePos.x, relativePos.z) * Mathf.Rad2Deg;

            arrowIndicator.rotation = Quaternion.Euler(0, 0, -angleToNPC);//icon角度 ps:可以再調整

            if (npcViewportPos.z < 0)
            {
                npcViewportPos.x = 1 - npcViewportPos.x;
                npcViewportPos.y = 1 - npcViewportPos.y;
            }

            float screenX = Mathf.Clamp(npcViewportPos.x, 0.05f, 0.95f) * Screen.width;
            float screenY = Mathf.Clamp(npcViewportPos.y, 0.05f, 0.95f) * Screen.height;

            // npc在相機後面時，提示icon在底部
            if (relativePos.z < 0)
            {
                screenY = 0.05f * Screen.height;
            }

            arrowIndicator.position = new Vector3(screenX, screenY, 0);
        }
    }
    #endregion

    #region < Override Function >
    public override void GameEvent(LevelTypeID r_SceneTypeID, GameEventID r_EventID)
    {
        base.GameEvent(r_SceneTypeID, r_EventID);

        switch (r_SceneTypeID)
        {
            case LevelTypeID.Lv2_OutSideDoor:
                Lv2_Event(r_EventID);
                break;
            default:
                Debug.LogError(string.Format("[SceneCtrlr - OutSide] Error SceneTypeID : {0}", r_SceneTypeID));
                break;
        }
    }

    public override void KeyboardCheck()
    {
        base.KeyboardCheck();
    }

    void Start10DollarEvent()
    {
        // 已經在處理硬幣事件，直接返回
        if (isHandlingCoinEvent) return;

        isHandlingCoinEvent = true;
        //SetItemCanvasState(false);
        //SetCrosshairEnable(true);
        UIState((int)UIItemID.Empty, false);
        GlobalDeclare._waitingPlay10Dollar = false;
        GlobalDeclare._played10DollarEvent = true;

        // 呼叫InteractionController
        interactionController.StartThrowingSequence();
    }

    public override void ShowHint(LevelTypeID r_SceneTypeID, HintItemID r_ItemID)
    {
        base.ShowHint(r_SceneTypeID, r_ItemID);

        try
        {
            ItemController NextItem = null;

            switch (r_SceneTypeID)
            {
                case LevelTypeID.Lv2_OutSideDoor:
                    string itemName = "";

                    switch (r_ItemID)
                    {
                        case HintItemID.Lv2_OutSideDoor:
                            itemName = "_Scene02_InteractItems/Lv2_Front_Door";
                            break;
                        case HintItemID.Lv2_Mom:
                            itemName = "_Scene02_InteractItems/Lv2_Mom";
                            break;
                        case HintItemID.Lv2_Paper:
                            itemName = "_Scene02_InteractItems/Lv2_Paper";
                            break;
                        case HintItemID.Lv2_Table:
                            itemName = "_Scene02_InteractItems/Lv2_Table";
                            break;
                        case HintItemID.LV2_10Dollar:
                            itemName = "_Scene02_InteractItems/Lv2_10Dollar";
                            break;
                        case HintItemID.Lv2_FlowerCircle:
                            itemName = "_Scene02_InteractItems/Lv2_FlowerCircle";
                            break;
                        case HintItemID.Lv2_Cuisine_Soup:
                            itemName = "_Scene02_InteractItems/Lv2_Cuisine_Soup";
                            break;
                        default:
                            Debug.LogError(string.Format("[ERROR] [ShowHint] [Lv2_OutSideDoor] Error Item ID :: {0}", r_ItemID));
                            break;
                    }

                    NextItem = GameObject.Find(itemName).GetComponent<ItemController>();
                    break;
                default:
                    Debug.LogError(string.Format("[ERROR] [ShowHint] [Lv2_OutSideDoor] Error Scene ID :: {0}", r_SceneTypeID));
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

    public override void SetPlayerLocation(Vector3 location)
    {
        base.SetPlayerLocation(location);
    }

    public override void SetPlayerControl(bool r_bEnable)
    {
        base.SetPlayerControl(r_bEnable);
    }

    public override void MoveItem(UIItemID r_ItemID)
    {
        switch (r_ItemID)
        {
            case UIItemID.Lv2_Paper:
                _itemObjsForRawImage[this._currentItemIndex].transform.DOMove(
                    new Vector3(-27.887f, 1.922f, 8.7448f), 0.5f);
                break;
            case UIItemID.Lv2_10Dollar:
                _itemObjsForRawImage[this._currentItemIndex].transform.DOMove(
                    new Vector3(-27.82f, 1.85f, 8.494f), 0.5f);
                break;
        }
    }

    public override void UIState(int r_ItemID, bool r_bEnable, bool r_bNeedSubTitle = false)
    {
        base.UIState(r_ItemID, r_bEnable, r_bNeedSubTitle);

        if(r_bEnable == true) EnvironmentLight.SetActive(false);
        else EnvironmentLight.SetActive(true);

        _itemCanvasHandler._txtTopTitle.text =
            r_bEnable ? GlobalDeclare.item_Title_OutSide[r_ItemID] : "";

        _itemCanvasHandler._txtMainInfo.text =
            r_bEnable ? GlobalDeclare.item_MainInfo_OutSide[r_ItemID] : "";

        _itemCanvasHandler._txtBottonInfo.text =
            r_bEnable ? (r_bNeedSubTitle ? GlobalDeclare.item_BottonInfo_OutSide[r_ItemID] : "") : "";
    }

    #endregion

    #region < Basic Function >
    void Lv2_Event(GameEventID r_EventID)
    {
        //try
        //{
            switch (r_EventID)
            {
                case GameEventID.Lv2_GoInside:
                TransitFadeIn(GlobalDeclare.Lv1_Grandma_House);
                    break;
                case GameEventID.Lv2_TalkToMom:
                    Lv2_TalkToMom();
                    break;
                case GameEventID.Lv2_CheckPaper:
                    readPaper = true;
                    UIState((int)UIItemID.Lv2_Paper, true, false);
                    ShowHint(LevelTypeID.Lv2_OutSideDoor, HintItemID.Lv2_FlowerCircle);
                    CheckParperMission();
                    break;
                case GameEventID.Lv2_PutLotusPaper:
                    Lv2_PutLotusPaper();
                    break;
                case GameEventID.LV2_10Dollar:
                    if (GlobalDeclare._checkList01_holdLotus == false && GlobalDeclare._checkList02_holdRice == false)
                    {
                        // 如果已經在處理硬幣事件，直接返回
                        if (isHandlingCoinEvent) break;

                        UIState((int)UIItemID.Lv2_10Dollar, true, true);
                        GlobalDeclare._waitingPlay10Dollar = true;
                        SetPlayerControl(false);
                    }
                    else PlayDialogue((byte)OutSide_Dialogue.Lv2_009_ShouldPutDown);
                    break;
                case GameEventID.Lv2_FlowerCircle:
                    if (GlobalDeclare._checkList01_holdLotus == false && GlobalDeclare._checkList02_holdRice == false) Lv2_FlowerCircle();
                    else PlayDialogue((byte)OutSide_Dialogue.Lv2_009_ShouldPutDown);
                    break;
                case GameEventID.Lv2_Cuisine_Soup:
                    if(GlobalDeclare._checkList01_holdLotus == false) Lv2_Cuisine_Soup();
                    else PlayDialogue((byte)OutSide_Dialogue.Lv2_009_ShouldPutDown);
                    break;
                default:
                    Debug.LogError(string.Format("<color=red><b>[Error]</b></color> [Lv1_Event] Error Event ID :: {0}", r_EventID));
                    break;
            }
        //}
        //catch (System.Exception exception)
        //{
        //    Debug.LogError(string.Format("[<color=red><b>Error</b></color>] [Lv1_Event] Event  <color=red><b>{0}</b></color>  Error  ::  {1}", r_EventID, exception.Message));
        //    throw;
        //}
    }

    public void UIStateByIndex(LevelTypeID level, int index)
    {
        _itemCanvasHandler._txtTopTitle.text = GlobalDeclare.item_Title_OutSide[index];
        _itemCanvasHandler._txtMainInfo.text = GlobalDeclare.item_MainInfo_OutSide[index];
        _itemCanvasHandler._txtBottonInfo.text = GlobalDeclare.item_BottonInfo_OutSide[index];
    }
    #endregion

    #region < Game Event >
    void Lv2_TalkToMom()
    {
        PlayerCtrlr._bCanControl = false;
        MomAnimator.SetInteger("Step", 1);
        if (MomFirstTalk == false)
        {
            MomFirstTalk = true;
            PlayDialogue((byte)OutSide_Dialogue.Lv2_000_E_Mother_First);
            ShowHint(LevelTypeID.Lv2_OutSideDoor, HintItemID.Lv2_Paper);
        }
        else
        {
            for (int i = 0; i < paperMissionFinsih.Length; i++)
            {
                if (paperMissionFinsih[i] == false)
                {
                    PlayDialogue((byte)OutSide_Dialogue.Lv2_001_E_Mother_PaperNotFinish);
                    return;
                }
            }
            PlayDialogue((byte)OutSide_Dialogue.Lv2_002_E_Mother_PaperFinish);
        }
    }

    void Lv2_PutLotusPaper()
    {
        TakingObjects[0].SetActive(false);
        GlobalDeclare._checkList01_holdLotus = false;
        paperMissionFinsih[(int)PaperMission.PutLotusOnTable] = true;
        paperFinish[(int)PaperMission.PutLotusOnTable].SetActive(true);
        interactiopaperFinish[(int)PaperMission.PutLotusOnTable].SetActive(true);
        paperFinish[(int)PaperMission.PutLotusOnTable].SetActive(true);
        interactiopaperFinish[(int)PaperMission.PutLotusOnTable].SetActive(true);
    }

    void Lv2_FlowerCircle()
    {
        FlowerCircle.GetComponent<ItemController>().bAlwaysActive = false;
        FlowerCircle.GetComponent<ItemController>().ItemDisable();
        PlayerCtrlr._bCanControl = false;
        FlowerCircle.transform.DORotate(new Vector3(0, 0, 0), 1);
        FlowerCircle.transform.DOMove(new Vector3(494.652f, -0.117f, 476.953f), 1);
        PlayDialogue((byte)OutSide_Dialogue.Lv2_003_E_FlowerCircle);
        paperMissionFinsih[(int)PaperMission.LayOutSideCircle] = true;
        paperFinish[(int)PaperMission.LayOutSideCircle].SetActive(true);
        interactiopaperFinish[(int)PaperMission.LayOutSideCircle].SetActive(true);
    }
    void Lv2_Cuisine_Soup()
    {
        if(FinishDollar == false) PlayDialogue((byte)OutSide_Dialogue.Lv2_008_DollarNotFinish);
        else
        {
            GlobalDeclare._checkList02_holdRice = true;
            TakingObjects[1].SetActive(true);
            Rice_Funeral.SetActive(false);
        }
    }

    IEnumerator FirstSevenDay()
    {
        yield return new WaitForSeconds(9f);
        HandAni.SetActive(true);
        yield return new WaitForSeconds(9f);
        PlayDialogue((byte)OutSide_Dialogue.Lv2_004_FirstSevenDays_Half);
    }

    public void FinishHeardMelody()
    {
        Player_Ani.enabled = false;
        nowMission = "";
    }

    public void OnCoinThrowingFinished()
    {
        isHandlingCoinEvent = false;
        Debug.Log("硬幣投擲結束，可以再次互動");
    }
    #endregion
}
