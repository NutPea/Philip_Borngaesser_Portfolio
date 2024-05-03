using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsUIHandler : MonoBehaviour
{
    public List<GameObject> OtherUI;
    public List<GameObject> OptionsUI;
    public bool optionsAreOpen;

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider effectVolumeSlider;

    public AudioMixer masterAudioMixer;

    public const string MASTER_MIXER_STRING = "MAIN_MIXER";
    public const string MUSIC_MIXER_STRING = "MUSIC_MIXER";
    public const string EFFECT_MIXER_STRING = "EFFECT_MIXER";

    public const string MASTER_PLAYER_PREFS = "MASTER_PREFS";
    public const string MUSIC_PLAYER_PREFS = "MUSIC_PREFS";
    public const string EFFECT_PLAYER_PREFS = "EFFECT_PREFS";

    private void Awake()
    {
        InitSliderValues();

        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        effectVolumeSlider.onValueChanged.AddListener(SetEffectVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void InitSliderValues()
    {
        float masterVolume = PlayerPrefs.GetFloat(MASTER_PLAYER_PREFS);
        SetMasterVolume(masterVolume);
        masterVolumeSlider.value = masterVolume;


        float effectVolume = PlayerPrefs.GetFloat(EFFECT_PLAYER_PREFS);
        SetEffectVolume(effectVolume);
        effectVolumeSlider.value = effectVolume;


        float musicVolume = PlayerPrefs.GetFloat(MUSIC_PLAYER_PREFS);
        SetMusicVolume(musicVolume);
        musicVolumeSlider.value = musicVolume;

    }

    public void SetMasterVolume(float value)
    {
        masterAudioMixer.SetFloat(MASTER_MIXER_STRING,Mathf.Log10(value)*20);
        PlayerPrefs.SetFloat(MASTER_PLAYER_PREFS, value);
    }

    public void SetEffectVolume(float value)
    {
        masterAudioMixer.SetFloat(EFFECT_MIXER_STRING, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(EFFECT_PLAYER_PREFS, value);
    }
    public void SetMusicVolume(float value)
    {
        masterAudioMixer.SetFloat(MUSIC_MIXER_STRING, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(MUSIC_PLAYER_PREFS, value);
    }

    public void ToggleOptions()
    {
        if (!optionsAreOpen)
        {
            OpenOptions();
        }
        else
        {
            CloseDialogue();
        }
    }

    public void OpenOptions()
    {
        foreach(GameObject otherUI in OtherUI)
        {
            otherUI.gameObject.SetActive(false);
        }

        foreach (GameObject optionUI in OptionsUI)
        {
            optionUI.gameObject.SetActive(true);
        }
        optionsAreOpen = true;
    }

    public void CloseDialogue()
    {
        foreach (GameObject otherUI in OtherUI)
        {
            otherUI.gameObject.SetActive(true);
        }

        foreach (GameObject optionUI in OptionsUI)
        {
            optionUI.gameObject.SetActive(false);
        }
        optionsAreOpen = false;
    }




}
