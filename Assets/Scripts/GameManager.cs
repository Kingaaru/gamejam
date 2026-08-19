using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Menu & UI References")]
    public GameObject mainMenuPanel;
    public GameObject gameHUD; 
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;

    [Header("Gameplay References")]
    public MonoBehaviour playerMovementScript; 
    
    [Tooltip("Drag your MouseLook / Camera rotation script here")]
    public MonoBehaviour mouseLookScript; 

    private float currentScore = 0f;
    private bool isGameOver = false;
    private bool isGamePlaying = false;

    void Start()
    {
        // 1. FREEZE TIME: This instantly stops the crasher, physics, and gravity.
        Time.timeScale = 0f;

        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (gameHUD != null) gameHUD.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // 2. DISABLE SCRIPTS: Stop player from moving or looking around
        if (playerMovementScript != null) playerMovementScript.enabled = false;
        if (mouseLookScript != null) mouseLookScript.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        currentScore = 0f;
        isGamePlaying = false;
    }

    void Update()
    {
        if (isGameOver || !isGamePlaying) return;

        currentScore += Time.unscaledDeltaTime * 10f;

        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + Mathf.FloorToInt(currentScore).ToString();
        }
    }

    public void StartGame()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (gameHUD != null) gameHUD.SetActive(true);

        // Turn the scripts back on
        if (playerMovementScript != null) playerMovementScript.enabled = true;
        if (mouseLookScript != null) mouseLookScript.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isGamePlaying = true;
        currentScore = 0f;
        
        // 3. UNFREEZE TIME: The crasher and physics will now activate!
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("Game is Exiting...");
        Application.Quit();
    }

    public void TriggerGameOver(string deathReason)
    {
        if (isGameOver) return;

        isGameOver = true;
        isGamePlaying = false;

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (gameHUD != null) gameHUD.SetActive(false);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (finalScoreText != null)
        {
            finalScoreText.text = deathReason + "\nFINAL SCORE: " + Mathf.FloorToInt(currentScore).ToString();
        }
    }
}
