using UnityEngine;
using UnityEngine.SceneManagement;

public class OutSidePlayer : PlayerController
{

    public override void Awake()
    {
        m_bCursorShow = false;
        m_bCanControl = true;
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