using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public static float MouseSensitivity = 1.0f;
    public Transform ro_tfItemObj;  // 旋轉物件

    private Vector3 originalCameraPosition; // 原始攝影機位置

    [SerializeField] [Header("Item 的圖層")] LayerMask ItemLayer;

    public AudioSource audioSource;     // 音效來源
    public AudioClip walkingSound;      // 走路音效

    private bool isWalking = false; // 是否正在走路

    // Can be setted by player
    float m_fUDSensitivity; // 上下轉速
    float m_fRLSensitivity; // 左右轉速
    public float fSensitivityAmplifier;

    // Const value  
    readonly float m_fMoveSpeed = 90f;
    readonly float m_fRayLength = 1.2f;
    readonly int m_iInteractiveLayer = 10;  // 互動圖層
    readonly Vector3 v3_zero = Vector3.zero;

    public bool m_bLimitRotation = false;
    float m_fHorizantalRotationValue;
    public Vector2 m_fHorizantalRotationRange;
    float m_fVerticalRotationValue;
    public Vector2 m_fVerticalRotationRange;

    [HideInInspector] public bool m_bCursorShow;
    [HideInInspector] public bool m_bCanControl;
    [HideInInspector] public bool m_bRayOnItem;

    Vector3 v3_MoveValue;
    Vector3 v3_MovePos;

    public Transform tfPlayerCamera;
    public Transform tfTransform;
    public Rigidbody rig;
    RaycastHit hit;
    Animation ani;

    ItemController current_Item;
    ItemController last_Item;
    SceneController gameManager;


    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animation>();
        tfTransform = transform;

        if (tfPlayerCamera == null)
            tfPlayerCamera = GameObject.Find("Player Camera").transform;

        if (gameManager == null)
            gameManager = GameObject.Find("__CONTROLLER/__GameManager").GetComponent<SceneController>();
    }

    void Start()
    {
        originalCameraPosition = tfPlayerCamera.localPosition;
        InitValue();
        SetCursor();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(tfPlayerCamera.position, tfPlayerCamera.position + (tfPlayerCamera.forward * m_fRayLength));
    }

    void Update()
    {
        RayHitCheck();

        // 不在 UI 畫面時才可控制
        if (!SceneController.m_bInUIView)
        {
            if (Input.GetKeyDown(KeyCode.F6))
                SetCursor();
        }
        else    // 在 UI 畫面時不可控制
        {
            if (Input.GetKeyDown(KeyCode.F6))
                SetCursor();
        }
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        MouseSensitivity = sensitivity;
        // 在這裡處理滑鼠靈敏度的邏輯
        // 例如，更新相應的變數，調整滑鼠靈敏度
    }
    void FixedUpdate()
    {
        // 暫時註解測試
        //// 滑鼠顯示、無法控制時不可控制
        //if (m_bCursorShow || !m_bCanControl)
        //{
        //    rig.velocity = Vector3.zero;
        //    return;
        //}

        //// 播放動畫時不可控制
        //if (ani.isPlaying)
        //{
        //    m_fVerticalRotationValue = 0;
        //    m_fHorizantalRotationValue = 0;
        //    rig.velocity = Vector3.zero;
        //    return;
        //}

        // 播放走路音效
        if (isWalking)
        {
            // 音效未播放時播放音效
            if (!audioSource.isPlaying)
                PlayWalkingSound();
        }
        else
        {
            // 音效播放時停止音效
            if (audioSource.isPlaying)
                audioSource.Stop();
        }

        Move();
        View();
    }

    void InitValue()
    {
        m_fUDSensitivity = 230;
        m_fRLSensitivity = 180;

        fSensitivityAmplifier = GlobalDeclare.fSensitivity;

        if (fSensitivityAmplifier == 0)
            fSensitivityAmplifier = 0.5f;

        m_bCursorShow = false;
        m_bCanControl = true;

        audioSource.volume = 1f;
        audioSource.loop = false;
    }

    void View()
    {
        // 左右轉 (只轉 *角色* )
        if (m_bLimitRotation)
        {
            m_fHorizantalRotationValue += Input.GetAxis("Mouse X") * m_fRLSensitivity * fSensitivityAmplifier * MouseSensitivity * Time.deltaTime;
            m_fHorizantalRotationValue = Mathf.Clamp(m_fHorizantalRotationValue, m_fHorizantalRotationRange.x, m_fHorizantalRotationRange.y);

            tfTransform.localEulerAngles = Vector3.up * m_fHorizantalRotationValue;
        }
        else
        {
            tfTransform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * m_fRLSensitivity * fSensitivityAmplifier * MouseSensitivity * Time.deltaTime);
        }

        // 上下轉 (只轉 *攝影機* )
        m_fVerticalRotationValue += Input.GetAxis("Mouse Y") * m_fUDSensitivity * fSensitivityAmplifier * MouseSensitivity * Time.deltaTime;
        m_fVerticalRotationValue = Mathf.Clamp(m_fVerticalRotationValue, -75, 75);

        tfPlayerCamera.localEulerAngles = -Vector3.right * m_fVerticalRotationValue;
    }

    void Move() // 移動
    {
        v3_MoveValue = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        v3_MovePos.x = v3_MoveValue.x * Time.deltaTime * m_fMoveSpeed;
        v3_MovePos.z = v3_MoveValue.z * Time.deltaTime * m_fMoveSpeed;

        v3_MovePos = tfTransform.right * v3_MovePos.x + tfTransform.forward * v3_MovePos.z;

        if (v3_MovePos != v3_zero)
        {
            rig.velocity = v3_MovePos;
            isWalking = true;
        }
        else
        {
            rig.velocity = v3_zero;
            isWalking = false;
        }
    }

    public void SetCursor()
    {
        m_bCursorShow = !m_bCursorShow;

        if (m_bCursorShow)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = m_bCursorShow;
    }

    // Ray check for item interact
    void RayHitCheck()  // 檢查射線是否打到物件
    {
        m_bRayOnItem = Physics.Raycast(tfPlayerCamera.position,     // Origin
                                       tfPlayerCamera.forward,      // Direction
                                       out hit,                     // RaycastHit
                                       m_fRayLength,                // RayLength
                                       ItemLayer);                  // ItemLayer);

        if (m_bRayOnItem)
        {
            current_Item = hit.transform.gameObject.GetComponent<ItemController>();

            if (current_Item.bActive)
            {
                current_Item.SetItemInteractive(true);

                last_Item = current_Item;

                if (Input.GetKeyDown(KeyCode.E))
                    current_Item.SendGameEvent();
            }
            else if (last_Item)
            {
                last_Item.SetItemInteractive(false);
            }
        }
        else
        {
            if (last_Item)
                last_Item.SetItemInteractive(false);
        }
    }

    void PlaySound(AudioClip clip)  // 播放音效
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlayWalkingSound() // 播放走路音效
    {
        PlaySound(walkingSound);
    }

}