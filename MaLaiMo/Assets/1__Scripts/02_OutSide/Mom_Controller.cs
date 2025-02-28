using UnityEngine;
using DG.Tweening;

public class Mom_Controller : MonoBehaviour
{
    public Transform player;
    public float maxDistance = 15f;
    public float rotationSpeed = 5f;
    public Transform[] Step;

    public Animator Mon_Animator;
    private bool isWaiting = true;
    private bool Finish;
    private bool CanMove = true;
    private Transform CurrentStep;
    private Material material;

    void Start()
    {
        material = gameObject.GetComponent<MeshRenderer>().material;
        CurrentStep = Step[0];
    }


    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (CanMove == false) return;
        if (distance > maxDistance)
        {
            if (isWaiting == false)
            {
                isWaiting = true;
                //Mon_Animator.SetBool("isWalking", false);
                MomLookAt(player, 0.5f);
            }
        }
        else 
        {
            if (isWaiting == true)
            {
                isWaiting = false;
                MomLookAt(CurrentStep, 0.5f);
                //Mon_Animator.SetBool("isWalking", true);
            }
            transform.Translate(Vector3.forward * 3 * Time.deltaTime);
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
        else if (other.name == "第二階段")
        {
            CanMove = false;
            CurrentStep = Step[2];
            MomLookAt(CurrentStep, 0.5f);
        }
        else if (other.name == "第三階段")
        {
            CanMove = false;
            CurrentStep = Step[3];
            MomLookAt(CurrentStep, 0.1f);
            Color baseColor = material.GetColor("_BaseColor");  // HDRP / URP Shader 使用 _BaseColor
            material.DOColor(new Color(baseColor.r, baseColor.g, baseColor.b, 0), "_BaseColor", 2)
                    .OnComplete(() => gameObject.SetActive(false));
        }
    }

}
