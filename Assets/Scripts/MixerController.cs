using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string masterVolumeParameter = "MasterVolume";
    [SerializeField] private string ambienceVolumeParameter = "AmbienceVolume";
    [SerializeField] private string effectsVolumeParameter = "EffectsVolume";

    private const string AmbienceVolumeKey = "AmbienceVolume";
    private const string EffectsVolumeKey = "EffectsVolume";
    private const float MinVolume = 0.0001f;
    public float AmbienceVolume => PlayerPrefs.GetFloat(AmbienceVolumeKey, 1f);
    public float EffectsVolume => PlayerPrefs.GetFloat(EffectsVolumeKey, 1f);

    private void Start()
    {
        SetAmbienceVolume(AmbienceVolume);
        SetEffectsVolume(EffectsVolume);
    }

    public void SetMasterVolume(float volume)
    {
        SetVolume(masterVolumeParameter, volume);
    }

    public void SetAmbienceVolume(float volume)
    {
        SetVolume(ambienceVolumeParameter, volume);
        SaveAmbienceVolume(volume);
    }

    public void SetEffectsVolume(float volume)
    {
        SetVolume(effectsVolumeParameter, volume);
        SaveEffectsVolume(volume);
    }

    private void SetVolume(string parameter, float volume)
    {
        var mixerVolume = volume <= MinVolume ? -80.0f : MathF.Log10(volume) * 20.0f;
        mixer.SetFloat(parameter, mixerVolume);
    }

    private static void SaveAmbienceVolume(float volume)
    {
        PlayerPrefs.SetFloat(AmbienceVolumeKey, volume);
    }

    private static void SaveEffectsVolume(float volume)
    {
        PlayerPrefs.SetFloat(EffectsVolumeKey, volume);
    }
}