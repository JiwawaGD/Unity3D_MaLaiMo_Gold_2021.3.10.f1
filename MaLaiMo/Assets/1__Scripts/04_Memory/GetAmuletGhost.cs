using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GetAmuletGhost : MonoBehaviour
{
    public GrandmaRoom_Memory_Player Player;
    public CanvasGroup playerBlood;
    public CanvasGroup playerBlack;
    public float halfOpenTimer;
    public bool rasingHead;
    public Transform neck;
    private float orgNeckRotationX;
    public Animator Animator;
    private void Start()
    {
        orgNeckRotationX = neck.eulerAngles.x;
    }

    public IEnumerator setRandomGhostSeePlayer()
    {
        var random = Random.Range(1.0f, 7.0f);
        yield return new WaitForSeconds(random);
        var setIsRasingHead = Random.Range(1.0f, 10.0f);
        if (setIsRasingHead > 3)
        {
            neck.DOLocalRotate(new Vector3(-58f, -36, 34), 1)
                .OnComplete(() =>
                {
                    if (Player.dead == false && Player.getAmulet() == false)
                    {
                        rasingHead = true;
                        StartCoroutine(setRandomGhostHeadDown());
                    }
                });
        }
        else
        {
            Vector3 orgRotate = transform.eulerAngles;
            Sequence seq = DOTween.Sequence();
            seq.Append(neck.DOLocalRotate(new Vector3(-5f, 0, 0), 1f));
            seq.Append(neck.DOLocalRotate(new Vector3(orgNeckRotationX, 0, 0), 1f));
            seq.OnComplete(() =>
            {
                if (Player.dead == false && Player.getAmulet() == false) StartCoroutine(setRandomGhostSeePlayer());
            });
        }
    }

    public IEnumerator setRandomGhostHeadDown()
    {
        var rasingHeadTime = Random.Range(6.0f, 15.0f);
        yield return new WaitForSeconds(rasingHeadTime);
        if (Player.dead == false && Player.getAmulet() == false)
        {
            rasingHead = false;
            halfOpenTimer = 0;
            playerBlood.DOFade(0f, 1f);
            neck.DOLocalRotate(new Vector3(orgNeckRotationX, 0, 0), 1)
                .OnComplete(() => {
                    if (Player.dead == false && Player.getAmulet() == false) StartCoroutine(setRandomGhostSeePlayer());
                });
        }
    }

    public void Update()
    {
        if (rasingHead == false || Player.dead == true) return;
        if (Player.eyeStates == "open") StartCoroutine(PlayerDead());
        else if (Player.eyeStates == "helfOpen")
        {
            halfOpenTimer += Time.deltaTime;
            playerBlood.alpha = Mathf.Clamp01(halfOpenTimer / 3);
            if (halfOpenTimer >= 3) StartCoroutine(PlayerDead());
        }
    }

    public IEnumerator PlayerDead()
    {
        Player.dead = true;
        Player.closeEyes();
        yield return new WaitForSeconds(0.25f);
        Animator.enabled = true;
        neck.eulerAngles = new Vector3(orgNeckRotationX, 0, 0);
        Animator.SetBool("PlayerDead", true);
        gameObject.transform.localPosition = new Vector3(-63.30074f, 0.578f, -17.84401f);
        yield return new WaitForSeconds(0.5f);
        Player.tfPlayerCamera.eulerAngles = new Vector3(-40.35f, 90, 0);
        Player.tfPlayerCamera.localPosition = Vector3.zero;
        Player.openEyes();
        yield return new WaitForSeconds(0.7f);
        //StartCoroutine(Player.struggle());
        Player.gameObject.GetComponent<Animation>().Play("HeadShake"); 
        playerBlood.DOFade(1, 5)
            .OnComplete(() => {
                playerBlack.DOFade(1, 5);
            });
        //抓玩家動畫
    }
}
