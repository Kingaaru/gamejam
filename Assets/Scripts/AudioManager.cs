using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources (Auto-Assigned in Awake)")]
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

            // Automatically grab all AudioSource components attached to this object
            AudioSource[] sources = GetComponents<AudioSource>();
            
            if (sources.Length >= 3)
            {
                musicSource = sources[0];
                sfxSource = sources[1];
                slowMoSource = sources[2];
            }
            else
            {
                Debug.LogError("CRITICAL: AudioManager needs exactly 3 Audio Source components attached to it!");
            }
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
        Debug.Log("AudioManager: Playing Parry Sound!");
        if (sfxSource != null && parryClang != null)
        {
            sfxSource.PlayOneShot(parryClang); 
        }
    }

    public void PlaySlowMoEntry()
    {
        Debug.Log("AudioManager: Playing Slow Mo Whoosh!");
        if (slowMoSource != null && slowMoWhoosh != null)
        {
            slowMoSource.PlayOneShot(slowMoWhoosh);
        }
    }
    
    public void PlayPlayerHit()
    {
        Debug.Log("AudioManager: Playing Player Hit Sound!");
        if (sfxSource != null && playerHit != null)
        {
            sfxSource.PlayOneShot(playerHit);
        }
    }
}
