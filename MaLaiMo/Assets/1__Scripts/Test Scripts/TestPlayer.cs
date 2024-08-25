using UnityEngine;
using System.Collections;

public class TestPlayer : MonoBehaviour
{
    float fMoveSpeed = 5.0f;
    float fMouseSensitivity = 100.0f;

    public Transform tfPlayerBody;
    public Transform tfPlayerCam;
    public bool canMove = true;
    float fCamRotation = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (canMove == true)
        {
            Move();
            View();
        } 
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


    void OnTriggerEnter(Collider other)
    {
        if (other.name == "左邊")
        {
            canMove = false;
            gameObject.transform.position = new Vector3(57.88f, 0.8f, 26.92f);
            StartCoroutine(PlayAnimation());
            print("進去左邊");
        }
        else if (other.name == "右邊")
        {
            canMove = false;
            gameObject.transform.position = new Vector3(59.4f, 0.8f, 63.71f);
            StartCoroutine(PlayAnimation());
            print("進去左邊");
        }
    }

    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(2f);
        canMove = true;
    }
}