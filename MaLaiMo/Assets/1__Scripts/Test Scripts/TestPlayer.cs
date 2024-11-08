using UnityEngine;
using System.Collections;
using DG.Tweening;

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
    public Animation Ani;

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
        if (Ani.enabled == true) return;

        if (other.name == "左邊")
        {
            Ani.enabled = true;
            canMove = false;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 112.63f);
            if (NowDirection == "向左") TM.GoStraight();
            else
            {
                TM.GoBack();
                NowDirection = "向左";
            }
            tfPlayerBody.DOMove(new Vector3(51f, 5.799085f, -9.55f), 0.5f).OnComplete(() =>
            {
                tfPlayerBody.DORotate(new Vector3(0, -113.2f, 0), 1f);
                tfPlayerCam.DOLocalRotate(new Vector3(42.5f, 0, 0), 1f).OnComplete(() =>
                {
                    StartCoroutine(PlayAnimation("Player_Forest_left"));
                }); 
            });
            print("進去左邊");
        }
        else if (other.name == "右邊")
        {
            Ani.enabled = true;
            canMove = false;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 112.63f);
            if (NowDirection == "向右") TM.GoStraight();
            else
            {
                TM.GoBack();
                NowDirection = "向右";
            }
            tfPlayerBody.DOMove(new Vector3(50.468f, 5.799085f, 108.118f), 0.5f).OnComplete(() =>
            {
                tfPlayerBody.DORotate(new Vector3(0, -116.6f, 0), 1f);
                tfPlayerCam.DOLocalRotate(new Vector3(38.83f, 0, 0), 1f).OnComplete(() =>
                {
                    StartCoroutine(PlayAnimation("Player_Forest_Right"));
                });
            });
            print("進去右邊");
        }
    }

    IEnumerator PlayAnimation(string direction)
    {
        yield return new WaitForSeconds(0.1f);
        Ani.PlayQueued(direction);
        yield return new WaitForSeconds(6.2f);
        Ani.enabled = false;
        canMove = true;
        if (direction == "Player_Forest_left")
        {
            tfPlayerBody.rotation = Quaternion.Euler(0, 36.8f, 0);
            tfPlayerCam.localRotation = Quaternion.Euler(8.3f, 0, 0);
        } 
        else
        {
            tfPlayerBody.rotation = Quaternion.Euler(0, -220.95f, 0);
            tfPlayerCam.localRotation = Quaternion.Euler(7.5f, 0, 0);
        }
    }
}