using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider sensitivitySlider;

    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", 500f);

        ApplySettings();
        audioMixer.GetFloat("MasterVolume", out float val);
        Debug.Log($"MasterVolume is set to: {val}");

    }

    public void ApplySettings()
    {
        Debug.Log("¡Aplicando configuración!");

        SetVolume("MasterVolume", masterSlider.value);
        SetVolume("MusicVolume", musicSlider.value);
        SetVolume("SFXVolume", sfxSlider.value);

        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivitySlider.value);
    }

    public void SetVolume(string exposedParam, float sliderValue)
    {
        float volume = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(exposedParam, volume);
    }
}
