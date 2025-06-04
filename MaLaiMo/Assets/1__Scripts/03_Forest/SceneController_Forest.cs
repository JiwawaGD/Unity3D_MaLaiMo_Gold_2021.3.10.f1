using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneController_Forest : SceneController
{
    public Button Btn_Straight;
    public Button Btn_Back;
    public Button Btn_Reset;

    public Text txt_SuccessCount;
    public Text txt_PassCount;
    public Text txt_Title;
    public Image TransitionImg;

    bool[] m_baHasPlayLevel = new bool[7];

    public SceneType CurrentScene_2;
    public bool FinsihFirstDia = false;
    public GameObject[] SceneOnjects;
    public GameObject CurrentSceneOnject;
    public ForestPlayer ForestPlayer;
    public GameObject E_WrongGhost;
    public GameObject H_WrongGhost;
    int m_iSuccessCount;
    int m_iPassLevelCount;

    public enum SceneType
    {
        A_Right = 0,
        B_Wrong,
        C_Wrong,
        D_Wrong,
        E_Wrong,
        F_Wrong,
        G_Wrong,
        H_Wrong,
    }

    public enum GoWay
    {
        Straight,
        Back,
    }

    //public override void Awake()
    //{
    //    // DialogueObjects[0].CallAction(false);
    //}

    public override void Start()
    {
        TransitionImg.DOFade(0f, 1)
                  .OnComplete(() => PlayDialogue((byte)Lv3_Dialogue.Lv3_000_LookNote));
    }

    public override void UIState(int r_ItemID, bool r_bEnable, bool r_bNeedSubTitle = false)
    {
        base.UIState(r_ItemID, r_bEnable, r_bNeedSubTitle);

        _itemCanvasHandler._txtTopTitle.text = r_bEnable ? GlobalDeclare.item_Title_Forest[(int)r_ItemID] : "";
        _itemCanvasHandler._txtMainInfo.text = r_bEnable ? GlobalDeclare.item_MainInfo_Forest[(int)r_ItemID] : "";
        _itemCanvasHandler._txtBottonInfo.text = r_bEnable ? (r_bNeedSubTitle ? GlobalDeclare.item_BottonInfo_Forest[(int)r_ItemID] : "") : "";
    }



    public void GoStraight()
    {
        CheckIsGoRight(GoWay.Straight);
    }

    public void GoBack()
    {
        CheckIsGoRight(GoWay.Back);
    }

    void CheckIsGoRight(GoWay way)
    {
        bool bPassSuccess = false;

        if (CurrentScene_2 == SceneType.A_Right && way == GoWay.Straight)
        {
            m_iPassLevelCount++;
            m_iSuccessCount++;

            bPassSuccess = true;
        }
        else if (CurrentScene_2 != SceneType.A_Right && way == GoWay.Back)
        {
            m_iPassLevelCount++;

            bPassSuccess = true;
        }
        else
        {
            m_iPassLevelCount = 0;
            m_iSuccessCount = 0;
        }

        RandomNextLevel(bPassSuccess);
        SetCount();
    }

    void RandomNextLevel(bool r_bPassSuccess)
    {
        SceneType NextSceneType;
        int iNextLevelWeight = Random.Range(1, 11);

        int iWrongLevelIndex = GetRandomUnRepeatLevelIndex();
        int iRightLevelWeight;

        if (m_iSuccessCount > 7)
            Reset();

        if (r_bPassSuccess)
        {
            if (m_iSuccessCount == 7)
            {
                txt_Title.text = "YOU WIN!!";
                return;
            }

            if (CurrentScene_2 == SceneType.A_Right)
            {
                switch (m_iSuccessCount)
                {
                    case 0:
                        iRightLevelWeight = 8;
                        break;
                    case 1:
                    case 2:
                        iRightLevelWeight = 6;
                        break;
                    case 3:
                    case 4:
                        iRightLevelWeight = 5;
                        break;
                    case 5:
                        iRightLevelWeight = 4;
                        break;
                    case 6:
                        iRightLevelWeight = 2;
                        break;
                    default:
                        iRightLevelWeight = 0;
                        Debug.LogError("Wrong Weight");
                        break;
                }
            }
            else
            {
                iRightLevelWeight = 8;
            }
        }
        else
        {
            // 通關失敗 => 80%正確場景, 20% 錯誤場景
            iRightLevelWeight = 8;
        }

        if (iNextLevelWeight <= iRightLevelWeight)
        {
            NextSceneType = SceneType.A_Right;
        }
        else
        {
            NextSceneType = (SceneType)iWrongLevelIndex;
            m_baHasPlayLevel[iWrongLevelIndex - 1] = true;
        }

        CurrentScene_2 = NextSceneType;
        CurrentSceneOnject.SetActive(false);
        if(CurrentScene_2 == SceneType.E_Wrong && ForestPlayer.NowDirection == "向右")
        {
            E_WrongGhost.transform.localPosition = new Vector3(-0.47f, 5.18f, -62.69f);
            print("對面");
        }
        else if(CurrentScene_2 == SceneType.H_Wrong && ForestPlayer.NowDirection == "向右")
        {
            H_WrongGhost.transform.localPosition = new Vector3(-0.47f, 5.18f, -62.69f);
            print("對面");
        }
        CurrentSceneOnject = SceneOnjects[(int)CurrentScene_2];
        CurrentSceneOnject.SetActive(true);
        //txt_Title.text = "目前關卡 : " + CurrentScene.ToString();
    }

    int GetRandomUnRepeatLevelIndex()
    {
        int iWrongLevelCount = m_iSuccessCount > 0 ? 8 : 7;
        int iWrongLevelIndex = Random.Range(1, iWrongLevelCount);

        bool bResetLevelState = true;

        for (int iLevelIndex = 1; iLevelIndex < iWrongLevelCount; iLevelIndex++)
        {
            if (!m_baHasPlayLevel[iLevelIndex - 1])
            {
                bResetLevelState = false;
                break;
            }
        }

        if (bResetLevelState)
        {
            for (int iLevelIndex = 1; iLevelIndex < iWrongLevelCount; iLevelIndex++)
                m_baHasPlayLevel[iLevelIndex - 1] = false;
        }

        bool bChcekIndexEnable = true;

        while (bChcekIndexEnable)
        {
            if (m_baHasPlayLevel[iWrongLevelIndex - 1])
            {
                iWrongLevelIndex = Random.Range(1, iWrongLevelCount);
            }
            else
            {
                break;
            }
        }

        return iWrongLevelIndex;
    }

    void SetCount()
    {
        txt_SuccessCount.text = m_iSuccessCount.ToString();
        txt_PassCount.text = m_iPassLevelCount.ToString();
    }

    public void CallFlashLightNote() 
    {
        UINote("提示 - 按 F 開啟/關閉手電筒", true);
        FinsihFirstDia = true;
    }

    void Reset()
    {
        for (int iLevelIndex = 0; iLevelIndex < m_baHasPlayLevel.Length; iLevelIndex++)
            m_baHasPlayLevel[iLevelIndex] = false;

        m_iSuccessCount = 0;
        m_iPassLevelCount = 0;

        SceneType NextSceneType = SceneType.A_Right;
        txt_Title.text = "目前關卡 : " + NextSceneType.ToString();

        SetCount();
    }
}