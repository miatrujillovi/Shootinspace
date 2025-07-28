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

    private GameSettings currentSettings;

    private void Start()
    {
        masterSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();
        sensitivitySlider.onValueChanged.RemoveAllListeners();

        masterSlider.minValue = 0f;
        masterSlider.maxValue = 1f;

        musicSlider.minValue = 0f;
        musicSlider.maxValue = 1f;

        sfxSlider.minValue = 0f;
        sfxSlider.maxValue = 1f;

        sensitivitySlider.minValue = 0f;
        sensitivitySlider.maxValue = 500f;

        currentSettings = SettingsManager.Load();

        masterSlider.value = Mathf.Clamp(currentSettings.masterVolume, 0f, 1f);
        musicSlider.value = Mathf.Clamp(currentSettings.musicVolume, 0f, 1f);
        sfxSlider.value = Mathf.Clamp(currentSettings.sfxVolume, 0f, 1f);
        sensitivitySlider.value = Mathf.Clamp(currentSettings.mouseSensitivity, 0f, 500f);

        SetVolume("MasterVolume", masterSlider.value);
        SetVolume("MusicVolume", musicSlider.value);
        SetVolume("SFXVolume", sfxSlider.value);

        masterSlider.onValueChanged.AddListener((v) => { currentSettings.masterVolume = v; SetVolume("MasterVolume", v); });
        musicSlider.onValueChanged.AddListener((v) => { currentSettings.musicVolume = v; SetVolume("MusicVolume", v); });
        sfxSlider.onValueChanged.AddListener((v) => { currentSettings.sfxVolume = v; SetVolume("SFXVolume", v); });
        sensitivitySlider.onValueChanged.AddListener((v) => { currentSettings.mouseSensitivity = v; });
    }


    public void ApplySettings()
    {
        Debug.Log("¡Aplicando configuración!");

        currentSettings.masterVolume = masterSlider.value;
        currentSettings.musicVolume = musicSlider.value;
        currentSettings.sfxVolume = sfxSlider.value;
        currentSettings.mouseSensitivity = sensitivitySlider.value;

        SetVolume("MasterVolume", masterSlider.value);
        SetVolume("MusicVolume", musicSlider.value);
        SetVolume("SFXVolume", sfxSlider.value);

        SettingsManager.Save(currentSettings);
    }

    public void SetVolume(string exposedParam, float sliderValue)
    {
        float volume = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(exposedParam, volume);
    }

    public void DeleteConfig()
    {
        SettingsManager.Delete();
    }
}
