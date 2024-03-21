using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public partial class GameManager : MonoBehaviour
{
    public void IvkShowGrandmaFaceUI()
    {
        imgUIBackGround.color = new Color(1, 1, 1, 1);
        Sprite GrandmaFaceSprite = Resources.Load<Sprite>("Sprites/GrandmaFace");
        imgUIBackGround.sprite = GrandmaFaceSprite;
        Invoke(nameof(IvkShowEndView), 5f);
    }

    public void IvkShowEndView()
    {
        imgUIBackGround.color = new Color(1, 1, 1, 1);
        Sprite EndViewSprite = Resources.Load<Sprite>("Sprites/EndView");
        imgUIBackGround.sprite = EndViewSprite;
        m_bReturnToBegin = true;
        playerCtrlr.m_bCanControl = true;
    }

    public void IvkShowDoorKey()
    {
        ShowHint(HintItemID.Lv1_Grandma_Room_Key);
    }

    public void IvkProcessGhostHandPushAnimator()
    {
        ProcessPlayerAnimator("Player_Falling_In_Bathroom");
        DialogueObjects[(byte)Lv1_Dialogue.WakeUp_Lv2].CallAction();

        ShowHint(HintItemID.Lv2_Room_Door);
        ShowHint(HintItemID.Lv2_Light_Switch);
        ShowHint(HintItemID.Lv2_FlashLight);
        ShowHint(HintItemID.Lv2_Side_Table);

        Invoke(nameof(DelayGhostHandPush), 2.2f);
    }

    void DelayGhostHandPush()
    {
        ProcessItemAnimator("Ghost_Hand", "Ghost_Hand_Push");

        Invoke(nameof(IvkProcessPlayerWakeUpSecondTime), 3.5f);
    }

    public void IvkProcessPlayerWakeUpSecondTime()
    {
        playerCtrlr.m_bCanControl = true;
        playerCtrlr.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        playerCtrlr.gameObject.GetComponent<Rigidbody>().useGravity = true;

        Light playerFlashlight = playerCtrlr.tfPlayerCamera.GetComponent<Light>();
        playerFlashlight.enabled = false;
        audManager.Play(1, "Falling_To_Black_Screen_Sound_Part2", false);
        Animation am = playerCtrlr.GetComponent<Animation>();
        am.Stop();
        ProcessPlayerAnimator("Player_Wake_Up_SecondTime");
    }

    public void IvkShowLv2DoorKey()
    {
        ShowHint(HintItemID.Lv2_Room_Key);
    }

    void IvkLv1_SetGhostHandPosition()
    {
        // 鬼手出現
        GameObject GhostHandObj = GameObject.Find("Ghost_Hand");
        GhostHandObj.transform.position = new Vector3(-8.5f, 0, 6);

        // 鬼門觸發
        GameObject GhostHandTriggerObj = GameObject.Find("Ghost_Hand_Trigger");
        GhostHandTriggerObj.transform.position = new Vector3(-8.5f, 0.1f, 6.1f);

        ShowHint(HintItemID.Lv1_Toilet_GhostHand_Trigger);
    }

    public void IvkLv1_SetGrandmaGhostPosition()
    {
        Lv2_Grandma_Ghost_Obj.GetComponent<Animator>().applyRootMotion = true;
        Lv2_Grandma_Ghost_Obj.transform.localPosition = new Vector3(-8f, -2f, 2.35f);
    }

    public void IvkLv2_Grandma_Pass_Door()
    {
        Lv2_Grandma_Ghost_Obj.GetComponent<Animator>().applyRootMotion = true;
        ShowHint(HintItemID.Lv2_Toilet_Door);
        audManager.Play(1, "grandma_StrangeVoice", false);
    }

    public void Lv2DelayChangeObjectPos()
    {
        ProcessPlayerAnimator("Player_Lv2_Shocked_By_Toilet_Ghost");
        audManager.Play(1, "Crying_in_the_bathroom", false);
        Lv2_Furniture_State_1_Obj.SetActive(false);
        Lv2_Corridor_Door_Frame_Obj.SetActive(false);
        Lv2_Furniture_State_2_Obj.SetActive(true);
        Lv2_Wall_Replace_Door_Frame_Obj.SetActive(true);
        ShowHint(HintItemID.Lv2_Rice_Funeral);

        Invoke(nameof(DelayToiletGhostAni), 9.5f);
    }

    void DelayToiletGhostAni()
    {
        Lv2_Toilet_Door_GhostHead_Obj.GetComponent<Animator>().SetTrigger("Lv2_Toilet_Door_GhostHead_Scared");

        playerCtrlr.m_bCanControl = true;
        playerCtrlr.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        playerCtrlr.gameObject.GetComponent<Rigidbody>().useGravity = true;
    }

    void OnVideoPlayerStarted(VideoPlayer vp)
    {
        RawImgGrandmaUI.enabled = true;
        vp.started -= OnVideoPlayerStarted;
    }

    public void IvkLv2_PlayGrandmaVideo()
    {
        videoPlayer.Play();
    }

    void DelayBoySneakerDialog()
    {
        DialogueObjects[(byte)Lv1_Dialogue.Lv2_Boy_Sneaker].CallAction();
    }

    void DelayCheckBoySneaker()
    {
        audManager.Play(1, "Crying_in_the_bathroom", false);

        videoPlayer.started += OnVideoPlayerStarted;
        IvkLv2_PlayGrandmaVideo();

        ProcessPlayerAnimator("Player_Lv2_Shocked_After_PhotoFrame");

        Invoke(nameof(IvkLv2_SlientAfterPhotoFrameForRecord), 4f);
    }

    public void IvkLv2_SlientAfterPhotoFrameForRecord()
    {
        videoPlayer.Stop();
        RawImgGrandmaUI.enabled = false;
        FinalUI.SetActive(true);
        bIsGameEnd = true;
    }

    void IvkLv2_BrokenPhotoFrameEnable()
    {
        ShowHint(HintItemID.Lv1_Photo_Frame);
        TempItem = Lv1_Photo_Frame_Obj.GetComponent<ItemController>();
        TempItem.eventID = GameEventID.Lv1_Photo_Frame_Has_Broken;
    }

    void DelayEnterLotusGame()
    {
        Lv1_Lotus_Paper_Obj.transform.localPosition = new Vector3(-3.9f, -5f, -2.4f);

        if (bHasTriggerLotus)
        {
            LotusGameManager LotusCtrlr = GameObject.Find("LotusGameController").GetComponent<LotusGameManager>();
            LotusCtrlr.SendMessage("SetLotusCanvasEnable", true);
            LotusGameManager.bIsGamePause = false;
        }
        else
        {
            bHasTriggerLotus = true;
            SceneManager.LoadScene(3, LoadSceneMode.Additive);
        }
    }
}