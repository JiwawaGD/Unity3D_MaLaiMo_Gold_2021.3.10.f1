using System.Collections;
using UnityEngine;

public partial class GameEventController : MonoBehaviour
{
    #region - Field -
    [SerializeField] [Header("對話物件")] DialogueManager[] DialogueObjects;

    [SerializeField] [Header("玩家")] PlayerController PlayerCtrlr;
    #endregion

    #region - Basic Function -
    public void RecGameEvent(LevelTypeID r_SceneTypeID, GameEventID r_EventID)
    {
        switch (r_SceneTypeID)
        {
            case LevelTypeID.BeginScene:
                break;
            case LevelTypeID.Introduce:
                break;
            case LevelTypeID.Lv1_GrandmaHouse:
                Lv1_Event(r_EventID);
                break;
            case LevelTypeID.Lv2_GrandmaHouse:
                Lv2_Event(r_EventID);
                break;
        }
    }
    #endregion

    #region - Event Function -
    void Lv1_Event(GameEventID r_EventID)
    {
        switch (r_EventID)
        {
            case GameEventID.Lv1_TalkToPackage:
                Lv1_TalkToPackage();
                break;
            case GameEventID.Lv1_GrandmaRoomDoorOpen:
                Lv1_GrandmaRoomDoorOpen();
                break;
            default:
                Debug.LogError(string.Format("[Lv1_Event] Error GameEventID : {0}", r_EventID));
                break;
        }
    }

    void Lv2_Event(GameEventID r_EventID)
    {
        switch (r_EventID)
        {
            default:
                Debug.LogError(string.Format("[Lv2_Event] Error GameEventID : {0}", r_EventID));
                break;
        }
    }
    #endregion

    #region - External Function -
    public void DealDelay()
    {

    }

    #region - Animation Type -
    public void ProcessPlayerAnimator(string r_sAnimationName)
    {
        Transform tfPlayer = GameObject.Find("_Player/LingLing").transform;
        Animation Am = tfPlayer.GetComponent<Animation>();

        Am.PlayQueued(r_sAnimationName);
        GlobalDeclare.SetPlayerAnimateType(PlayerAnimateType.Empty);
    }

    IEnumerator PlayerToAniPos(Vector3 r_V3TargetPos, Quaternion r_PlayerRotation, Quaternion r_CameraRotation)
    {
        PlayerCtrlr.m_bCanControl = false;
        PlayerCtrlr.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        PlayerCtrlr.gameObject.GetComponent<Rigidbody>().useGravity = false;

        // 移動玩家
        float fTotalMoveTime = 1.0f;
        float fCurrentMoveTime = 0.0f;

        while (fCurrentMoveTime < fTotalMoveTime)
        {
            PlayerCtrlr.transform.localPosition = Vector3.Lerp(PlayerCtrlr.transform.localPosition, r_V3TargetPos, fCurrentMoveTime / (fTotalMoveTime * 5f));
            PlayerCtrlr.transform.localRotation = Quaternion.Slerp(PlayerCtrlr.transform.localRotation, r_PlayerRotation, fCurrentMoveTime / (fTotalMoveTime * 5f));

            fCurrentMoveTime += Time.deltaTime;

            yield return null;
        }

        PlayerCtrlr.transform.localPosition = r_V3TargetPos;
        PlayerCtrlr.transform.localRotation = r_PlayerRotation;

        // 移動玩家 Camera
        float fTotalViewTime = 1.0f;
        float fCurrentViewTime = 0.0f;

        while (fCurrentViewTime < fTotalViewTime)
        {
            PlayerCtrlr.tfPlayerCamera.localRotation = Quaternion.Slerp(PlayerCtrlr.tfPlayerCamera.localRotation, r_CameraRotation, fCurrentViewTime / (fTotalViewTime * 5f));

            fCurrentViewTime += Time.deltaTime;

            yield return null;
        }

        PlayerCtrlr.tfPlayerCamera.localRotation = r_CameraRotation;
    }
    #endregion
    #endregion
}