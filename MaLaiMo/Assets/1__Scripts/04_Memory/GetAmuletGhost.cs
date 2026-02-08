using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAmuletGhost : MonoBehaviour
{
    public GrandmaRoom_Memory_Player Player;
    public CanvasGroup playerBlood;
    public float halfOpenTimer;
    public bool rasingHead;
    public Transform neck;
    private float orgNeckRotationX;
    private void Start()
    {
        orgNeckRotationX = neck.eulerAngles.x;
    }

    public IEnumerator setRandomGhostSeePlayer()
    {
        var random = Random.Range(1.0f, 7.0f);
        yield return new WaitForSeconds(random);
        var setIsRasingHead = Random.Range(1.0f, 10.0f);
        if (setIsRasingHead > 4)
        {
            neck.DOLocalRotate(new Vector3(-58f, -36, 34), 1)
                .OnComplete(() =>
                {
                    rasingHead = true;
                    StartCoroutine(setRandomGhostHeadDown());
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
                StartCoroutine(setRandomGhostHeadDown());
            });
        }
    }

    public IEnumerator setRandomGhostHeadDown()
    {
        var rasingHeadTime = Random.Range(6.0f, 15.0f);
        yield return new WaitForSeconds(rasingHeadTime);
        rasingHead = false;
        halfOpenTimer = 0;
        playerBlood.DOFade(0f, 1f);
        neck.DOLocalRotate(new Vector3(orgNeckRotationX, 0, 0), 1)
            .OnComplete(() => {
                if (Player.dead == false && Player.getAmulet() == false) StartCoroutine(setRandomGhostSeePlayer());
            });

    }

    public void Update()
    {
        if (rasingHead == false || Player.dead == true) return;
        if (Player.eyeStates == "open") PlayerDead();
        else if (Player.eyeStates == "helfOpen")
        {
            halfOpenTimer += Time.deltaTime;
            playerBlood.alpha = Mathf.Clamp01(halfOpenTimer / 3);
            if (halfOpenTimer >= 3) PlayerDead();
        }
    }

    public void PlayerDead()
    {
        Player.dead = true;
        Player.openEyes();
        print("死了");
        //抓玩家動畫
    }
}
