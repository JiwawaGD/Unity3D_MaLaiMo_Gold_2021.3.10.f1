using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmaRoom_Memory_Player : PlayerController
{
    public bool dead = false;
    public string eyeStates = "";
    public Transform[] Eyes;
    public static bool _getAmulet = false;
    public GameObject AmuletHint;
    public bool getAmulet()
    {
        return _getAmulet;
    }
    public override void Update()
    {
        base.Update();
        if (dead) return;
        if(eyeStates != "")
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) openEyes();
            else if (Input.GetKeyDown(KeyCode.Alpha2)) helfOpenEyes();
            else if (Input.GetKeyDown(KeyCode.Alpha3)) closeEyes();
        }
        if(eyeStates == "open" && tfPlayerCamera.localPosition.x <= 1f) tfPlayerCamera.position += Vector3.right * 0.05f * Time.deltaTime;
        else if (tfPlayerCamera.localPosition.x > 0.9) AmuletHint.SetActive(true); 
        if(_getAmulet == false && tfPlayerCamera.localPosition.x > 1 && Input.GetKeyDown(KeyCode.E))
        {
            _getAmulet = true;
            AmuletHint.transform.parent.parent.gameObject.SetActive(false);
        }
    }

    public void openEyes()
    {
        eyeStates = "open";
        Eyes[0].DOLocalMoveY(1050f, 0.2f);
        Eyes[1].DOLocalMoveY(-1050f, 0.2f);
    }
    private void helfOpenEyes()
    {
        eyeStates = "helfOpen";
        Eyes[0].DOLocalMoveY(650f, 0.2f);
        Eyes[1].DOLocalMoveY(-650f, 0.2f);
    }
    public void closeEyes()
    {
        eyeStates = "close";
        Eyes[0].DOLocalMoveY(250f, 0.2f);
        Eyes[1].DOLocalMoveY(-250f, 0.2f);
    }

}
