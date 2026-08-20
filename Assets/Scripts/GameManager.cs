using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Menu & UI References")]
    public GameObject mainMenuPanel;
    public GameObject gameHUD; 
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    
    [Tooltip("Drag your Lobby Coins Text UI here")]
    public TextMeshProUGUI lobbyCoinsText; 

    [Header("Gameplay References")]
    public MonoBehaviour playerMovementScript; 
    public MonoBehaviour mouseLookScript; 

    [Header("Lives System")]
    public int currentRedLives = 3;
    public int currentBlueLives = 0;
    
    public Image[] redHeartIcons; 
    public Image[] blueHeartIcons; 
    [Header("Audio")]
    public AudioClip damageSound;

    private float currentScore = 0f;
    public bool isGameOver = false; // Made public so the player knows when to respawn
    private bool isGamePlaying = false;

    void Start()
    {
        Time.timeScale = 0f;

        // 1. DEFAULT ARMOR FIX: Reset blue lives and ensure default is owned
        PlayerPrefs.SetInt("BonusLives", 0);
        PlayerPrefs.SetInt("Default Armor_Unlocked", 1);
        currentRedLives = 3;
        currentBlueLives = 0; 
        UpdateHeartsUI();

        // 2. COINS FIX: Show coins in the lobby
        if (lobbyCoinsText != null)
        {
            lobbyCoinsText.gameObject.SetActive(true);
            lobbyCoinsText.text = "COINS: " + PlayerPrefs.GetInt("TotalCoins", 0).ToString();
        }

        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (gameHUD != null) gameHUD.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        if (playerMovementScript != null) playerMovementScript.enabled = false;
        if (mouseLookScript != null) mouseLookScript.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        currentScore = 0f;
        isGamePlaying = false;
        isGameOver = false;
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

    public void ApplyArmorLives(int extraLives)
    {
        currentBlueLives = extraLives;
        UpdateHeartsUI();
    }

    // THE FIX: Route all damage through this, and pass a custom death reason!
    public void TakeDamage(string deathReason = "OUT OF LIVES!")
    {
        if (isGameOver) return;

        // THE FIX: Play the sound effect directly at the camera's location!
        if (damageSound != null)
        {
            AudioSource.PlayClipAtPoint(damageSound, Camera.main.transform.position);
        }

        if (currentBlueLives > 0)
        {
            currentBlueLives--; // Break armor first
        }
        else
        {
            currentRedLives--; // Then break base health
        }

        UpdateHeartsUI();

        if (currentRedLives <= 0)
        {
            TriggerGameOver(deathReason);
        }
    }

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < redHeartIcons.Length; i++) redHeartIcons[i].enabled = i < currentRedLives;
        for (int i = 0; i < blueHeartIcons.Length; i++) blueHeartIcons[i].enabled = i < currentBlueLives;
    }

    public void StartGame()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (gameHUD != null) gameHUD.SetActive(true);

        if (playerMovementScript != null) playerMovementScript.enabled = true;
        if (mouseLookScript != null) mouseLookScript.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        Time.timeScale = 1f;
    }

    public void StartScoring()
    {
        isGamePlaying = true;
        // HIDE COINS WHEN RUN STARTS
        if (lobbyCoinsText != null) lobbyCoinsText.gameObject.SetActive(false); 
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
        SaveRunScore();
    }

    private void SaveRunScore()
    {
        int runScore = Mathf.FloorToInt(currentScore);
        int currentBank = PlayerPrefs.GetInt("TotalCoins", 0);
        PlayerPrefs.SetInt("TotalCoins", currentBank + runScore);
        PlayerPrefs.Save();
    }
}