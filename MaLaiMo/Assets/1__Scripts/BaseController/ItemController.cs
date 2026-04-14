using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ItemController : MonoBehaviour
{
    #region < Field >
    [SerializeField]
    [Header("遊戲場景編號")]
    private LevelTypeID m_CurrentLevelID;

    [SerializeField]
    [Header("遊戲事件")]
    private GameEventID EventID;

    [SerializeField]
    [Header("物件可提示範圍")]
    private float fHintRange;

    [HideInInspector]
    public bool bActive;

    [Header("是否可以無限觸發(裝飾物件)")]
    public bool bAlwaysActive;
    public bool isOpen = false;
    #region UI
    GameObject HintObj;     // 眼睛 UI
    Transform tfHint;

    GameObject InteractObj; // 提示可互動 UI
    Transform tfInteract;
    #endregion

    SceneController SceneCtrlr;
    Transform tfPlayerCamera;
    Vector3 v3This;
    bool bShowHint;
    float fDistanceWithPlayer;
    #endregion

    #region < Unity Hook >
    void Awake()
    {
        GetFields();
    }

    void Start()
    {
        Initialize();
    }

    void FixedUpdate()
    {
        if (bShowHint)
        {
            tfHint.LookAt(tfPlayerCamera);

            fDistanceWithPlayer = Vector3.Distance(v3This, tfPlayerCamera.position);

            if (fDistanceWithPlayer <= fHintRange)
                HintObj.SetActive(true);
            else
                HintObj.SetActive(false);
        }
    }
    #endregion

    #region < API >
    public void SetItemInteractive(bool r_bShow)
    {
        InteractObj.SetActive(r_bShow);

        if (r_bShow)
            tfInteract.LookAt(tfPlayerCamera);
    }

    public void SetHintable(bool r_bShow)
    {
        gameObject.layer = r_bShow ? LayerMask.NameToLayer("InteractiveItem") : LayerMask.NameToLayer("Default");
        bShowHint = r_bShow;
    }

    public void SendGameEvent()
    {
        ItemDisable();
        SceneCtrlr.GameEvent(m_CurrentLevelID, EventID);
    }

    public void ItemDisable()
    {
        if (bAlwaysActive)
            return;

        SetItemInteractive(false);

        bActive = false;
        HintObj.SetActive(bActive);
        SetHintable(bActive);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void SetNewGameEventID(GameEventID newGameEventID)
    {
        this.EventID = newGameEventID;
    }

    public void SetAlwaysActive(bool alwaysActive)
    {
        this.bAlwaysActive = alwaysActive;
    }
    #endregion

    #region < Internal Method >
    /// <summary>
    /// Find & Set Field Data
    /// </summary>
    void GetFields()
    {
        if (HintObj == null)
            HintObj = transform.Find("InteractiveItem").Find("Hint").gameObject;

        if (tfHint == null)
            tfHint = HintObj.transform;

        if (InteractObj == null)
            InteractObj = transform.Find("InteractiveItem").Find("Interact").gameObject;

        if (tfInteract == null)
            tfInteract = InteractObj.transform;

        if (SceneCtrlr == null)
        {
            string strSceneCtrlrName = "";

            switch (m_CurrentLevelID)
            {
                case LevelTypeID.Lv1_GrandmaHouse:
                    strSceneCtrlrName = "_Scene01_Controller/SceneController";
                    break;
                case LevelTypeID.Lv2_OutSideDoor:
                    strSceneCtrlrName = "_Scene02_Controller/SceneController";
                    break;
                case LevelTypeID.Lv3_GrandmaHouse_Memory:
                    strSceneCtrlrName = "_Scene01_Controller/SceneController";
                    break;
                case LevelTypeID.BeginScene:
                case LevelTypeID.Introduce:
                default:
                    break;
            }

            SceneCtrlr = GameObject.Find(strSceneCtrlrName).GetComponent<SceneController>();
        }

        if (tfPlayerCamera == null)
            tfPlayerCamera = GameObject.Find("_Common_Player/LingLing/Player Camera").transform;
    }

    /// <summary>
    /// Init Value
    /// </summary>
    void Initialize()
    {
        gameObject.layer = LayerMask.NameToLayer("InteractiveItem");
        v3This = transform.position;

        this.HintObj.SetActive(false);
        this.InteractObj.SetActive(false);
    }
    #endregion
}
