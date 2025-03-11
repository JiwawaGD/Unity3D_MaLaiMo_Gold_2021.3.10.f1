using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController_OutSide : SceneController
{
    #region < Fields >
    [Header("=== By Scene 各場景使用欄位 ===\r\n")] public GameObject _temp;

    [Header("室內傳至室外的角色座標")] public Transform _insideGoOutTransitPos;

    public Transform mom;
    public RectTransform uiElement;
    public RectTransform arrowIndicator;
    public Camera PlayerCamera;
    public Transform Player;
    public GameObject ForestTP;

    private Vector3 IconPos;
    #endregion

    #region < Unity Hook >
    public override void Start()
    {
        base.Start();

        // 大門 Hint 保持開著
        ShowHint(LevelTypeID.Lv2_OutSideDoor, HintItemID.Lv2_GoInside);

        // 並非執行初次森林事件，就執行媽媽引導玩家
        if (!GlobalDeclare._firstStartGameLevel_2)
        {
            ForestTP.SetActive(true);
            mom.gameObject.SetActive(true);
            uiElement.gameObject.SetActive(true);
            arrowIndicator.gameObject.SetActive(true);
            PlayerCtrlr.tfTransform.LookAt(mom);
            PlayerCtrlr._bCanControl = false;
            GlobalDeclare._firstStartGameLevel_2 = true;
            PlayDialogue((byte)OutSide_Dialogue.Lv2_007_GoOut);
        }
        else
        {
            // 設定玩家傳送座標
            SetPlayerLocation(this._insideGoOutTransitPos.localPosition);

            // 非第一次進場場景 > 轉場圖片 Fade Out
            TransitFadeOut();
        }
    }

    public override void Update()
    {
        base.Update();
        if (GlobalDeclare._firstStartGameLevel_2 == true) return;

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
                        case HintItemID.Lv2_GoInside:
                            itemName = "_Scene02_InteractItems/Lv2_Front_Door";
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
    #endregion

    #region < Basic Function >
    void Lv2_Event(GameEventID r_EventID)
    {
        try
        {
            switch (r_EventID)
            {
                case GameEventID.Lv2_GoInside:
                    Lv2_GoInside();
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
    void Lv2_GoInside()
    {
        SetPlayerControl(false);

        _transitBlackImg.DOFade(1f, 1)
                        .OnComplete(() => SceneManager.LoadScene(GlobalDeclare.Lv1_Grandma_House));
    }
    #endregion
}
