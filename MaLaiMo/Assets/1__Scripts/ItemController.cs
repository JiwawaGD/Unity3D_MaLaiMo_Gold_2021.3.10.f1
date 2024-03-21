using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ItemController : MonoBehaviour
{
    [Header("遊戲場景編號")]
    public SceneTypeID sceneID;

    [Header("遊戲事件")]
    public int iEventID;

    [Header("是否可以無限觸發(裝飾物件)")]
    public bool bAlwaysActive;

    [Header("物件可提示範圍")]
    public float fHintRange;

    [HideInInspector] // 是否可以觸發
    public bool bActive;

    #region UI
    GameObject HintObj;     // 眼睛 UI
    Transform tfHint;

    GameObject InteractObj; // 提示可互動 UI
    Transform tfInteract;
    #endregion

    GameManager gameManager;
    Transform tfPlayerCamera;
    Vector3 v3This;
    bool bShowHint;
    float fDistanceWithPlayer;

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
            //fDistanceWithPlayer = Vector3.SqrMagnitude(v3This - tfPlayerCamera.position);   //待確認   

            // 先改回這個方法 (某些道具會有無法跳出眼睛的狀況)
            fDistanceWithPlayer = Vector3.Distance(v3This, tfPlayerCamera.position);    //原始使用

            if (fDistanceWithPlayer <= fHintRange)
                HintObj.SetActive(true);
            else
                HintObj.SetActive(false);
        }
    }

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

        if (gameManager == null)
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (tfPlayerCamera == null)
            tfPlayerCamera = GameObject.Find("Player Camera").transform;
    }

    void Initialize()
    {
        gameObject.layer = LayerMask.NameToLayer("InteractiveItem");
        v3This = transform.position;
        //fHintRange = 3f;
    }

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
        gameManager.GameEvent(sceneID, (byte)iEventID);
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
}