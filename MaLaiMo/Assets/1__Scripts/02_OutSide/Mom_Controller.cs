using UnityEngine;
using DG.Tweening;

public class Mom_Controller : MonoBehaviour
{
    public Transform player;
    public float maxDistance = 20f;
    public float resumeDistance = 3f;
    public float rotationSpeed = 5f;
    public Transform[] Step;

    public Animator Mon_Animator;
    private bool isWaiting = true;
    private bool Finish;
    private bool CanMove = true;
    private Transform CurrentStep;
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = gameObject.GetComponent<MeshRenderer>().material;
        CurrentStep = Step[0];
        Color baseColor = material.GetColor("_BaseColor");  // HDRP / URP Shader 使用 _BaseColor
        material.DOColor(new Color(baseColor.r, baseColor.g, baseColor.b, 0), "_BaseColor", 2)
                .OnComplete(() => gameObject.SetActive(false));
    }

    // Update is called once per frame
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
                MomLookAt(player);
            }
        }
        else if (distance <= resumeDistance)
        {
            if (isWaiting == true)
            {
                isWaiting = false;
                MomLookAt(CurrentStep);
                //Mon_Animator.SetBool("isWalking", true);
            }
            transform.Translate(Vector3.forward * 3 * Time.deltaTime);
        }
    }
    //媽媽轉向玩家
    void MomLookAt(Transform target)
    {
        CanMove = false;
        transform.DOLookAt(target.position, 1)
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
            MomLookAt(CurrentStep);
        }
        else if (other.name == "第二階段")
        {
            CanMove = false;
            CurrentStep = Step[2];
            MomLookAt(CurrentStep);
        }
        else if (other.name == "第三階段")
        {
            print("跑完");
            Color baseColor = material.GetColor("_BaseColor");  // HDRP / URP Shader 使用 _BaseColor
            material.DOColor(new Color(baseColor.r, baseColor.g, baseColor.b, 0), "_BaseColor", 2)
                    .OnComplete(() => gameObject.SetActive(false));
        }
    }

}
