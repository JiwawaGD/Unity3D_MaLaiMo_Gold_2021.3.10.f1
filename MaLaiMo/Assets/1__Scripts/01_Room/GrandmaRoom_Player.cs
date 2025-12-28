using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmaRoom_Player : PlayerController
{
    public string eyeStates = "";
    public Transform[] Eyes;
    public override void Update()
    {
        base.Update();
        if(eyeStates != "")
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) openEyes();
            else if (Input.GetKeyDown(KeyCode.Alpha2)) helfOpenEyes();
            else if (Input.GetKeyDown(KeyCode.Alpha3)) closeEyes();
        }
        if(eyeStates == "open") tfPlayerCamera.position += Vector3.right * 0.05f * Time.deltaTime;
    }

    private void openEyes()
    {
        eyeStates = "open";
        Eyes[0].DOLocalMoveY(1050f, 0.5f);
        Eyes[1].DOLocalMoveY(-1050f, 0.5f);
    }
    private void helfOpenEyes()
    {
        eyeStates = "helfOpen";
        Eyes[0].DOLocalMoveY(650f, 0.5f);
        Eyes[1].DOLocalMoveY(-650f, 0.5f);
    }
    private void closeEyes()
    {
        eyeStates = "close";
        Eyes[0].DOLocalMoveY(250f, 0.5f);
        Eyes[1].DOLocalMoveY(-250f, 0.5f);
    }
}
