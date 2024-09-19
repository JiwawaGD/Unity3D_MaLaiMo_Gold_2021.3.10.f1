using UnityEngine;

public class PianoManger : MonoBehaviour
{

    public AudioClip[] Sounds;
    public AudioSource Aud;
    public Animator[] PianoAnimation;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Aud.PlayOneShot(Sounds[0], 1f);
            PianoAnimation[0].SetBool("down",true);
        }
        else if(Input.GetKeyUp(KeyCode.Q)) PianoAnimation[0].SetBool("down", false);

        if (Input.GetKeyDown(KeyCode.W))
        {
            Aud.PlayOneShot(Sounds[1], 1f);
            PianoAnimation[1].SetBool("down", true);
        }
        else if (Input.GetKeyUp(KeyCode.W)) PianoAnimation[1].SetBool("down", false);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Aud.PlayOneShot(Sounds[2], 1f);
            PianoAnimation[2].SetBool("down", true);
        }
        else if (Input.GetKeyUp(KeyCode.E)) PianoAnimation[2].SetBool("down", false);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Aud.PlayOneShot(Sounds[3], 1f);
            PianoAnimation[3].SetBool("down", true);
        }
        else if (Input.GetKeyUp(KeyCode.R)) PianoAnimation[3].SetBool("down", false);

        if (Input.GetKeyDown(KeyCode.T))
        {
            Aud.PlayOneShot(Sounds[4], 1f);
            PianoAnimation[4].SetBool("down", true);
        }
        else if (Input.GetKeyUp(KeyCode.T)) PianoAnimation[4].SetBool("down", false);

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Aud.PlayOneShot(Sounds[5], 1f); ;
            PianoAnimation[5].SetBool("down", true);
        }
        else if (Input.GetKeyUp(KeyCode.Y)) PianoAnimation[5].SetBool("down", false);

        if (Input.GetKeyDown(KeyCode.U))
        {
            Aud.PlayOneShot(Sounds[6], 1f);
            PianoAnimation[6].SetBool("down", true);
        }
        else if (Input.GetKeyUp(KeyCode.U)) PianoAnimation[6].SetBool("down", false);

        if (Input.GetKeyDown(KeyCode.I))
        {
            Aud.PlayOneShot(Sounds[7], 1f);
            PianoAnimation[7].SetBool("down", true);
        }
        else if (Input.GetKeyUp(KeyCode.I)) PianoAnimation[7].SetBool("down", false);

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Aud.PlayOneShot(Sounds[8], 1f);
            PianoAnimation[8].SetBool("down", true);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2)) PianoAnimation[8].SetBool("down", false);

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Aud.PlayOneShot(Sounds[9], 1f);
            PianoAnimation[9].SetBool("down", true);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3)) PianoAnimation[9].SetBool("down", false);

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Aud.PlayOneShot(Sounds[10], 1f);
            PianoAnimation[10].SetBool("down", true);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha5)) PianoAnimation[10].SetBool("down", false);

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Aud.PlayOneShot(Sounds[11], 1f);
            PianoAnimation[11].SetBool("down", true);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha6)) PianoAnimation[11].SetBool("down", false);

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Aud.PlayOneShot(Sounds[12], 1f);
            PianoAnimation[12].SetBool("down", true);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha7)) PianoAnimation[12].SetBool("down", false);
    }
}
