using UnityEngine;
using TMPro; 
using System.Collections.Generic;

public class QTEManager : MonoBehaviour
{
    [Header("QTE Settings")]
    public float qteTimeLimit = 3.0f; 
    public int sequenceLength = 4;
    
    [Header("UI References")]
    public TextMeshProUGUI qteDisplayText;
    public TextMeshProUGUI timerDisplayText;
    public GameObject qtePanel;

    private KeyCode[] possibleKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.UpArrow, KeyCode.Mouse0, KeyCode.Mouse1 };
    private List<KeyCode> currentSequence = new List<KeyCode>();
    private int currentStepIndex = 0;
    private float qteTimer;
    private bool isQTEActive = false;

    private void Start() { qtePanel.SetActive(false); }

    public void StartQTE()
    {
        currentSequence.Clear();
        currentStepIndex = 0;
        qteTimer = qteTimeLimit;

        for (int i = 0; i < sequenceLength; i++)
        {
            KeyCode randomKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
            currentSequence.Add(randomKey);
        }

        isQTEActive = true;
        qtePanel.SetActive(true);
        TimeManager.Instance.StartSlowMotion();
        UpdateUI();
    }

    private void Update()
    {
        if (!isQTEActive) return;

        qteTimer -= Time.unscaledDeltaTime;
        timerDisplayText.text = "Time: " + qteTimer.ToString("F1");

        if (qteTimer <= 0)
        {
            ParryFailed("Out of Time!");
            return;
        }
        ListenForInput();
    }

    private void ListenForInput()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(currentSequence[currentStepIndex]))
            {
                currentStepIndex++;
                UpdateUI();

                if (currentStepIndex >= currentSequence.Count)
                {
                    ParrySuccess();
                }
            }
            else
            {
                if (!Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKeyDown(KeyCode.Mouse1) && !Input.GetKeyDown(KeyCode.Mouse2))
                {
                    ParryFailed("Wrong Key!");
                }
            }
        }
    }

    private void UpdateUI()
    {
        string displayText = "";
        for (int i = 0; i < currentSequence.Count; i++)
        {
            if (i < currentStepIndex)
                displayText += "<s><color=green>" + currentSequence[i].ToString() + "</color></s> ";
            else
                displayText += currentSequence[i].ToString() + " ";
        }
        qteDisplayText.text = displayText;
    }

    private void ParrySuccess()
    {
        isQTEActive = false;
        qtePanel.SetActive(false);
        TimeManager.Instance.ResetTime();
        
        // This line plays the successful parry sound!
        AudioManager.Instance.PlayParrySound();
        
        Debug.Log("MASSIVE PARRY! Speed boost applied.");
        FindObjectOfType<GiantHandController>().DeflectHand();
    }

    private void ParryFailed(string reason)
    {
        isQTEActive = false;
        qtePanel.SetActive(false);
        TimeManager.Instance.ResetTime();
        
        // This line plays the damage sound!
        AudioManager.Instance.PlayPlayerHit();
        
        Debug.Log("PARRY FAILED: " + reason + " - Player takes damage!");
        FindObjectOfType<GiantHandController>().HandHitPlayer();
    }
}