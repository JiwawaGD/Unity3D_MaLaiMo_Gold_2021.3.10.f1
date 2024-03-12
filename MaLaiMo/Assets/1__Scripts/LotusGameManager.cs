using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LotusGameManager : MonoBehaviour
{
    [SerializeField] [Header("蓮花紙當前階段")] bool[] bLotusState;
    [SerializeField] [Header("蓮花紙 - 物件")] GameObject[] LotusPaperObj;
    [SerializeField] [Header("蓮花紙 - 動畫師")] Animator[] LotusPaperAni;
    [SerializeField] [Header("蓮花紙 - 動畫片段")] AnimationClip[] LotusPaperAniClip;
    [SerializeField] [Header("提示按鈕 - 物件")] GameObject HintObj;
    [SerializeField] [Header("上下左右 - 圖片")] Sprite[] HintSprite;
    [SerializeField] [Header("音效撥放器")] AudioSource lotusAudioSource;
    [SerializeField, Tooltip("金紙")] AudioClip[] goldPaper;

    [Header("蓮花 Canvas")] public GameObject LotusCanvas;

    int iAllLotusCount;
    bool bIsAnimating;

    Image HintImg;
    RectTransform HintRectTf;
    Transform TfLotus;
    AnimatorStateInfo LotusState;
    GameManager GM;

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

    void Awake()
    {
        HintRectTf = HintObj.GetComponent<RectTransform>();
        HintImg = HintObj.GetComponent<Image>();
    }

    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        iAllLotusCount = 30;

        for (int index = 0; index < iAllLotusCount; index++)
            bLotusState[index] = false;

        bLotusState[0] = true;
        TfLotus = LotusPaperObj[6].transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F7))
        {
            GM.ExitLotusGame();
            GlobalDeclare.bLotusGameComplete = true;
        }

        if (bIsAnimating)
            return;

        if (bIsGamePause)
            return;

        if (Input.GetKeyDown(KeyCode.W))
            PlayLotusAni(KeyCode.W);

        if (Input.GetKeyDown(KeyCode.A))
            PlayLotusAni(KeyCode.A);

        if (Input.GetKeyDown(KeyCode.S))
            PlayLotusAni(KeyCode.S);

        if (Input.GetKeyDown(KeyCode.D))
            PlayLotusAni(KeyCode.D);

        if (Input.GetKeyDown(KeyCode.Q))
            PlayLotusAni(KeyCode.Q);

        if (Input.GetKeyDown(KeyCode.E))
            PlayLotusAni(KeyCode.E);

        if (Input.GetKeyDown(KeyCode.C))
            PlayLotusAni(KeyCode.C);

        if (Input.GetKeyDown(KeyCode.Z))
            PlayLotusAni(KeyCode.Z);
    }

    void PlayLotusAni(KeyCode r_key)
    {
        switch (r_key)
        {
            case KeyCode.W:
                if (bLotusState[0])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[0], LotusPaperAniClip[0], strLotusAniTriggerName[0], 0));
                }
                else if (bLotusState[3])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[3], LotusPaperAniClip[3], strLotusAniTriggerName[3], 3));
                }
                else if (bLotusState[9])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[6], LotusPaperAni[6], LotusPaperAniClip[9], strLotusAniTriggerName[9], 9));
                }
                else if (bLotusState[17])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[6], LotusPaperAni[6], LotusPaperAniClip[17], strLotusAniTriggerName[17], 17));
                }
                else if (bLotusState[25])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[5], LotusPaperAni[6], LotusPaperAniClip[25], strLotusAniTriggerName[25], 25));
                }
                break;
            case KeyCode.A:
                if (bLotusState[2])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[0], LotusPaperAni[2], LotusPaperAniClip[2], strLotusAniTriggerName[2], 2));
                }
                else if (bLotusState[8])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[0], LotusPaperAni[6], LotusPaperAniClip[8], strLotusAniTriggerName[8], 8));
                }
                else if (bLotusState[16])
                {
                    GM.Lv1_CandleFall();
                    StartCoroutine(ProcessAnimator(HintSprite[0], LotusPaperAni[6], LotusPaperAniClip[16], strLotusAniTriggerName[16], 16));
                }
                else if (bLotusState[24])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[0], LotusPaperAni[6], LotusPaperAniClip[24], strLotusAniTriggerName[24], 24));
                }
                break;
            case KeyCode.S:
                if (bLotusState[1])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[1], LotusPaperAni[1], LotusPaperAniClip[1], strLotusAniTriggerName[1], 1));
                }
                else if (bLotusState[4])
                {
                    GM.Lv1_ShowTVWhiteNoise();
                    StartCoroutine(ProcessAnimator(HintSprite[3], LotusPaperAni[4], LotusPaperAniClip[4], strLotusAniTriggerName[4], 4));
                }
                else if (bLotusState[7])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[1], LotusPaperAni[6], LotusPaperAniClip[7], strLotusAniTriggerName[7], 7));
                }
                else if (bLotusState[15])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[1], LotusPaperAni[6], LotusPaperAniClip[15], strLotusAniTriggerName[15], 15));
                }
                else if (bLotusState[23])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[1], LotusPaperAni[6], LotusPaperAniClip[23], strLotusAniTriggerName[23], 23));
                }
                break;
            case KeyCode.D:
                if (bLotusState[5])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[3], LotusPaperAni[5], LotusPaperAniClip[5], strLotusAniTriggerName[5], 5));
                }
                else if (bLotusState[6])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[6], LotusPaperAniClip[6], strLotusAniTriggerName[6], 6));
                }
                else if (bLotusState[14])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[6], LotusPaperAniClip[14], strLotusAniTriggerName[14], 14));
                }
                else if (bLotusState[22])
                {
                    GM.Lv1_DollTurnAround();
                    StartCoroutine(ProcessAnimator(HintSprite[2], LotusPaperAni[6], LotusPaperAniClip[22], strLotusAniTriggerName[22], 22));
                }
                break;
            case KeyCode.Q:
                if (bLotusState[12])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[5], LotusPaperAni[6], LotusPaperAniClip[12], strLotusAniTriggerName[12], 12));
                }
                else if (bLotusState[20])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[5], LotusPaperAni[6], LotusPaperAniClip[20], strLotusAniTriggerName[20], 20));
                }
                else if (bLotusState[29])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[5], LotusPaperAni[6], LotusPaperAniClip[29], strLotusAniTriggerName[29], 29));
                }
                break;
            case KeyCode.E:
                if (bLotusState[13])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[3], LotusPaperAni[6], LotusPaperAniClip[13], strLotusAniTriggerName[13], 13));
                }
                else if (bLotusState[21])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[3], LotusPaperAni[6], LotusPaperAniClip[21], strLotusAniTriggerName[21], 21));
                }
                else if (bLotusState[26])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[6], LotusPaperAni[6], LotusPaperAniClip[26], strLotusAniTriggerName[26], 26));
                }
                break;
            case KeyCode.C:
                if (bLotusState[10])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[7], LotusPaperAni[6], LotusPaperAniClip[10], strLotusAniTriggerName[10], 10));
                }
                else if (bLotusState[18])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[7], LotusPaperAni[6], LotusPaperAniClip[18], strLotusAniTriggerName[18], 18));
                }
                else if (bLotusState[27])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[7], LotusPaperAni[6], LotusPaperAniClip[27], strLotusAniTriggerName[27], 27));
                }
                break;
            case KeyCode.Z:
                if (bLotusState[11])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[4], LotusPaperAni[6], LotusPaperAniClip[11], strLotusAniTriggerName[11], 11));
                }
                else if (bLotusState[19])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[4], LotusPaperAni[6], LotusPaperAniClip[19], strLotusAniTriggerName[19], 19));
                }
                else if (bLotusState[28])
                {
                    StartCoroutine(ProcessAnimator(HintSprite[4], LotusPaperAni[6], LotusPaperAniClip[28], strLotusAniTriggerName[28], 28));
                }
                break;
            default:
                break;
        }
    }

    IEnumerator ProcessAnimator(Sprite sprite, Animator ani, AnimationClip clip, string strTriggerName, int iStateIndex)
    {
        bIsAnimating = true;
        HintObj.SetActive(false);
        HintImg.sprite = sprite;

        ani.SetTrigger(strTriggerName);
        lotusAudioSource.PlayOneShot(goldPaper[Random.Range(0, 2)]);

        yield return new WaitForSeconds(clip.length + 0.2f);

        CheckAniState(ani, strTriggerName, iStateIndex);
    }

    void CheckAniState(Animator ani, string strTriggerName, int iStateIndex)
    {
        LotusState = ani.GetCurrentAnimatorStateInfo(0);

        if (bLotusState[29])
        {
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            gm.SendMessage("ExitLotusGame");
            GlobalDeclare.bLotusGameComplete = true;
            //gm.DestroyImmediateLo();
            return;
        }

        if (LotusState.IsName(strTriggerName) && LotusState.normalizedTime >= 1.0f)
        {
            bLotusState[iStateIndex] = false;
            bLotusState[iStateIndex + 1] = true;
            bIsAnimating = false;
        }

        for (int index = 1; index < 7; index++)
        {
            if (bLotusState[index])
            {
                LotusPaperObj[index - 1].transform.position = new Vector3(0, 0, 0);
                LotusPaperObj[index].transform.position = new Vector3(1, 0, 0);
            }
        }

        HintRectTf.anchoredPosition = UIHintPosition(iStateIndex + 1);
        HintObj.SetActive(true);
    }

    void RotateLotus()
    {
        for (int iCount = 0; iCount < 10; iCount++)
        {
            TfLotus.Rotate(0, -4.5f, 0);
        }
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

    void SetLotusCanvasEnable(bool bEnable)
    {
        LotusCanvas.SetActive(bEnable);
    }
}