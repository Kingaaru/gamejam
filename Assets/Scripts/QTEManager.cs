using UnityEngine;
using TMPro; 
using System.Collections.Generic;
using System.Collections; 

public class QTEManager : MonoBehaviour
{
    [Header("QTE Settings")]
    public float qteTimeLimit = 5.0f; 
    public int sequenceLength = 4; 
    
    [Header("UI References")]
    public TextMeshProUGUI qteDisplayText;
    public TextMeshProUGUI timerDisplayText;
    public GameObject qtePanel;

    private KeyCode[] possibleKeys = { 
        KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V,
        KeyCode.Mouse0, KeyCode.Mouse1 
    };
    
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
                bool wrongValidKeyPressed = false;
                foreach (KeyCode key in possibleKeys)
                {
                    if (Input.GetKeyDown(key))
                    {
                        wrongValidKeyPressed = true;
                        break;
                    }
                }

                if (wrongValidKeyPressed)
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
            string keyName = currentSequence[i].ToString();
            
            if (keyName == "Mouse0") keyName = "LeftClick";
            if (keyName == "Mouse1") keyName = "RightClick";

            if (i < currentStepIndex)
                displayText += "<s><color=green>" + keyName + "</color></s> ";
            else
                displayText += keyName + " ";
        }
        qteDisplayText.text = displayText;
    }

    private void ParrySuccess()
    {
        isQTEActive = false;
        // Start the success coroutine to show the UI text before resetting
        StartCoroutine(ShowSuccessMessage());
    }

    private void ParryFailed(string reason)
    {
        isQTEActive = false; 
        StartCoroutine(ShowFailMessage(reason));
    }

    private IEnumerator ShowSuccessMessage()
    {
        // Flash MASSIVE PARRY in green
        qteDisplayText.text = "<color=green><b>MASSIVE PARRY!</b></color>";
        
        yield return new WaitForSecondsRealtime(1.0f);

        // Resume normal success logic after the pause
        qtePanel.SetActive(false);
        TimeManager.Instance.ResetTime();
        
        // AudioManager.Instance.PlayParrySound();
        FindAnyObjectByType<GiantHandController>().DeflectHand();
    }

    private IEnumerator ShowFailMessage(string reason)
    {
        // Flash PARRY FAILED along with the specific reason in red
        qteDisplayText.text = "<color=red><b>PARRY FAILED\n" + reason.ToUpper() + "</b></color>";
        
        yield return new WaitForSecondsRealtime(1.0f);

        // Resume normal failure logic
        qtePanel.SetActive(false);
        TimeManager.Instance.ResetTime();
        
        // AudioManager.Instance.PlayPlayerHit();
        FindAnyObjectByType<GiantHandController>().HandHitPlayer();
    }
}