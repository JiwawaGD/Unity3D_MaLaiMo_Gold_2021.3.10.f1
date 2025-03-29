using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class LotusGameManager : MonoBehaviour
{
    #region < Field >
    [SerializeField] [Header("七個蓮花紙 - 物件")] GameObject[] LotusPaperObj;
    [SerializeField] [Header("七個蓮花紙 - 動畫控制器")] Animator[] LotusPaperAni;
    [SerializeField] [Header("七個蓮花紙 - 表演座標")] Vector3[] _showLocation;
    [SerializeField] [Header("七個蓮花紙 - 表演角度")] Vector3[] _showRotation;

    [SerializeField] [Header("七個蓮花紙 - 全部動畫片段")] AnimationClip[] LotusPaperAniClip;

    [SerializeField] [Header("八個方向的提示 - 圖片")] Sprite[] HintSprite;

    [SerializeField] [Header("提示按鈕 - 物件")] GameObject HintObj;

    [SerializeField] [Header("摺完的蓮花 - 物件")] GameObject _finishedLotusPaper;

    [SerializeField] [Header("音效撥放器")] AudioSource lotusAudioSource;

    int _currentState;
    int iAllLotusCount;
    bool[] _bLotusStates;
    bool _isAnimating;

    Image HintImg;
    RectTransform HintRectTf;
    Transform TfLotus;
    AnimatorStateInfo LotusState;

    public static bool bIsGamePause = false;

    readonly string[] strLotusAniTriggerName = new string[30]
    {
        "State_1",
        "State_2",
        "State_3",
        "State_4",
        "State_5",
        "State_6",
        "State_7-1",
        "State_7-2",
        "State_7-3",
        "State_7-4",
        "State_7-5",
        "State_7-6",
        "State_7-7",
        "State_7-8",
        "State_7-9",
        "State_7-10",
        "State_7-11",
        "State_7-12",
        "State_7-13",
        "State_7-14",
        "State_7-15",
        "State_7-16",
        "State_7-17",
        "State_7-18",
        "State_7-19",
        "State_7-20",
        "State_7-21",
        "State_7-22",
        "State_7-23",
        "State_7-24",
    };
    #endregion

    #region < API >
    public bool[] LotusPlayPoint
    {
        get { return this._bLotusStates; }
    }

    public void SetPaperLocation()
    {
        Transform tfPaper = LotusPaperObj[0].transform;
        tfPaper.localPosition = this._showLocation[0];
        tfPaper.localRotation = Quaternion.Euler(this._showRotation[0]);
    }
    #endregion

    #region < Unity Hook >
    void Awake()
    {
        HintImg = HintObj.GetComponent<Image>();
    }

    void Start()
    {
        iAllLotusCount = 30;

        this._bLotusStates = new bool[iAllLotusCount];

        for (int index = 0; index < iAllLotusCount; index++)
            _bLotusStates[index] = false;

        _bLotusStates[0] = true;
        TfLotus = LotusPaperObj[6].transform;
    }

    void Update()
    {
        if (!GlobalDeclare._playingLotusGame)
            return;

        if (_isAnimating)
            return;

        if (bIsGamePause)
            return;

        KeyBoardCheck();
    }
    #endregion

    #region < Internal Method >
    void KeyBoardCheck()
    {
        // 當有任何按鍵被按下
        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key) &&
                (key == KeyCode.W | key == KeyCode.A || key == KeyCode.S || key == KeyCode.D ||
                 key == KeyCode.Q || key == KeyCode.E || key == KeyCode.Z || key == KeyCode.C))
            {
                PlayLotusAni(key);
            }
        }
    }

    void PlayLotusAni(KeyCode r_key)
    {
        switch (r_key)
        {
            case KeyCode.W:
                if (_bLotusStates[0])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[0], LotusPaperAniClip[0], strLotusAniTriggerName[0], 0));
                }
                else if (_bLotusStates[3])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[3], LotusPaperAniClip[3], strLotusAniTriggerName[3], 3));
                }
                else if (_bLotusStates[9])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[6], LotusPaperAni[6], LotusPaperAniClip[9], strLotusAniTriggerName[9], 9));
                }
                else if (_bLotusStates[17])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[6], LotusPaperAni[6], LotusPaperAniClip[17], strLotusAniTriggerName[17], 17));
                }
                else if (_bLotusStates[25])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[5], LotusPaperAni[6], LotusPaperAniClip[25], strLotusAniTriggerName[25], 25));
                }
                break;
            case KeyCode.A:
                if (_bLotusStates[2])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[0], LotusPaperAni[2], LotusPaperAniClip[2], strLotusAniTriggerName[2], 2));
                }
                else if (_bLotusStates[8])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[0], LotusPaperAni[6], LotusPaperAniClip[8], strLotusAniTriggerName[8], 8));
                }
                else if (_bLotusStates[16])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[0], LotusPaperAni[6], LotusPaperAniClip[16], strLotusAniTriggerName[16], 16));
                }
                else if (_bLotusStates[24])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[0], LotusPaperAni[6], LotusPaperAniClip[24], strLotusAniTriggerName[24], 24));
                }
                break;
            case KeyCode.S:
                if (_bLotusStates[1])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[1], LotusPaperAni[1], LotusPaperAniClip[1], strLotusAniTriggerName[1], 1));
                }
                else if (_bLotusStates[4])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[3], LotusPaperAni[4], LotusPaperAniClip[4], strLotusAniTriggerName[4], 4));
                }
                else if (_bLotusStates[7])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[1], LotusPaperAni[6], LotusPaperAniClip[7], strLotusAniTriggerName[7], 7));
                }
                else if (_bLotusStates[15])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[1], LotusPaperAni[6], LotusPaperAniClip[15], strLotusAniTriggerName[15], 15));
                }
                else if (_bLotusStates[23])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[1], LotusPaperAni[6], LotusPaperAniClip[23], strLotusAniTriggerName[23], 23));
                }
                break;
            case KeyCode.D:
                if (_bLotusStates[5])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[3], LotusPaperAni[5], LotusPaperAniClip[5], strLotusAniTriggerName[5], 5));
                }
                else if (_bLotusStates[6])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[6], LotusPaperAniClip[6], strLotusAniTriggerName[6], 6));
                }
                else if (_bLotusStates[14])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[6], LotusPaperAniClip[14], strLotusAniTriggerName[14], 14));
                }
                else if (_bLotusStates[22])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[6], LotusPaperAniClip[22], strLotusAniTriggerName[22], 22));
                }
                break;
            case KeyCode.Q:
                if (_bLotusStates[12])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[5], LotusPaperAni[6], LotusPaperAniClip[12], strLotusAniTriggerName[12], 12));
                }
                else if (_bLotusStates[20])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[5], LotusPaperAni[6], LotusPaperAniClip[20], strLotusAniTriggerName[20], 20));
                }
                else if (_bLotusStates[29])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[5], LotusPaperAni[6], LotusPaperAniClip[29], strLotusAniTriggerName[29], 29));
                }
                break;
            case KeyCode.E:
                if (_bLotusStates[13])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[3], LotusPaperAni[6], LotusPaperAniClip[13], strLotusAniTriggerName[13], 13));
                }
                else if (_bLotusStates[21])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[3], LotusPaperAni[6], LotusPaperAniClip[21], strLotusAniTriggerName[21], 21));
                }
                else if (_bLotusStates[26])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[6], LotusPaperAni[6], LotusPaperAniClip[26], strLotusAniTriggerName[26], 26));
                }
                break;
            case KeyCode.C:
                if (_bLotusStates[10])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[7], LotusPaperAni[6], LotusPaperAniClip[10], strLotusAniTriggerName[10], 10));
                }
                else if (_bLotusStates[18])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[7], LotusPaperAni[6], LotusPaperAniClip[18], strLotusAniTriggerName[18], 18));
                }
                else if (_bLotusStates[27])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[7], LotusPaperAni[6], LotusPaperAniClip[27], strLotusAniTriggerName[27], 27));
                }
                break;
            case KeyCode.Z:
                if (_bLotusStates[11])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[4], LotusPaperAni[6], LotusPaperAniClip[11], strLotusAniTriggerName[11], 11));
                }
                else if (_bLotusStates[19])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[4], LotusPaperAni[6], LotusPaperAniClip[19], strLotusAniTriggerName[19], 19));
                }
                else if (_bLotusStates[28])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[4], LotusPaperAni[6], LotusPaperAniClip[28], strLotusAniTriggerName[28], 28));
                }
                break;
        }
    }

    IEnumerator ProcessAnimator(Sprite sprite, Animator ani, AnimationClip clip, string strTriggerName, int iStateIndex)
    {
        _isAnimating = true;
        HintObj.SetActive(false);
        HintImg.sprite = sprite;

        ani.SetTrigger(strTriggerName);

        yield return new WaitForSeconds(clip.length + 0.2f);

        CheckAniState(ani, strTriggerName, iStateIndex);
    }

    void CheckAniState(Animator ani, string strTriggerName, int iStateIndex)
    {
        LotusState = ani.GetCurrentAnimatorStateInfo(0);

        if (_bLotusStates[29])
        {
            SceneController_Room sceneCtrlr = GameObject.Find("SceneController").GetComponent<SceneController_Room>();
            sceneCtrlr.SendMessage("ExitLotusGame");
            return;
        }

        // 動畫完整結束
        if (LotusState.IsName(strTriggerName) && LotusState.normalizedTime >= 1.0f)
        {
            _bLotusStates[iStateIndex] = false;
            _bLotusStates[iStateIndex + 1] = true;
            this._currentState = iStateIndex + 1;
            _isAnimating = false;
        }

        // 設定 Lotus 物件座標
        for (int lotusIndex = 1; lotusIndex < 7; lotusIndex++)
        {
            if (_bLotusStates[lotusIndex])
            {
                // Hide
                LotusPaperObj[lotusIndex - 1].transform.localPosition = new Vector3(0f, -1f, 0f);

                // Show
                LotusPaperObj[lotusIndex].transform.localPosition = this._showLocation[lotusIndex];
                LotusPaperObj[lotusIndex].transform.localRotation = Quaternion.Euler(this._showRotation[lotusIndex]);
            }
        }

        HintObj.SetActive(true);
    }

    Vector2 UIHintPosition(int iStateIndex)
    {
        return iStateIndex switch
        {
            0 => new Vector2(0, 400),      // W
            1 => new Vector2(0, -400),     // S
            2 => new Vector2(-500, 0),     // A
            3 => new Vector2(0, 400),      // W
            4 => new Vector2(0, -400),     // S
            5 => new Vector2(500, 0),      // D
            6 => new Vector2(500, 0),      // D
            7 => new Vector2(0, -400),     // S
            8 => new Vector2(-500, 0),     // A
            9 => new Vector2(0, 400),      // W
            10 => new Vector2(300, -300),  // C
            11 => new Vector2(-300, -300), // Z
            12 => new Vector2(-300, 300),  // Q
            13 => new Vector2(300, 300),   // E
            14 => new Vector2(500, 0),     // D
            15 => new Vector2(0, -400),    // S
            16 => new Vector2(-500, 0),    // A
            17 => new Vector2(0, 400),     // W
            18 => new Vector2(300, -300),  // C
            19 => new Vector2(-300, -300), // Z
            20 => new Vector2(-300, 300),  // Q
            21 => new Vector2(300, 300),   // E
            22 => new Vector2(500, 0),     // D
            23 => new Vector2(0, -400),    // S
            24 => new Vector2(-500, 0),    // A
            25 => new Vector2(0, 400),     // W
            26 => new Vector2(300, 300),   // E
            27 => new Vector2(300, -300),  // C
            28 => new Vector2(-300, -300), // Z
            29 => new Vector2(-300, 300),  // Q
            _ => new Vector2(0, 0),
        };
    }
    #endregion
}
