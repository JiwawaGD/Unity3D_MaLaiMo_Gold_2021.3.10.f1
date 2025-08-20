using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Ghost : MonoBehaviour
{
    public Transform Player;
    public Animator GhostAnimator;
    private float maxDistance = 15f;
    private bool alreadyNoticed = false;
    private bool alreadyCallNoticedFuc = false;
    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, Player.position);
        if (distance < maxDistance)
        {
            maxDistance = 1000f;
            if (alreadyNoticed == false && alreadyCallNoticedFuc == false) StartCoroutine(NoticedPlayer());
            else if (alreadyNoticed == true) GetComponent<NavMeshAgent>().SetDestination(Player.position);
        }
    }
    IEnumerator NoticedPlayer()
    {
        alreadyCallNoticedFuc = true;
        yield return new WaitForSeconds(2f);
        GhostAnimator.SetTrigger("hunt");
        alreadyNoticed = true;
    }
}
