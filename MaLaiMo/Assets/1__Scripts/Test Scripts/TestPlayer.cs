using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    float fMoveSpeed = 5.0f;
    float fMouseSensitivity = 100.0f;

    public Transform tfPlayerBody;
    public Transform tfPlayerCam;

    float fCamRotation = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        View();
    }

    void Move()
    {
        float fMoveHorizontal = Input.GetAxis("Horizontal");
        float fMoveVertical = Input.GetAxis("Vertical");

        Vector3 v3Movement = tfPlayerBody.right * fMoveHorizontal + tfPlayerBody.forward * fMoveVertical;
        tfPlayerBody.Translate(v3Movement * fMoveSpeed * Time.deltaTime, Space.World);
    }

    void View()
    {
        float fMouseX = Input.GetAxis("Mouse X") * fMouseSensitivity * Time.deltaTime;
        float fMouseY = Input.GetAxis("Mouse Y") * fMouseSensitivity * Time.deltaTime;

        fCamRotation -= fMouseY;
        fCamRotation = Mathf.Clamp(fCamRotation, -90.0f, 90.0f);

        tfPlayerCam.localRotation = Quaternion.Euler(fCamRotation, 0.0f, 0.0f);
        tfPlayerBody.Rotate(Vector3.up * fMouseX);
    }
}