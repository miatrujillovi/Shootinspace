using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [Space]
    [Header("Music Audio References")]
    [SerializeField] private AudioClip fightMusic;
    [SerializeField] private AudioClip interludeMusic;
    [Space]
    [Header("SFX Audio References")]
    [SerializeField] private AudioClip gunShot;
    [SerializeField] private AudioClip parry;
    [SerializeField] private AudioClip destierro;
    [SerializeField] private AudioClip enemyInjured;
    [SerializeField] private AudioClip playerInjured;
    [SerializeField] private AudioClip enemyDeath;
    [SerializeField] private AudioClip fuelObtained;
    [SerializeField] private AudioClip buttonClick;

    private void Start()
    {
        musicSource.clip = interludeMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
