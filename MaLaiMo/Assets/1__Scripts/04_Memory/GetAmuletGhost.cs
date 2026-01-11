using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAmuletGhost : MonoBehaviour
{
    public GrandmaRoom_Memory_Player Player;
    public float halfOpenTimer;
    public bool rasingHead;

    private void Start()
    {
        
    }

    public IEnumerator setRandomGhostSeePlayer()
    {
        var random = Random.Range(3.0f, 7.0f);
        yield return new WaitForSeconds(random);
        rasingHead = true;
        var rasingHeadTime = Random.Range(3.0f, 10.0f);
        //抬頭動畫
        if (Player.eyeStates == "open")
        {
            PlayerDead();
            yield return null;
        }
        yield return new WaitForSeconds(rasingHeadTime);
        rasingHead = false;
        halfOpenTimer = 0;
        //低頭動畫
    }

    public void Update()
    {
        if (Player.eyeStates == "halfOpen" && rasingHead)
        {
            halfOpenTimer += Time.deltaTime;
            if (halfOpenTimer >= 3) PlayerDead();
        }
    }

    public void PlayerDead()
    {
        Player.dead = true;
        Player.openEyes();
        //抓玩家動畫
    }
}
