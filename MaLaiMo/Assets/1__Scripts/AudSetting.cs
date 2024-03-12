using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudSetting : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }
    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat(AUDManager.MUSIC_KEY, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(AUDManager.SFX_KEY, 1f);
    }
    private void OnDisable()
    {
        PlayerPrefs.SetString(AUDManager.MUSIC_KEY, musicSlider.value.ToString());
        PlayerPrefs.SetString(AUDManager.SFX_KEY, sfxSlider.value.ToString());
    }
    void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }
    void SetSFXVolume(float value)
    {
        audioMixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }
}
