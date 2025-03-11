using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class OutSidePlayer : PlayerController
{

    public Image InToForestBlackImg;
    public Transform Mom;

    public override void Awake()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "森林傳送點")
        {
            this._bCanControl = false;
            StartCoroutine(InToForest());
        }
    }

    private IEnumerator InToForest()
    {
        StartCoroutine(Mom.GetComponent<Mom_Controller>().InToForest());
        yield return new WaitForSeconds(2.5f);
        InToForestBlackImg.DOFade(1f, 1)
                          .OnComplete(() => SceneManager.LoadScene("5 Forest_Scene"));
    }
}