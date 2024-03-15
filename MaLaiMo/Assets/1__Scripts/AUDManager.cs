using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class AUDManager : MonoBehaviour
{
    List<AudioSource> audioSourceList = new List<AudioSource>();
    [SerializeField] AudioMixer audioMixer;
    public static AUDManager instance;

    [Header("AudioSource")]
    [SerializeField] string audioAssetBundleName;
    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] AudioSource ScendAudioSource;
    [SerializeField, Tooltip("沖水聲")] public AudioSource FlushSound;

    [Header("通用音效")]
    [SerializeField, Tooltip("鬼影音效")] AudioClip Ghostly_Shrill;
    [SerializeField, Tooltip("發現線索音效")] AudioClip get_Item_Sound;
    [SerializeField, Tooltip("開關燈(包括手電筒)")] AudioClip light_Switch_Sound;
    [SerializeField, Tooltip("開抽屜")] AudioClip Open_Drawer;
    [SerializeField, Tooltip("拿鑰匙")] AudioClip Get_Key;
    [SerializeField, Tooltip("門被鎖住")] AudioClip Door_Locked;
    [SerializeField, Tooltip("關門")] AudioClip Door_Close;
    [SerializeField, Tooltip("UI內文")] AudioClip ui_Context;

    [Header("第一關音效")]
    [SerializeField, Tooltip("開始音效")] AudioClip LV1_Opening_Scene;
    [SerializeField, Tooltip("摺紙聲")] AudioClip[] gold_Paper;
    [SerializeField, Tooltip("孝簾拉開")] AudioClip LV1_Open_Filial_Piety_Curtain;
    [SerializeField, Tooltip("廁所門打開")] AudioClip LV1_Bathroom_Toilet_Door;

    [Header("第二關音效")]
    [SerializeField, Tooltip("小女孩笑聲")] AudioClip Lv2_Girl_laughing;
    [SerializeField, Tooltip("阿嬤詭異聲")] AudioClip LV2_Grandma_StrangeVoice;
    [SerializeField, Tooltip("開啟廁所門，阿嬤出現")] AudioClip LV2_Grandma_Appear_bathroom;
    [SerializeField, Tooltip("推倒玩家鬼手音效Part2")] AudioClip Falling_To_Black_Screen_Sound_Part2;
    [SerializeField, Tooltip("敲門聲")] AudioClip LV2_Knock_Door;
    [SerializeField, Tooltip("相框破碎聲")] AudioClip LV2_PhotoFrame_Breaking;
    [SerializeField, Tooltip("門開起 + 詭異音效")] AudioClip LV2_Open_Door;
    [SerializeField, Tooltip("阿嬤最後嚇人")] AudioClip LV2_Grandma_Scary;

    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";

    private void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSourceList.Add(audioSource);
        }
        mainAudioSource = GetComponent<AudioSource>();
        Transform childTransform = transform.Find("SecondAudioSource");
        if (childTransform != null)
            ScendAudioSource = childTransform.GetComponent<AudioSource>();
        if (instance == null) instance = this;
    }

    private IEnumerator RestoreVolume(float originalVolume)
    {
        yield return new WaitForSeconds(Open_Drawer.length); // 等待音效播放完畢

        mainAudioSource.volume = originalVolume; // 恢復原始音量
    }


    private AudioSource GetAudioSource(int index)
    {
        if (index >= 0 && index < audioSourceList.Count)
        {
            return audioSourceList[index];
        }
        return null;
    }
    public void Play(int index, string name, bool isLoop)
    {
        var audioSource = GetAudioSource(index);

        if (audioSource != null)
        {
            var clip = GetAudioClip(name);

            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.loop = isLoop;
                audioSource.Play();
            }
        }
    }
    AudioClip GetAudioClip(string name)
    {
        switch (name)
        {
            case "Girl_laughing":
                return Lv2_Girl_laughing;
            case "the_sound_of_the_eyes_opening_the_door":
                return LV2_Open_Door;
            case "the_toilet_door_opens":
                return LV1_Bathroom_Toilet_Door;
            case "At_the_end_it_is_found_that_Acuan_has_mostly_disappeared_and_Acuan_has_climbed_up":
                return LV2_Grandma_Scary;
            case "grandma_StrangeVoice":
                return LV2_Grandma_StrangeVoice;
            case "Crying_in_the_bathroom":
                return LV2_Grandma_Appear_bathroom;
            case "get_Item_Sound":
                return get_Item_Sound;
            case "ghostly_shrill_sound_effects":
                return Ghostly_Shrill;
            case "mirror_Breaking_Sound":
                return LV2_PhotoFrame_Breaking;
            case "light_Switch_Sound":
                return light_Switch_Sound;
            case "drawer_Opening_Sound":
                return Open_Drawer;
            case "tet_Sound_Of_Get_The_Key":
                return Get_Key;
            case "gold_Paper":
                return gold_Paper[Random.Range(0, 2)];
            case "filial_Piety_Curtain":
                return LV1_Open_Filial_Piety_Curtain;
            case "the_door_is_locked_and_cannot_be_opened_with_sound_effects":
                return Door_Locked;
            case "door_Close":
                return Door_Close;
            case "Falling_To_Black_Screen_Sound_Part2":
                return Falling_To_Black_Screen_Sound_Part2;
            case "emergency_Knock_On_The_Door":
                return LV2_Knock_Door;
            case "Opening_Scene":
                return LV1_Opening_Scene;
            case "ui_Context":
                return ui_Context;

        }
        return null;
    }

    public void PlayerLotusPaperSFX()
    {
        mainAudioSource.PlayOneShot(gold_Paper[Random.Range(0, 2)]);
    }



    /// <summary>
    /// 存取紀錄
    /// </summary>
    public void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        audioMixer.SetFloat(AudSetting.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        audioMixer.SetFloat(AudSetting.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }
}
