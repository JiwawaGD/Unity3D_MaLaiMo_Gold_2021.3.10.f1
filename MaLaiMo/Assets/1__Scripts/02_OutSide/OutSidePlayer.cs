using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class OutSidePlayer : PlayerController
{

    public Image InToForestBlackImg;
    public Transform Mom;

    public override void Awake()
    {
        transform.LookAt(Mom);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "森林傳送點")
        {
            _bCanControl = false;
            InToForestBlackImg.DOFade(1f, 1)
                              .OnComplete(() => SceneManager.LoadScene("5 Forest_Scene"));
        }
    }
}