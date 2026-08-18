using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText; // Optional: Link a second text object here

    private float currentScore = 0f;
    private bool isGameOver = false;

    void Start()
    {
        // Ensure the game runs at normal speed on start
        Time.timeScale = 1f; 
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (!isGameOver)
        {
            // Increase score based on time alive (multiplied by 10 so it goes up faster)
            currentScore += Time.unscaledDeltaTime * 10f;
            scoreText.text = "SCORE: " + Mathf.FloorToInt(currentScore).ToString();
        }
    }

    public void TriggerGameOver(string deathReason)
    {
        if (isGameOver) return;

        isGameOver = true;
        gameOverPanel.SetActive(true);
        
        // Stop the game entirely
        Time.timeScale = 0f; 

        // Unlock the mouse cursor so you can add a "Restart" button later
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (finalScoreText != null)
        {
            finalScoreText.text = deathReason + "\nFINAL SCORE: " + Mathf.FloorToInt(currentScore).ToString();
        }
    }
}