using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetsAudioMixerOnStart : MonoBehaviour
{

    public AudioMixer masterAudioMixer;
    void Start()
    {
        float masterVolume = PlayerPrefs.GetFloat(OptionsUIHandler.MASTER_PLAYER_PREFS);
        masterAudioMixer.SetFloat(OptionsUIHandler.MASTER_MIXER_STRING, Mathf.Log10(masterVolume) * 20);

        float effectVolume = PlayerPrefs.GetFloat(OptionsUIHandler.EFFECT_PLAYER_PREFS);
        masterAudioMixer.SetFloat(OptionsUIHandler.EFFECT_MIXER_STRING, Mathf.Log10(effectVolume) * 20);

        float musicVolume = PlayerPrefs.GetFloat(OptionsUIHandler.MUSIC_PLAYER_PREFS);
        masterAudioMixer.SetFloat(OptionsUIHandler.MUSIC_MIXER_STRING, Mathf.Log10(musicVolume) * 20);
    }

}
