using UnityEngine;
using UnityEngine.UI;

public class TestManager : MonoBehaviour
{
    public Button Btn_Straight;
    public Button Btn_Back;
    public Button Btn_Reset;

    public Text txt_SuccessCount;
    public Text txt_PassCount;
    public Text txt_Title;

    bool[] m_baHasPlayLevel = new bool[7];

    SceneType CurrentScene;

    int m_iSuccessCount;
    int m_iPassLevelCount;

    public enum SceneType
    {
        A_Right,
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

    void Start()
    {
        Btn_Straight.onClick.AddListener(GoStraight);
        Btn_Back.onClick.AddListener(GoBack);
        Btn_Reset.onClick.AddListener(Reset);
    }

    void GoStraight()
    {
        CheckIsGoRight(GoWay.Straight);
    }

    void GoBack()
    {
        CheckIsGoRight(GoWay.Back);
    }

    void CheckIsGoRight(GoWay way)
    {
        bool bPassSuccess = false;

        if (CurrentScene == SceneType.A_Right && way == GoWay.Straight)
        {
            m_iPassLevelCount++;
            m_iSuccessCount++;

            bPassSuccess = true;
        }
        else if (CurrentScene != SceneType.A_Right && way == GoWay.Back)
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

            if (CurrentScene == SceneType.A_Right)
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

        CurrentScene = NextSceneType;
        txt_Title.text = "目前關卡 : " + CurrentScene.ToString();
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