using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource slowMoSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip parryClang;
    public AudioClip slowMoWhoosh;
    public AudioClip playerHit;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(backgroundMusic);
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayParrySound()
    {
        if (sfxSource != null && parryClang != null)
        {
            sfxSource.PlayOneShot(parryClang); 
        }
    }

    public void PlaySlowMoEntry()
    {
        if (slowMoSource != null && slowMoWhoosh != null)
        {
            slowMoSource.PlayOneShot(slowMoWhoosh);
        }
    }
    
    public void PlayPlayerHit()
    {
         if (sfxSource != null && playerHit != null)
        {
            sfxSource.PlayOneShot(playerHit);
        }
    }
}