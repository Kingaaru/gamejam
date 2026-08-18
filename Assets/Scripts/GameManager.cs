using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText; 

    private float currentScore = 0f;
    private bool isGameOver = false;
    private bool gameStarted = false;

    void Start()
    {
        Time.timeScale = 1f; 
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        
        currentScore = 0f;
        gameStarted = false;

        // Force UI to show 0 immediately on load
        if (scoreText != null)
        {
            scoreText.text = "SCORE: 0";
        }
    }

    void Update()
    {
        if (isGameOver) return;

        // Wait until the player gives input (presses any key or clicks) to start scoring
        if (!gameStarted)
        {
            if (Input.anyKeyDown || Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f)
            {
                gameStarted = true;
                currentScore = 0f; // Force reset to exact zero the moment movement begins
            }
            return;
        }

        // Increment score now that player has actively started playing
        currentScore += Time.unscaledDeltaTime * 10f;

        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + Mathf.FloorToInt(currentScore).ToString();
        }
    }

    public void TriggerGameOver(string deathReason)
    {
        if (isGameOver) return;

        isGameOver = true;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        
        Time.timeScale = 0f; 

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (finalScoreText != null)
        {
            finalScoreText.text = deathReason + "\nFINAL SCORE: " + Mathf.FloorToInt(currentScore).ToString();
        }
    }
}
