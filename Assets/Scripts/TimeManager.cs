using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("Slow Motion Settings")]
    public float slowMoTimeScale = 0.1f;
    public float transitionDuration = 0.5f;

    private float originalTimeScale = 1f;
    private float originalFixedDeltaTime;
    private Coroutine timeCoroutine;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        originalFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void StartSlowMotion()
    {
        if (timeCoroutine != null) StopCoroutine(timeCoroutine);
        timeCoroutine = StartCoroutine(TransitionTimeScale(slowMoTimeScale));
    }

    public void ResetTime()
    {
        if (timeCoroutine != null) StopCoroutine(timeCoroutine);
        timeCoroutine = StartCoroutine(TransitionTimeScale(originalTimeScale));
    }

    private IEnumerator TransitionTimeScale(float targetScale)
    {
        float startScale = Time.timeScale;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            Time.timeScale = Mathf.Lerp(startScale, targetScale, elapsed / transitionDuration);
            Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale; 
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = targetScale;
        Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale;
    }
}