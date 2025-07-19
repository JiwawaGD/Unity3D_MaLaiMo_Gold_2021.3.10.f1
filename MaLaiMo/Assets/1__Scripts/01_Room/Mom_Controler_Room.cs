using DG.Tweening;
using UnityEngine;


public class Mom_Controler_Room : MonoBehaviour
{

    public Transform[] steps;
    public GrandmaRoom_Player Player;
    private Animator Mom_Ani;

    public void GoOutFilialPietyCurtain()
    {
        transform.DOLookAt(steps[0].position, 0.5f)
        .OnComplete(() =>
        {
            Mom_Ani.SetBool("isWalking", true);
            transform.DOMove(steps[0].position, 5f);
        });
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "第一階段")
        {
            Mom_Ani.SetBool("isWalking", false);
            transform.DOLookAt(steps[1].position, 0.5f)
            .OnComplete(() =>
            {
                Mom_Ani.SetBool("isWalking", true);
                transform.DOMove(steps[1].position, 5f);
            });
        }
        else if (other.name == "第二階段")
        {
            Player._bCanControl = true;
            gameObject.SetActive(false);
        }

    }
}
