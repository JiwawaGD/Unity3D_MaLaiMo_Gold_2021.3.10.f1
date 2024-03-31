using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("執行事項")]
    [Tooltip("{%wait: second\n" +
             "{%object: index; active; alpha(-1為None)\n" +
             "{%voice: index; volume\n" +
             "普通字串直接打")]
    public string[] ActionEvent;

    [Header("需處理物件")]
    public GameObject[] HandleObject;

    [Header("添加的音效")]
    public AudioClip[] Voices;

    private Text DialogueText;

    private int ActionCount;

    private AudioSource aud;
    private int currentPos = 0; //當前打字位置
    private GameManager GM;
    private bool IsPlaying = false;

    private void Start()
    {
        ActionCount = 0;
        DialogueText = GameObject.Find("__DIAOGUES/DialogueUICanvas/DialogueText").GetComponent<Text>();
        aud = GameObject.Find("__AUDIO/對話音效管理器").GetComponent<AudioSource>();
        GM = GameObject.Find("__CONTROLLER/__GameManager").GetComponent<GameManager>();
    }

    public void CallAction()
    {
        GlobalDeclare.byCurrentDialogIndex = (byte)Lv1_Dialogue.Begin;


        GM.CurrentDialogue = gameObject.name;

        if (IsPlaying == false)
            StartCoroutine(StartAction());
    }

    private IEnumerator StartAction()
    {
        IsPlaying = true;
        if (ActionEvent[ActionCount].Contains("{%wait:"))
        {
            var WaitingTime = float.Parse(ActionEvent[ActionCount].Substring(7));
            yield return new WaitForSeconds(WaitingTime);
        }
        else if (ActionEvent[ActionCount].Contains("{%object:"))
        {
            var ObjectAction = ActionEvent[ActionCount].Substring(9);
            string[] Objectdata = ObjectAction.Split(";");
            var ObjectIndex = Int64.Parse(Objectdata[0]);
            var ObjectActive = Convert.ToBoolean(Objectdata[1]);
            var ObjectAlpha = float.Parse(Objectdata[2]);
            HandleObject[ObjectIndex].SetActive(ObjectActive);
            if (ObjectAlpha != -1f) yield return StartCoroutine(AlphaInOut(HandleObject[ObjectIndex], ObjectAlpha));
        }
        else if (ActionEvent[ActionCount].Contains("{%voice:"))
        {
            var VoiceAction = ActionEvent[ActionCount].Substring(8);
            string[] Voicedata = VoiceAction.Split(";");
            var VoiceIndex = Int64.Parse(Voicedata[0]);
            var VoiceVolume = Int64.Parse(Voicedata[1]);
            aud.PlayOneShot(Voices[VoiceIndex], VoiceVolume);
        }
        else
        {
            //print(GM.CurrentDialogue + "       " + gameObject.name);
            if (GM.CurrentDialogue == gameObject.name) DialogueText.text = ActionEvent[ActionCount];
        }

        //yield return StartCoroutine(OnStartWriter());

        if (ActionCount < ActionEvent.Length - 1)
        {
            ActionCount++;
            StartCoroutine(StartAction());
        }
        else
        {
            ActionCount = 0;
            IsPlaying = false;
        }
    }

    private IEnumerator AlphaInOut(GameObject node, float alpha)
    {
        var CG = node.GetComponent<CanvasGroup>();

        if (CG.alpha > alpha)
        {
            while (CG.alpha > alpha)
            {
                CG.alpha = CG.alpha - 0.08f;
                yield return new WaitForSeconds(0.05f);
            }
        }
        else if (CG.alpha < alpha)
        {
            while (CG.alpha < alpha)
            {
                CG.alpha = CG.alpha + 0.08f;
                yield return new WaitForSeconds(0.05f);
            }
        }
        yield return null;
    }

    ///// 打字
    //public IEnumerator OnStartWriter()
    //{
    //    while (currentPos < ActionEvent[ActionCount].Length)
    //    {
    //        currentPos++;
    //        DialogueText.text = ActionEvent[ActionCount].Substring(0, currentPos); //刷新文本顯示内容
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //    currentPos = 0;
    //    yield return null;
    //}

}
