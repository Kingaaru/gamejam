using UnityEngine;

public class PlayerDuck : MonoBehaviour
{
    [Header("Ducking Settings")]
    [Tooltip("The key you hold to duck/slide")]
    public KeyCode duckKey = KeyCode.LeftControl; 
    
    [Tooltip("How small the player gets (0.5 means half height)")]
    public float duckHeight = 0.5f; 
    
    [Tooltip("How fast the camera drops and rises")]
    public float crouchSpeed = 10f; 

    private Vector3 normalScale;
    private Vector3 duckScale;

    void Start()
    {
        // Remember the starting size of the player
        normalScale = transform.localScale;
        
        // Calculate the target size for when we are ducking (only shrinking the Y axis)
        duckScale = new Vector3(normalScale.x, normalScale.y * duckHeight, normalScale.z);
    }

    void Update()
    {
        // Check if the game is paused or over to prevent weird behavior
        if (Time.timeScale == 0f) return;

        // If we are holding down the key, our target is the duck scale. Otherwise, it's normal scale.
        Vector3 targetScale = Input.GetKey(duckKey) ? duckScale : normalScale;
        
        // Smoothly transition the player's scale to the target
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * crouchSpeed);
    }
}