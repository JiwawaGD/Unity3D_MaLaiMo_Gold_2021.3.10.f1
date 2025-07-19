using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Mom_Controller_OutSide : MonoBehaviour
{
    public Transform player;

    public float rotationSpeed = 5f;
    public Transform[] Step;
    public SkinnedMeshRenderer cloth;
    public SkinnedMeshRenderer eyes;
    public SkinnedMeshRenderer hair_back;
    public SkinnedMeshRenderer hair_forward;
    public int AFKTimeCount = 0;

    public Animator Mon_Animator;
    private float maxDistance = 5f;
    private bool isWaiting = true;
    private bool CanMove = true;
    private bool Finish = false;
    private Transform CurrentStep;

    void Start()
    {
        CurrentStep = Step[0];
    }


    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (CanMove == false) return;
        if (distance > maxDistance && Finish == false)
        {
            if (isWaiting == false)
            {
                isWaiting = true;
               Mon_Animator.SetBool("isWalking", false);
                MomLookAt(player, 0.5f);
            }
            else transform.LookAt(player.position);
            AFKTimeCount++;
        }
        else 
        {
            if (isWaiting == true)
            {
                isWaiting = false;
                MomLookAt(CurrentStep, 0.5f);
                Mon_Animator.SetBool("isWalking", true);
            }
            transform.Translate(Vector3.forward * 3.5f * Time.deltaTime);
            AFKTimeCount = 0;
        }
    }
    //媽媽轉向玩家
    void MomLookAt(Transform target,float time)
    {
        CanMove = false;
        transform.DOLookAt(target.position, time)
                 .OnComplete(() =>
                 {
                     CanMove = true;
                 });
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "第一階段")
        {
            CanMove = false;
            CurrentStep = Step[1];
            MomLookAt(CurrentStep, 0.5f);
        }
        else if (other.name == "第一階段(加距離)")
        {
            maxDistance = 15f;
        }
        else if (other.name == "第二階段")
        {
            CanMove = false;
            CurrentStep = Step[2];
            maxDistance = 5f;
            MomLookAt(CurrentStep, 0.5f);
        }
        else if (other.name == "第三階段")
        {
            CanMove = false;
            CurrentStep = Step[3];
            maxDistance = 15f;
            MomLookAt(CurrentStep, 0.1f);
        }
        else if (other.name == "第四階段")
        {
            CurrentStep = Step[4];
            maxDistance = 0f;
        }
    }

    public IEnumerator InToForest()
    {
        Finish = true;
        CanMove = false;
        MomLookAt(CurrentStep, 0.5f);
        yield return new WaitForSeconds(1f);
        cloth.materials[0].DOColor(new Color(1, 1, 1, 0), "_BaseColor", 2);
        eyes.materials[0].DOColor(new Color(1, 1, 1, 0), "_BaseColor", 2);
        hair_back.materials[0].DOColor(new Color(1, 1, 1, 0), "_BaseColor", 2);
        hair_forward.materials[0].DOColor(new Color(1, 1, 1, 0), "_BaseColor", 2)
                                 .OnComplete(() => gameObject.SetActive(false));
    }
}
