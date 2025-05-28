using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ForestPlayer : PlayerController
{
    public string NowDirection = "向左";

    public bool canMove = true;
    public Transform TargetLeft;
    public Transform TargetRight;
    public Animation Ani;
    public Rigidbody TreeRig;
    public Animator DeadBodyAni;
    public Rigidbody DeadBodyRig;
    SceneController_Forest gameManager;

    public override void Awake()
    {
        base.Awake();
        if (gameManager == null)
            gameManager = GameObject.Find("_Scene01_Controller/SceneController").GetComponent<SceneController_Forest>();
        _bCursorShow = false;
        this._bCanControl = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (Ani.enabled == true) return;
        if (other.name == "左邊")
        {
            this._bCanControl = false;
            Ani.enabled = true;
            canMove = false;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 112.63f);
            if (NowDirection == "向左") gameManager.GoStraight();
            else
            {
                gameManager.GoBack();
                NowDirection = "向左";
            }
            tfTransform.DOMove(new Vector3(51f, 5.799085f, -9.55f), 0.5f).OnComplete(() =>
            {
                tfTransform.DORotate(new Vector3(0, -113.2f, 0), 1f);
                tfPlayerCamera.DOLocalRotate(new Vector3(42.5f, 0, 0), 1f).OnComplete(() =>
                {
                    StartCoroutine(PlayAnimation("Player_Forest_left"));
                }); 
            });
            print("進去左邊");
        }
        else if (other.name == "右邊")
        {
            this._bCanControl = false;
            Ani.enabled = true;
            canMove = false;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 112.63f);
            if (NowDirection == "向右") gameManager.GoStraight();
            else
            {
                gameManager.GoBack();
                NowDirection = "向右";
            }
            tfTransform.DOMove(new Vector3(50.468f, 5.799085f, 108.118f), 0.5f).OnComplete(() =>
            {
                tfTransform.DORotate(new Vector3(0, -116.6f, 0), 1f);
                tfPlayerCamera.DOLocalRotate(new Vector3(38.83f, 0, 0), 1f).OnComplete(() =>
                {
                    StartCoroutine(PlayAnimation("Player_Forest_Right"));
                });
            });
            print("進去右邊");
        }
        else if (other.name == "鬼魂")
        {
            this._bCanControl = false;
            SceneManager.LoadScene("5 Forest_Scene");
        }
        else if (other.name == "樹枝掉下觸發器")
        {
             //播放樹枝掉下動畫
             TreeRig.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
        else if (other.name == "屍體掉下觸發器")
        {
            //播放屍體掉下動畫
            DeadBodyAni.enabled = false;
            DeadBodyRig.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.F) && gameManager.FinsihFirstDia == true)
        {
            if (FlashLight.enabled == true) FlashLight.enabled = false;
            else FlashLight.enabled = true;
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
            tfTransform.rotation = Quaternion.Euler(0, 36.8f, 0);
            tfPlayerCamera.localRotation = Quaternion.Euler(8.3f, 0, 0);
        } 
        else
        {
            tfTransform.rotation = Quaternion.Euler(0, -220.95f, 0);
            tfPlayerCamera.localRotation = Quaternion.Euler(7.5f, 0, 0);
        }

        this._bCanControl = true;
    }
}