using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ItemController : MonoBehaviour
{
    #region < Field >
    [Header("遊戲場景編號")]
    public LevelTypeID m_CurrentLevelID;

    [Header("遊戲事件")]
    public GameEventID EventID;

    [Header("是否可以無限觸發(裝飾物件)")]
    public bool bAlwaysActive;

    [Header("物件可提示範圍")]
    public float fHintRange;

    [HideInInspector]
    public bool bActive;

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
    #endregion

    #region < Internal Method >
    /// <summary>
    /// Find & Set Field Data
    /// </summary>
    void GetFields()
    {
        if (HintObj == null)
            HintObj = gameObject.transform.GetChild(0).GetChild(0).gameObject;

        if (tfHint == null)
            tfHint = HintObj.transform;

        if (InteractObj == null)
            InteractObj = gameObject.transform.GetChild(0).GetChild(1).gameObject;

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
    }

    void ItemDisable()
    {
        if (bAlwaysActive)
            return;

        SetItemInteractive(false);

        bActive = false;
        HintObj.SetActive(bActive);
        SetHintable(bActive);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
    #endregion
}