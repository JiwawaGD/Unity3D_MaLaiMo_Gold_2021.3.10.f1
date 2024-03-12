using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class AUDManager : MonoBehaviour
{
    List<AudioSource> audioSourceList = new List<AudioSource>();
    [SerializeField] AudioMixer audioMixer;
    public static AUDManager instance;

    [SerializeField] string audioAssetBundleName;
    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] AudioSource ScendAudioSource;

    [SerializeField, Tooltip("鬼影專用音效")] AudioClip ghostly_shrill_sound_effects;
    [SerializeField, Tooltip("鏡子破碎聲")] AudioClip mirror_Breaking_Sound;
    [SerializeField, Tooltip("發現線索音效")] AudioClip get_Item_Sound;
    [SerializeField, Tooltip("墜落轉黑畫面時")] AudioClip falling_To_Black_Screen_Sound;

    [SerializeField, Tooltip("東西竄動的聲音")] AudioClip the_sound_of_something_moving;
    [SerializeField, Tooltip("小朋友笑聲")] AudioClip children_laughing;
    [SerializeField, Tooltip("小女孩笑聲")] AudioClip Girl_laughing;
    [SerializeField, Tooltip("阿嬤開始向前")] AudioClip grandma_Starts_Walking;
    [SerializeField, Tooltip("阿嬤詭異聲")] AudioClip grandma_StrangeVoice;
    [SerializeField, Tooltip("扭動身體的音效")] AudioClip body_Twisting_Sound;
    [SerializeField, Tooltip("模糊不清的人聲音")] AudioClip muffled_Vocals;
    [SerializeField, Tooltip("緊張呼吸聲")] AudioClip nervous_Breathing;
    [SerializeField, Tooltip("手電筒開關聲")] AudioClip flashlight_Switch_Sound;
    [SerializeField, Tooltip("鬼怪沙啞嘶吼聲")] AudioClip the_hoarse_roar_of_the_ghost;
    [SerializeField, Tooltip("鬼影快速出現短暫音效")] AudioClip ghosts_appear_quickly_with_short_sound_effects;
    [SerializeField, Tooltip("門縫鬼影")] AudioClip Crying_in_the_bathroom;

    [SerializeField, Tooltip("開關燈")] AudioClip light_Switch_Sound;
    [SerializeField, Tooltip("抽屜")] AudioClip drawer_Opening_Sound;
    [SerializeField] AudioClip get_out_od_bed;
    [SerializeField, Tooltip("床")] AudioClip the_Sound_Of_Opening_Wardrobes_And_Doors;
    [SerializeField, Tooltip("鑰匙")] AudioClip tet_Sound_Of_Get_The_Key;

    [SerializeField, Tooltip("摺紙聲")] AudioClip[] gold_Paper;
    [SerializeField, Tooltip("時鐘")] AudioClip clock;
    [SerializeField, Tooltip("鋼琴")] AudioClip piano;
    [SerializeField, Tooltip("孝簾拉開音效")] AudioClip filial_Piety_Curtain;
    [SerializeField, Tooltip("佛歌")] AudioClip buddhist_Song;

    [SerializeField, Tooltip("蠟燭燃燒")] AudioClip candle_Burning;

    [SerializeField, Tooltip("門被鎖起來打不開音效")] AudioClip the_door_is_locked_and_cannot_be_opened_with_sound_effects;
    [SerializeField, Tooltip("關門聲")] AudioClip door_Close;
    [SerializeField, Tooltip("老舊門開門聲")] AudioClip the_sound_of_the_old_door_opening;
    [SerializeField, Tooltip("眼睛小徑開門音效")] AudioClip the_sound_of_the_eyes_opening_the_door;
    [SerializeField, Tooltip("廁所門打開聲音")] AudioClip the_toilet_door_opens;

    [SerializeField, Tooltip("腳尾飯")] AudioClip sound_Of_Something_Falling;

    [SerializeField, Header("廁所")] AudioClip bathroomSound;
    [SerializeField] AudioClip toilet_water_dripping_sound;
    [SerializeField, Tooltip("推倒玩家鬼手音效Part1")] AudioClip Falling_To_Black_Screen_Sound_Part1;
    [SerializeField, Tooltip("推倒玩家鬼手音效Part2")] AudioClip Falling_To_Black_Screen_Sound_Part2;
    [SerializeField, Tooltip("轉水龍頭聲")] AudioClip turn_The_Tap;
    [SerializeField, Tooltip("敲門聲")] AudioClip emergency_Knock_On_The_Door;

    [SerializeField, Tooltip("白噪音")] AudioClip white_Noise;
    [SerializeField, Tooltip("遊戲開始鐘鼓音效")] AudioClip game_start_bell_and_drum_sound_effect;
    [SerializeField, Tooltip("恐怖開始")] AudioClip Opening_Scene;
    [SerializeField, Tooltip("發現線索音效")] AudioClip soprano_Violin;
    [SerializeField, Tooltip("進入場景低沉音效")] AudioClip enter_Scene_Sound;
    [SerializeField, Tooltip("UI內文")] AudioClip ui_Context;
    [SerializeField] AudioClip At_the_end_it_is_found_that_Acuan_has_mostly_disappeared_and_Acuan_has_climbed_up;
    [SerializeField, Tooltip("沖水聲")] public AudioSource FlushSound;


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
        yield return new WaitForSeconds(drawer_Opening_Sound.length); // 等待音效播放完畢

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
                return Girl_laughing;
            case "the_sound_of_the_eyes_opening_the_door":
                return the_sound_of_the_eyes_opening_the_door;
            case "the_toilet_door_opens":
                return the_toilet_door_opens;
            case "At_the_end_it_is_found_that_Acuan_has_mostly_disappeared_and_Acuan_has_climbed_up":
                return At_the_end_it_is_found_that_Acuan_has_mostly_disappeared_and_Acuan_has_climbed_up;
            case "body_Twisting_Sound":
                return body_Twisting_Sound;
            case "grandma_StrangeVoice":
                return grandma_StrangeVoice;
            case "grandma_Starts_Walking":
                return grandma_Starts_Walking;
            case "flashlight_Switch_Sound":
                return flashlight_Switch_Sound;
            case "Crying_in_the_bathroom":
                return Crying_in_the_bathroom;
            case "get_Item_Sound":
                return get_Item_Sound;
            case "ghostly_shrill_sound_effects":
                return ghostly_shrill_sound_effects;
            case "mirror_Breaking_Sound":
                return mirror_Breaking_Sound;
            case "light_Switch_Sound":
                return light_Switch_Sound;
            case "drawer_Opening_Sound":
                return drawer_Opening_Sound;
            case "getting_Out_Of_Bed":
                return get_out_od_bed;
            case "the_Sound_Of_Opening_Wardrobes_And_Doors":
                return the_Sound_Of_Opening_Wardrobes_And_Doors;
            case "tet_Sound_Of_Get_The_Key":
                return tet_Sound_Of_Get_The_Key;
            case "gold_Paper":
                return gold_Paper[Random.Range(0, 2)];
            case "clock":
                return clock;
            case "piano":
                return piano;
            case "filial_Piety_Curtain":
                return filial_Piety_Curtain;
            case "buddhist_Song":
                return buddhist_Song;
            case "candle_Burning":
                return candle_Burning;
            case "the_door_is_locked_and_cannot_be_opened_with_sound_effects":
                return the_door_is_locked_and_cannot_be_opened_with_sound_effects;
            case "door_Close":
                return door_Close;
            case "the_sound_of_the_old_door_opening":
                return the_sound_of_the_old_door_opening;
            case "sound_Of_Something_Falling":
                return sound_Of_Something_Falling;
            case "toilet_water_dripping_sound":
                return toilet_water_dripping_sound;
            case "Falling_To_Black_Screen_Sound_Part1":
                return Falling_To_Black_Screen_Sound_Part1;
            case "Falling_To_Black_Screen_Sound_Part2":
                return Falling_To_Black_Screen_Sound_Part2;
            case "turn_The_Tap":
                return turn_The_Tap;
            case "emergency_Knock_On_The_Door":
                return emergency_Knock_On_The_Door;
            case "white_Noise":
                return white_Noise;
            case "game_start_bell_and_drum_sound_effect":
                return game_start_bell_and_drum_sound_effect;
            case "Opening_Scene":
                return Opening_Scene;
            case "soprano_Violin":
                return soprano_Violin;
            case "enter_Scene_Sound":
                return enter_Scene_Sound;
            case "ui_Context":
                return ui_Context;

        }
        return null;
    }
    public void GrandmaStrangeVoiceStop()
    {
        mainAudioSource.PlayOneShot(grandma_StrangeVoice);
    }


    public void OpenTheDrawerSFX()
    {
        float originalVolume = mainAudioSource.volume;
        mainAudioSource.volume = originalVolume * 0.5f;
        mainAudioSource.PlayOneShot(drawer_Opening_Sound);

        StartCoroutine(RestoreVolume(originalVolume));
    }
    public void PlayerLotusPaperSFX()
    {
        mainAudioSource.PlayOneShot(gold_Paper[Random.Range(0, 2)]);
    }
    public void PlayerGameEventSFX()
    {
        mainAudioSource.PlayOneShot(ui_Context);
    }
    public void BodyTwistingSound()
    {
        mainAudioSource.PlayOneShot(body_Twisting_Sound);
    }

    public void TheSoundOfSomethingMoving()
    {
        mainAudioSource.PlayOneShot(the_sound_of_something_moving);
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
