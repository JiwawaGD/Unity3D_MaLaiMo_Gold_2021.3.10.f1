using UnityEngine;
using UnityEngine.SceneManagement;

public class OutSidePlayer : PlayerController
{

    public override void Awake()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "森林傳送點")
        {
            SceneManager.LoadScene("5 Forest_Scene");
        }
        else if (other.name == "室內傳送點")
        { 
            SceneManager.LoadScene("2 Grandma House");
        }
    }
}