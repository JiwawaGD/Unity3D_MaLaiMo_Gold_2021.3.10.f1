using System;

using DG.Tweening;

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region < Fields >
    public float _fMouseSensitivity = 1.0f;

    [Header("視野 - 上下靈敏度")]
    public float m_fUDSensitivity = 230f;
    [Header("視野 - 左右靈敏度")]
    public float m_fRLSensitivity = 230f;

    public float fSensitivityAmplifier;

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

    [HideInInspector] public bool _bCursorShow;
    [HideInInspector] public bool _bCanControl = true;
    [HideInInspector] public bool _bRayOnItem;

    Vector3 v3_MovePos;

    public Transform tfPlayerCamera;
    public Transform tfTransform;
    public Rigidbody _rig;
    public CapsuleCollider _collider;
    RaycastHit hit;

    ItemController current_Item;
    ItemController last_Item;
    #endregion

    #region < Unity Hook >
    public virtual void Awake()
    {
        _rig = GetComponent<Rigidbody>();
        tfTransform = transform;

        if (tfPlayerCamera == null)
            tfPlayerCamera = GameObject.Find("Player Camera").transform;
    }

    public void Start()
    {
        originalCameraPosition = tfPlayerCamera.localPosition;
        InitValue();
        DefaultCursorState();
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
        if (_bCursorShow || !_bCanControl)
        {
            _rig.velocity = Vector3.zero;
            return;
        }

        // 播放走路音效
        if (isWalking)
        {
            // 音效未播放時播放音效
            if (!audioSource.isPlaying)
                PlaySound(walkingSound);
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
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(tfPlayerCamera.position, tfPlayerCamera.position + (tfPlayerCamera.forward * m_fRayLength));
    }
    #endregion

    #region < Internal Method >
    void InitValue()
    {
        fSensitivityAmplifier = GlobalDeclare.fSensitivity;

        if (fSensitivityAmplifier == 0)
            fSensitivityAmplifier = 0.5f;

        _bCursorShow = false;

        audioSource.volume = 1f;
        audioSource.loop = false;
    }

    void View()
    {
        float mouseX = Input.GetAxis("Mouse X") * m_fRLSensitivity * fSensitivityAmplifier * _fMouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * m_fUDSensitivity * fSensitivityAmplifier * _fMouseSensitivity;

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

    void Move()
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
            _rig.velocity = v3_zero;
            isWalking = false;
        }
    }

    /// <summary>
    /// 檢查射線是否打到物件
    /// </summary>
    void RayHitCheck()
    {
        _bRayOnItem = Physics.Raycast(tfPlayerCamera.position,     // Origin
                                       tfPlayerCamera.forward,      // Direction
                                       out hit,                     // RaycastHit
                                       m_fRayLength,                // RayLength
                                       ItemLayer);                  // ItemLayer);

        if (_bRayOnItem)
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

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="clip"></param>
    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    #endregion

    #region < API >
    public void DefaultCursorState()
    {
        _bCursorShow = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = _bCursorShow;
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        _fMouseSensitivity = sensitivity;
        // 在這裡處理滑鼠靈敏度的邏輯
        // 例如，更新相應的變數，調整滑鼠靈敏度
    }

    public void SetCursor()
    {
        _bCursorShow = !_bCursorShow;

        if (_bCursorShow)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = _bCursorShow;
    }

    public void MoveToTargetPosition(Vector3 r_v3TargetPos, Vector3 r_v3PlayerEndRotation, Vector3 r_v3CameraEndRotation, float r_fDuration, Action onCompleteCallback = null)
    {
        // 角色不可操控
        this._bCanControl = false;
        this._rig.useGravity = false;
        this._collider.enabled = false;

        // 建立一個 Tween 序列
        Sequence sequence = DOTween.Sequence();

        // 加入移動動畫
        sequence.Append(transform.DOMove(r_v3TargetPos, r_fDuration).SetEase(Ease.Linear));

        // 假設要旋轉到某個角度，這裡可以使用一個目標的旋轉角度 (例如: Vector3.zero 代表不旋轉)
        sequence.Join(transform.DORotate(r_v3PlayerEndRotation, r_fDuration, RotateMode.Fast));
        sequence.Join(tfPlayerCamera.DORotate(r_v3CameraEndRotation, r_fDuration, RotateMode.Fast));

        // 當動畫結束時，執行回呼函式（如果有的話）
        sequence.OnComplete(() =>
        {
            transform.position = r_v3TargetPos;

            // 執行回呼
            onCompleteCallback?.Invoke();
        });
    }

    #endregion
}
