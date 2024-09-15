using UnityEngine;
using System.Collections;

public class TestPlayer : MonoBehaviour
{
    float fMoveSpeed = 5.0f;
    float fMouseSensitivity = 100.0f;
    private string NowDirection = "向左";

    public Transform tfPlayerBody;
    public Transform tfPlayerCam;
    public bool canMove = true;
    public Transform TargetLeft;
    public Transform TargetRight;
    public TestManager TM;
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
            gameObject.transform.position = new Vector3(50.789f, 5.841f, -4.317f);
            gameObject.transform.LookAt(new Vector3(TargetRight.position.x, transform.position.y, TargetRight.position.z));
            if (NowDirection == "向左") TM.GoStraight();
            else
            {
                TM.GoBack();
                NowDirection = "向左";
            }
            StartCoroutine(PlayAnimation());
            print("進去左邊");
        }
        else if (other.name == "右邊")
        {
            canMove = false;
            gameObject.transform.position = new Vector3(50.35f, 5.841f, 106.155f);
            gameObject.transform.LookAt(new Vector3(TargetLeft.position.x, transform.position.y, TargetLeft.position.z));
            if (NowDirection == "向右") TM.GoStraight();
            else
            {
                TM.GoBack();
                NowDirection = "向右";
            }
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