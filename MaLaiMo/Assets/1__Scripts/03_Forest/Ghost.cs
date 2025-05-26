using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    public Transform Player;
    private float maxDistance = 15f;


    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, Player.position);
        if (distance < maxDistance)
        {
            maxDistance = 1000f;
            GetComponent<NavMeshAgent>().SetDestination(Player.position);
        }
    }
}
