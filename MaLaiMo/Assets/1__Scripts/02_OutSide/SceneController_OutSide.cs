using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneController_OutSide : SceneController
{
    public Transform mom; 
    public RectTransform uiElement; 
    public RectTransform arrowIndicator; 
    public Camera PlayerCamera;
    public Transform Player; 

    private Vector3 IconPos;
    public override void Start()
    {
        PlayDialogue((byte)OutSide_Dialogue.Lv2_007_GoOut);
    }

    public override void Update()
    {
        base.Update();
        Vector3 npcViewportPos = PlayerCamera.WorldToViewportPoint(mom.position);
        Vector3 npcDirection = (mom.position - Player.position).normalized;

        //是否在玩家視野範圍內
        if (npcViewportPos.z > 0 && npcViewportPos.x > 0 && npcViewportPos.x < 1 && npcViewportPos.y > 0 && npcViewportPos.y < 1)
        {
            uiElement.gameObject.SetActive(true);
            arrowIndicator.gameObject.SetActive(false);
            Vector3 headOffset = new Vector3(0, 1.8f, 0); //標記在頭上
            uiElement.position = PlayerCamera.WorldToScreenPoint(mom.position + headOffset);
        }
        else
        {
            uiElement.gameObject.SetActive(false);
            arrowIndicator.gameObject.SetActive(true);

            Vector3 relativePos = Player.InverseTransformPoint(mom.position);
            float angleToNPC = Mathf.Atan2(relativePos.x, relativePos.z) * Mathf.Rad2Deg;

            arrowIndicator.rotation = Quaternion.Euler(0, 0, -angleToNPC);//icon角度 ps:可以再調整

            if (npcViewportPos.z < 0)
            {
                npcViewportPos.x = 1 - npcViewportPos.x;
                npcViewportPos.y = 1 - npcViewportPos.y;
            }

            float screenX = Mathf.Clamp(npcViewportPos.x, 0.05f, 0.95f) * Screen.width;
            float screenY = Mathf.Clamp(npcViewportPos.y, 0.05f, 0.95f) * Screen.height;

            // npc在相機後面時，提示icon在底部
            if (relativePos.z < 0)
            {
                screenY = 0.05f * Screen.height;
            }

            arrowIndicator.position = new Vector3(screenX, screenY, 0);
        }
    }
}