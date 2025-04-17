using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

enum HintType
{
    Up,         // W
    Left,       // A
    Down,       // S
    Right,      // D
    UpperLeft,  // Q
    UpperRight, // E
    DownLeft,   // Z
    DownRight,  // C
}

public class LotusGameManager : MonoBehaviour
{
    #region < Field >
    [SerializeField] [Header("場景控制器")] SceneController_Room _sceneController;

    [SerializeField] [Header("七個蓮花紙 - 物件")] GameObject[] LotusPaperObj;
    [SerializeField] [Header("七個蓮花紙 - 動畫控制器")] Animator[] LotusPaperAni;
    [SerializeField] [Header("七個蓮花紙 - 表演座標")] Vector3[] _showLocation;
    [SerializeField] [Header("七個蓮花紙 - 表演角度")] Vector3[] _showRotation;

    [SerializeField] [Header("七個蓮花紙 - 全部動畫片段")] AnimationClip[] LotusPaperAniClip;

    [SerializeField] [Header("提示按鈕 - 物件")] GameObject HintObj;
    [SerializeField] [Header("八個方向的提示 - 圖片")] Sprite[] HintSprite;


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

    private HintType _nextHintType;

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

    public void SetHintPosition()
    {
        SetHintPosition(HintType.Up);
        HintImg.sprite = HintSprite[(int)HintType.Up];

        this.HintObj.SetActive(true);
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
                    StartCoroutine(ProcessAnimator(2, LotusPaperAni[0], LotusPaperAniClip[0], strLotusAniTriggerName[0], 0));
                }
                else if (_bLotusStates[3])
                {
                    StartCoroutine(ProcessAnimator(2, LotusPaperAni[3], LotusPaperAniClip[3], strLotusAniTriggerName[3], 3));
                }
                else if (_bLotusStates[9])
                {
                    StartCoroutine(ProcessAnimator(6, LotusPaperAni[6], LotusPaperAniClip[9], strLotusAniTriggerName[9], 9));
                }
                else if (_bLotusStates[17])
                {
                    StartCoroutine(ProcessAnimator(6, LotusPaperAni[6], LotusPaperAniClip[17], strLotusAniTriggerName[17], 17));
                }
                else if (_bLotusStates[25])
                {
                    StartCoroutine(ProcessAnimator(5, LotusPaperAni[6], LotusPaperAniClip[25], strLotusAniTriggerName[25], 25));
                }
                break;
            case KeyCode.A:
                if (_bLotusStates[2])
                {
                    StartCoroutine(ProcessAnimator(0, LotusPaperAni[2], LotusPaperAniClip[2], strLotusAniTriggerName[2], 2));
                }
                else if (_bLotusStates[8])
                {
                    StartCoroutine(ProcessAnimator(0, LotusPaperAni[6], LotusPaperAniClip[8], strLotusAniTriggerName[8], 8));
                }
                else if (_bLotusStates[16])
                {
                    StartCoroutine(ProcessAnimator(0, LotusPaperAni[6], LotusPaperAniClip[16], strLotusAniTriggerName[16], 16));
                }
                else if (_bLotusStates[24])
                {
                    StartCoroutine(ProcessAnimator(0, LotusPaperAni[6], LotusPaperAniClip[24], strLotusAniTriggerName[24], 24));
                }
                break;
            case KeyCode.S:
                if (_bLotusStates[1])
                {
                    StartCoroutine(ProcessAnimator(1, LotusPaperAni[1], LotusPaperAniClip[1], strLotusAniTriggerName[1], 1));
                }
                else if (_bLotusStates[4])
                {
                    StartCoroutine(ProcessAnimator(3, LotusPaperAni[4], LotusPaperAniClip[4], strLotusAniTriggerName[4], 4));
                }
                else if (_bLotusStates[7])
                {
                    StartCoroutine(ProcessAnimator(1, LotusPaperAni[6], LotusPaperAniClip[7], strLotusAniTriggerName[7], 7));
                }
                else if (_bLotusStates[15])
                {
                    StartCoroutine(ProcessAnimator(1, LotusPaperAni[6], LotusPaperAniClip[15], strLotusAniTriggerName[15], 15));
                }
                else if (_bLotusStates[23])
                {
                    StartCoroutine(ProcessAnimator(1, LotusPaperAni[6], LotusPaperAniClip[23], strLotusAniTriggerName[23], 23));
                }
                break;
            case KeyCode.D:
                if (_bLotusStates[5])
                {
                    StartCoroutine(ProcessAnimator(3, LotusPaperAni[5], LotusPaperAniClip[5], strLotusAniTriggerName[5], 5));
                }
                else if (_bLotusStates[6])
                {
                    StartCoroutine(ProcessAnimator(2, LotusPaperAni[6], LotusPaperAniClip[6], strLotusAniTriggerName[6], 6));
                }
                else if (_bLotusStates[14])
                {
                    StartCoroutine(ProcessAnimator(2, LotusPaperAni[6], LotusPaperAniClip[14], strLotusAniTriggerName[14], 14));
                }
                else if (_bLotusStates[22])
                {
                    StartCoroutine(ProcessAnimator(2, LotusPaperAni[6], LotusPaperAniClip[22], strLotusAniTriggerName[22], 22));
                }
                break;
            case KeyCode.Q:
                if (_bLotusStates[12])
                {
                    StartCoroutine(ProcessAnimator(5, LotusPaperAni[6], LotusPaperAniClip[12], strLotusAniTriggerName[12], 12));
                }
                else if (_bLotusStates[20])
                {
                    StartCoroutine(ProcessAnimator(5, LotusPaperAni[6], LotusPaperAniClip[20], strLotusAniTriggerName[20], 20));
                }
                else if (_bLotusStates[29])
                {
                    StartCoroutine(ProcessAnimator(5, LotusPaperAni[6], LotusPaperAniClip[29], strLotusAniTriggerName[29], 29));
                }
                break;
            case KeyCode.E:
                if (_bLotusStates[13])
                {
                    StartCoroutine(ProcessAnimator(3, LotusPaperAni[6], LotusPaperAniClip[13], strLotusAniTriggerName[13], 13));
                }
                else if (_bLotusStates[21])
                {
                    StartCoroutine(ProcessAnimator(3, LotusPaperAni[6], LotusPaperAniClip[21], strLotusAniTriggerName[21], 21));
                }
                else if (_bLotusStates[26])
                {
                    StartCoroutine(ProcessAnimator(6, LotusPaperAni[6], LotusPaperAniClip[26], strLotusAniTriggerName[26], 26));
                }
                break;
            case KeyCode.C:
                if (_bLotusStates[10])
                {
                    StartCoroutine(ProcessAnimator(7, LotusPaperAni[6], LotusPaperAniClip[10], strLotusAniTriggerName[10], 10));
                }
                else if (_bLotusStates[18])
                {
                    StartCoroutine(ProcessAnimator(7, LotusPaperAni[6], LotusPaperAniClip[18], strLotusAniTriggerName[18], 18));
                }
                else if (_bLotusStates[27])
                {
                    StartCoroutine(ProcessAnimator(7, LotusPaperAni[6], LotusPaperAniClip[27], strLotusAniTriggerName[27], 27));
                }
                break;
            case KeyCode.Z:
                if (_bLotusStates[11])
                {
                    StartCoroutine(ProcessAnimator(4, LotusPaperAni[6], LotusPaperAniClip[11], strLotusAniTriggerName[11], 11));
                }
                else if (_bLotusStates[19])
                {
                    StartCoroutine(ProcessAnimator(4, LotusPaperAni[6], LotusPaperAniClip[19], strLotusAniTriggerName[19], 19));
                }
                else if (_bLotusStates[28])
                {
                    StartCoroutine(ProcessAnimator(4, LotusPaperAni[6], LotusPaperAniClip[28], strLotusAniTriggerName[28], 28));
                }
                break;
        }
    }

    IEnumerator ProcessAnimator(int spriteIndex, Animator ani, AnimationClip clip, string strTriggerName, int iStateIndex)
    {
        this._isAnimating = true;

        HintObj.SetActive(false);

        HintType nextHintType = GetHintType(spriteIndex);
        SetHintPosition(nextHintType);
        HintImg.sprite = HintSprite[(int)nextHintType];

        ani.SetTrigger(strTriggerName);

        yield return new WaitForSeconds(clip.length + 0.2f);

        CheckAniState(ani, strTriggerName, iStateIndex);
    }

    void CheckAniState(Animator ani, string strTriggerName, int iStateIndex)
    {
        LotusState = ani.GetCurrentAnimatorStateInfo(0);

        if (_bLotusStates[5])
        {
            PlayChairMoveAnimation();
        }
        else if (_bLotusStates[17])
        {
            SetTVNoiseOn();
        }
        else if (_bLotusStates[29])
        {
            LotusGameFinish();
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

    HintType GetHintType(int spriteIndex)
    {
        return spriteIndex switch
        {
            0 => HintType.Up,
            1 => HintType.Left,
            2 => HintType.Down,
            3 => HintType.Right,
            4 => HintType.UpperLeft,
            5 => HintType.UpperRight,
            6 => HintType.DownLeft,
            7 => HintType.DownRight,
            _ => HintType.Up,
        };
    }

    void SetHintPosition(HintType hintType)
    {
        Vector3 hintPosition = Vector3.zero;

        switch (hintType)
        {
            case HintType.Up:
                hintPosition = new Vector3(0.04f, 0.45f, -0.13f);
                break;
            case HintType.Down:
                hintPosition = new Vector3(0.19f, 0.05f, -0.13f);
                break;
            case HintType.Left:
                hintPosition = new Vector3(0.25f, 0.25f, -0.35f);
                break;
            case HintType.Right:
                hintPosition = new Vector3(0.25f, 0.25f, 0.1f);
                break;
            case HintType.UpperLeft:
                hintPosition = new Vector3(0.04f, 0.38f, -0.3f);
                break;
            case HintType.UpperRight:
                hintPosition = new Vector3(0.04f, 0.38f, 0.02f);
                break;
            case HintType.DownLeft:
                hintPosition = new Vector3(0.18f, 0.1f, 0.06f);
                break;
            case HintType.DownRight:
                hintPosition = new Vector3(0.18f, 0.1f, -0.3f);
                break;
        }

        this.HintObj.transform.localPosition = hintPosition;
    }

    void PlayChairMoveAnimation()
    {
        Animation chairAnim = GameObject.Find("_Scene01_InteractItems/__Level_1/Lv1_rosewood_Chair").GetComponent<Animation>();
        chairAnim["chair_move"].time = 0f;
        chairAnim.PlayQueued("chair_move"); 
    }

    void SetTVNoiseOn()
    {
        this._sceneController.SetTVNoise();
    }

    void LotusGameFinish()
    {
        this.LotusPaperObj[6].transform.localPosition = new Vector3(0f, -1f, 0f);
        this._finishedLotusPaper.SetActive(true);
        this._sceneController.LotusGameFinish();
    }
    #endregion
}
