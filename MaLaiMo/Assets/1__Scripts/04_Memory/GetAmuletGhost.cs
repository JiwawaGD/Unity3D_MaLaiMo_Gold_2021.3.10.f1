using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAmuletGhost : MonoBehaviour
{
    public GrandmaRoom_Memory_Player Player;
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
        var random = Random.Range(3.0f, 7.0f);
        yield return new WaitForSeconds(random);

        neck.DORotate(new Vector3(-50, 0, 0), 1)
            .OnComplete(() => {
                rasingHead = true;
                StartCoroutine(setRandomGhostHeadDown());
            });

    }

    public IEnumerator setRandomGhostHeadDown()
    {
        var rasingHeadTime = Random.Range(6.0f, 15.0f);
        yield return new WaitForSeconds(rasingHeadTime);
        rasingHead = false;
        halfOpenTimer = 0;

        neck.DORotate(new Vector3(orgNeckRotationX, 0, 0), 1)
            .OnComplete(() => {
                if (Player.dead == false && Player.getAmulet() == false) StartCoroutine(setRandomGhostSeePlayer());
            });

    }

    public void Update()
    {
        if (rasingHead == false) return;
        if (Player.eyeStates == "open") PlayerDead();
        else if (Player.eyeStates == "helfOpen")
        {
            halfOpenTimer += Time.deltaTime;
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
