using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public static float MouseSensitivity = 1.0f;
    public Transform ro_tfItemObj;

    [SerializeField]
    [Header("Mouse Settings")]
    private float smoothTime = 0.1f; // 添加平滑時間
    private float currentRotationVelocityX = 3f; // X軸當前速度
    private float currentRotationVelocityY = 0f; // Y軸當前速度
    private float targetRotationX = 0f; // 目標X軸旋轉
    private float targetRotationY = 0f; // 目標Y軸旋轉

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
    readonly float m_fMoveSpeed = 3f;
    readonly float m_fRayLength = 1.2f;
    readonly int m_iInteractiveLayer = 10;  // 互動圖層
    readonly Vector3 v3_zero = new Vector3(0, -9.8f, 0);

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

    #region Internal Function
    public virtual void Awake()
    {
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animation>();
        tfTransform = transform;

        if (tfPlayerCamera == null)
            tfPlayerCamera = GameObject.Find("Player Camera").transform;

    }
    public void Start()
    {
        originalCameraPosition = tfPlayerCamera.localPosition;
        InitValue();
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Update()
    {
        RayHitCheck();

        // 不在 UI 畫面時才可控制
        if (!SceneController.m_bInUIView)
        {
            if (Input.GetKeyDown(KeyCode.F6))
                SetCursor();
        }
    }
    public void FixedUpdate()
    {
        // 滑鼠顯示、無法控制時不可控制
        if (m_bCursorShow || !m_bCanControl)
        {
            rig.velocity = Vector3.zero;
            return;
        }

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

        View();
        Move();
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawLine(tfPlayerCamera.position, tfPlayerCamera.position + (tfPlayerCamera.forward * m_fRayLength));
    }
    #endregion

    public void DefaultCursorState()
    {
        m_bCursorShow = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = m_bCursorShow;
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        MouseSensitivity = sensitivity;
        // 在這裡處理滑鼠靈敏度的邏輯
        // 例如，更新相應的變數，調整滑鼠靈敏度
    }

    public void InitValue()
    {
        m_fUDSensitivity = 230;
        m_fRLSensitivity = 280;

        fSensitivityAmplifier = GlobalDeclare.fSensitivity;

        if (fSensitivityAmplifier == 0)
            fSensitivityAmplifier = 0.5f;

        m_bCursorShow = false;

        audioSource.volume = 1f;
        audioSource.loop = false;
    }

    public void View()
    {
        float mouseX = Input.GetAxis("Mouse X") * m_fRLSensitivity * fSensitivityAmplifier * MouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * m_fUDSensitivity * fSensitivityAmplifier * MouseSensitivity;

        // 計算目標旋轉值
        targetRotationX += mouseX * Time.deltaTime;
        targetRotationY += mouseY * Time.deltaTime; // 注意這裡是減法，因為我們要反轉Y軸

        // 限制垂直旋轉範圍
        targetRotationY = Mathf.Clamp(targetRotationY, m_fVerticalRotationRange.x, m_fVerticalRotationRange.y);

        if (m_bLimitRotation)
        {
            // 使用 SmoothDamp 實現平滑旋轉
            m_fHorizantalRotationValue = Mathf.SmoothDamp
                (
                    m_fHorizantalRotationValue,
                    targetRotationX,
                    ref currentRotationVelocityX,
                    smoothTime
                );

            m_fHorizantalRotationValue = Mathf.Clamp(
                m_fHorizantalRotationValue,
                m_fHorizantalRotationRange.x,
                m_fHorizantalRotationRange.y
            );

            tfTransform.localEulerAngles = Vector3.up * m_fHorizantalRotationValue;
        }
        else
        {
            // 平滑處理自由旋轉
            float smoothedRotationX = Mathf.SmoothDamp(
                tfTransform.eulerAngles.y,
                tfTransform.eulerAngles.y + mouseX * Time.deltaTime,
                ref currentRotationVelocityX,
                smoothTime
            );

            tfTransform.eulerAngles = new Vector3(
                tfTransform.eulerAngles.x,
                smoothedRotationX,
                tfTransform.eulerAngles.z
            );
        }

        // 攝影機垂直旋轉的平滑處理
        m_fVerticalRotationValue = Mathf.SmoothDamp(
            m_fVerticalRotationValue,
            targetRotationY,
            ref currentRotationVelocityY,
            smoothTime
        );

        tfPlayerCamera.localEulerAngles = -Vector3.right * m_fVerticalRotationValue;
    }

    public void Move() // 移動
    {
        float fMoveHorizontal = Input.GetAxis("Horizontal");
        float fMoveVertical = Input.GetAxis("Vertical");

        v3_MovePos = tfTransform.right * fMoveHorizontal + tfTransform.forward * fMoveVertical;

        //v3_MoveValue = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        //v3_MovePos.x = v3_MoveValue.x * Time.deltaTime * m_fMoveSpeed;
        //v3_MovePos.z = v3_MoveValue.z * Time.deltaTime * m_fMoveSpeed;

        //v3_MovePos = tfTransform.right * v3_MovePos.x + tfTransform.forward * v3_MovePos.z;

        if (v3_MovePos != Vector3.zero)
        {
            tfTransform.Translate(v3_MovePos * m_fMoveSpeed * Time.deltaTime, Space.World);
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
    public void RayHitCheck()  // 檢查射線是否打到物件
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

    public void PlaySound(AudioClip clip)  // 播放音效
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayWalkingSound() // 播放走路音效
    {
        PlaySound(walkingSound);
    }
}
