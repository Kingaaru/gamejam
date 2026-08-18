using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // We removed these from the Inspector. The script will build them automatically!
    private AudioSource musicSource;
    private AudioSource sfxSource;
    private AudioSource slowMoSource;

    [Header("Audio Clips (Drag your 4 files here!)")]
    public AudioClip backgroundMusic;
    public AudioClip parryClang;
    public AudioClip slowMoWhoosh;
    public AudioClip playerHit;

    [Header("Ducking Settings")]
    public float normalVolume = 1f;
    public float duckedVolume = 0.2f; 
    private Coroutine duckingCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // ZERO INSPECTOR SETUP REQUIRED: Code creates the speakers for you!
            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
            slowMoSource = gameObject.AddComponent<AudioSource>();

            // Force everything to play directly in the player's ears (2D)
            musicSource.spatialBlend = 0f;
            sfxSource.spatialBlend = 0f;
            slowMoSource.spatialBlend = 0f;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.volume = normalVolume;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayParrySound() 
    { 
        if (parryClang != null) sfxSource.PlayOneShot(parryClang); 
        TriggerDucking(1.2f); 
    }
    
    public void PlaySlowMoEntry() 
    { 
        if (slowMoWhoosh != null) slowMoSource.PlayOneShot(slowMoWhoosh); 
        TriggerDucking(2.2f); 
    }
    
    public void PlayPlayerHit() 
    { 
        if (playerHit != null) sfxSource.PlayOneShot(playerHit); 
        TriggerDucking(1.2f);
    }

    private void TriggerDucking(float duration)
    {
        if (duckingCoroutine != null) StopCoroutine(duckingCoroutine);
        duckingCoroutine = StartCoroutine(DuckMusicRoutine(duration));
    }

    private IEnumerator DuckMusicRoutine(float duration)
    {
        // Instantly lower the volume
        musicSource.volume = duckedVolume;
        
        // Wait for the SFX to finish 
        yield return new WaitForSecondsRealtime(duration);
        
        // Smoothly fade the music back up
        float timer = 0f;
        float fadeTime = 0.5f;
        while (timer < fadeTime)
        {
            timer += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(duckedVolume, normalVolume, timer / fadeTime);
            yield return null;
        }
        musicSource.volume = normalVolume;
    }
}
