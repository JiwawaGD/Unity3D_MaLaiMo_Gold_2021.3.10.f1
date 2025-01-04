using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController_Room : SceneController
{
    #region - Light Zone -
    #endregion

    #region - All Scene Items -
    #endregion

    #region - Game Point (記錄點) -
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
        //bIsPlayingPiano = true;
        //Transform tfPianoPos = GameObject.Find("PianoTarget").GetComponent<Transform>();
        //Transform tfCameraPos = tfPianoPos.GetChild(0);

        //yield return StartCoroutine(PlayerToAniPos(Targers[index].position, tfPianoPos.rotation, tfCameraPos.rotation));
        yield return null;

        //if (bIsPlayingPiano == true)
        //    PianoUI.SetActive(true);
    }

    void QuitPiano()
    {
        //bIsPlayingPiano = false;
        //PianoUI.SetActive(false);
        //playerCtrlr.m_bCanControl = true;
        //playerCtrlr.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        //playerCtrlr.gameObject.GetComponent<Rigidbody>().useGravity = true;
    }

    void ShowQRCode()
    {
        //bIsGameEnd = false;
        m_bReturnToBegin = true;
        //FinalUI.SetActive(false);
        QRCodeUI.SetActive(true);
    }

    // 延遲載入大廳場景
    IEnumerator DelayLodelobby()
    {
        audManager.Play(1, "Opening_Scene", false);
        //FinalUI.SetActive(true);
        SceneManager.LoadScene(0);
        yield return null;
    }

    void QuitLotusGame()
    {
        //bIsPlayingLotus = false;
        //playerCtrlr.tfPlayerCamera.gameObject.SetActive(true);
        //Lv1_Lotus_Paper_Obj.transform.localPosition = new Vector3(-3.9f, 0.6f, -2.4f);

        //playerCtrlr.transform.localPosition = new Vector3(-3, 0.8f, -2.5f);
        //playerCtrlr.m_bCanControl = true;
        //playerCtrlr.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        //playerCtrlr.gameObject.GetComponent<Rigidbody>().useGravity = true;

        //LotusGameManager LotusCtrlr = GameObject.Find("LotusGameController").GetComponent<LotusGameManager>();
        //LotusCtrlr.SendMessage("SetLotusCanvasEnable", false);

        //LotusGameManager.bIsGamePause = true;
    }

    // 離開蓮花遊戲
    public void ExitLotusGame()
    {
        //m_bPlayLotusEnable = false;
        //bIsPlayingLotus = false;
        //playerCtrlr.m_bCanControl = true;
        //playerCtrlr.tfPlayerCamera.gameObject.SetActive(true);
        //playerCtrlr.transform.localPosition = new Vector3(-3, 0.8f, -2.5f);

        //playerCtrlr.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        //playerCtrlr.gameObject.GetComponent<Rigidbody>().useGravity = true;

        //SceneManager.UnloadSceneAsync(3);

        //Lv1_Lotus_Paper_Obj.transform.localPosition = new Vector3(-3.9f, -2f, -2.4f);
        //Lv1_Finished_Lotus_Paper_Obj.transform.localPosition = new Vector3(-3.9f, 0.6f, -2.4f);
    }

    public void ButtonFunction(ButtonEventID _eventID)
    {
        switch (_eventID)
        {
            case ButtonEventID.UI_Back:
                break;
            case ButtonEventID.Enter_Game:
                //if (bIsUIOpen)
                //{
                //    RestoreItemLocation();
                //    bIsPlayingLotus = true;

                //    Transform tfPlayingLotusPos = GameObject.Find("Lv1_Playing_Lotus_Pos").GetComponent<Transform>();
                //    Transform tfCameraPos = tfPlayingLotusPos.GetChild(0);
                //    StartCoroutine(PlayerToAniPos(tfPlayingLotusPos.position, tfPlayingLotusPos.rotation, tfCameraPos.rotation));
                //}
                break;
        }
    }
    #endregion
}