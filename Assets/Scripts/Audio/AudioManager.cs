using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music Audio References")]
    [SerializeField] private AudioClip fightMusic;
    [SerializeField] private AudioClip interludeMusic;

    [Header("SFX Audio References")]
    [SerializeField] private AudioClip gunShot;
    [SerializeField] private AudioClip parry;
    [SerializeField] private AudioClip destierro;
    [SerializeField] private AudioClip enemyInjured;
    [SerializeField] private AudioClip playerInjured;
    [SerializeField] private AudioClip enemyDeath;
    [SerializeField] private AudioClip fuelObtained;
    [SerializeField] private AudioClip buttonClick;

    [Header("AudioMixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    private AudioClip currentMusic;
    public bool IsInMenu { get; set; } = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        musicSource.outputAudioMixerGroup = musicGroup;
        sfxSource.outputAudioMixerGroup = sfxGroup;
    }

    private void Start()
    {
        PlayInterludeMusic();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayFightMusic()
    {
        if (currentMusic == fightMusic) return;

        musicSource.clip = fightMusic;
        musicSource.Play();
        currentMusic = fightMusic;
    }

    public void PlayInterludeMusic()
    {
        if (currentMusic == interludeMusic) return;

        musicSource.clip = interludeMusic;
        musicSource.Play();
        currentMusic = interludeMusic;
    }
}
