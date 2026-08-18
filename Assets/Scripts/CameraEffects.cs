using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    private Camera cam;
    private Rigidbody playerRb;
    
    [Header("FOV Settings")]
    public float normalFOV = 60f;
    public float boostedFOV = 75f;
    public float fovTransitionSpeed = 10f;

    void Start()
    {
        cam = GetComponent<Camera>();
        // Find the Rigidbody on the parent Player object
        playerRb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        if (cam == null || playerRb == null) return;

        // Check how fast the player is moving horizontally
        float currentSpeed = new Vector3(playerRb.linearVelocity.x, 0, playerRb.linearVelocity.z).magnitude;

        // Target FOV increases if the player is moving fast or if time is slowed down (QTE)
        float targetFOV = normalFOV;
        
        if (currentSpeed > 5f || Time.timeScale < 1f)
        {
            targetFOV = boostedFOV;
        }

        // Smoothly transition the camera's FOV
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, fovTransitionSpeed * Time.unscaledDeltaTime);
    }
}
