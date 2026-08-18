using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections.Generic;
using System.Collections; 

public class QTEManager : MonoBehaviour
{
    [Header("QTE Settings")]
    public float qteTimeLimit = 5.0f; 
    public int sequenceLength = 4; 
    
    [Header("UI References")]
    public GameObject qtePanel;
    public TextMeshProUGUI timerDisplayText;
    public Image[] iconSlots; 

    [Header("Drag your 8 sprites here!")]
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;
    public Sprite spriteZ;
    public Sprite spriteX;
    public Sprite spriteC; 
    public Sprite spriteV;

    private Dictionary<KeyCode, Sprite> iconDictionary = new Dictionary<KeyCode, Sprite>();
    private List<KeyCode> currentSequence = new List<KeyCode>();
    private int currentStepIndex = 0;
    private float qteTimer;
    private bool isQTEActive = false;

    private void Start() 
    { 
        qtePanel.SetActive(false); 
        
        iconDictionary[KeyCode.UpArrow] = spriteUp;
        iconDictionary[KeyCode.DownArrow] = spriteDown;
        iconDictionary[KeyCode.LeftArrow] = spriteLeft;
        iconDictionary[KeyCode.RightArrow] = spriteRight;
        iconDictionary[KeyCode.Z] = spriteZ;
        iconDictionary[KeyCode.X] = spriteX;
        iconDictionary[KeyCode.C] = spriteC;
        iconDictionary[KeyCode.V] = spriteV;
    }

    public void StartQTE()
    {
        currentSequence.Clear();
        currentStepIndex = 0;
        qteTimer = qteTimeLimit;

        List<KeyCode> keys = new List<KeyCode>(iconDictionary.Keys);
        for (int i = 0; i < sequenceLength; i++)
        {
            currentSequence.Add(keys[Random.Range(0, keys.Count)]);
        }

        isQTEActive = true;
        qtePanel.SetActive(true);
        TimeManager.Instance.StartSlowMotion();
        
        // AUDIO TRIGGER: Play the slow-mo whoosh and duck the phonk track!
        AudioManager.Instance.PlaySlowMoEntry();
        
        UpdateUI();
    }

    private void Update()
    {
        if (!isQTEActive) return;

        qteTimer -= Time.unscaledDeltaTime;
        timerDisplayText.text = "Time: " + qteTimer.ToString("F1");

        if (qteTimer <= 0) ParryFailed("Out of Time!");
        else ListenForInput();
    }

    private void ListenForInput()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(currentSequence[currentStepIndex]))
            {
                StartCoroutine(PunchScaleAnimation(iconSlots[currentStepIndex].transform));
                currentStepIndex++;
                UpdateUI();
                if (currentStepIndex >= currentSequence.Count) ParrySuccess();
            }
            else if (iconDictionary.ContainsKey(GetKeyPressed()))
            {
                ParryFailed("Wrong Key!");
            }
        }
    }

    private KeyCode GetKeyPressed()
    {
        foreach (KeyCode k in iconDictionary.Keys)
            if (Input.GetKeyDown(k)) return k;
        return KeyCode.None;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < iconSlots.Length; i++)
        {
            if (i < currentSequence.Count)
            {
                iconSlots[i].gameObject.SetActive(true);
                iconSlots[i].sprite = iconDictionary[currentSequence[i]];
                iconSlots[i].color = (i < currentStepIndex) ? new Color(1, 1, 1, 0.3f) : new Color(1, 1, 1, 1f);
            }
            else iconSlots[i].gameObject.SetActive(false);
        }
    }

    private IEnumerator PunchScaleAnimation(Transform iconTransform)
    {
        float duration = 0.15f, elapsed = 0f;
        Vector3 orig = Vector3.one, punch = Vector3.one * 1.4f;
        while (elapsed < duration) { elapsed += Time.unscaledDeltaTime; iconTransform.localScale = Vector3.Lerp(orig, punch, elapsed / duration); yield return null; }
        elapsed = 0f;
        while (elapsed < duration) { elapsed += Time.unscaledDeltaTime; iconTransform.localScale = Vector3.Lerp(punch, orig, elapsed / duration); yield return null; }
        iconTransform.localScale = orig;
    }

    private void ParrySuccess() { isQTEActive = false; StartCoroutine(EndQTE(true)); }
    private void ParryFailed(string reason) { isQTEActive = false; StartCoroutine(EndQTE(false)); }

    private IEnumerator EndQTE(bool success)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        qtePanel.SetActive(false);
        TimeManager.Instance.ResetTime();
        
        if (success) 
        {
            AudioManager.Instance.PlayParrySound();
            FindAnyObjectByType<GiantHandController>().DeflectHand();
        }
        else 
        {
            AudioManager.Instance.PlayPlayerHit();
            FindAnyObjectByType<GiantHandController>().HandHitPlayer();
        }
    }

}