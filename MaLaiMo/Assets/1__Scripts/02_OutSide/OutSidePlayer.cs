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
        m_bCursorShow = false;
        m_bCanControl = true;
        transform.LookAt(Mom);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "森林傳送點")
        {
            InToForestBlackImg.DOFade(1f, 1);
                              //.OnComplete(() => SceneManager.LoadScene("5 Forest_Scene"));
        }
    }
}