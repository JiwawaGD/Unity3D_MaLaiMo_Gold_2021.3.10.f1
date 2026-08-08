using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;


public class Mom_Controler_Room : MonoBehaviour
{
    public GrandmaRoom_Player Player;
    public SceneController_Room SceneController;
    public Animator Mom_Ani;
    public Transform Gate;
    private NavMeshAgent agent;
    private bool startWalking = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();  
        agent.updateRotation = false;          
    }

    public void LookAtPlayer()
    {
        //transform.DOLookAt(Player.transform.position, 0.5f);
        transform.LookAt(Player.transform);
    }

    public void GoOut()
    {
        Mom_Ani.SetBool("isWalking", true);
        startWalking = true;
        SceneController.ShowHint(LevelTypeID.Lv1_GrandmaHouse, HintItemID.Lv1_Item_Grandma_Dead_Body);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "¤jªùÄ²µo")
        {
            Player._bCanControl = true;
            gameObject.SetActive(false);
        }

    }
    private void Update()
    {
        if (startWalking == true)
        {
            agent.SetDestination(Gate.position);

            Vector3 velocity = agent.velocity;
            velocity.y = 0; 

            if (velocity.sqrMagnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(velocity);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }
}
