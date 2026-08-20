using UnityEngine;

public class WeaponAutoAlign : MonoBehaviour
{
    [Header("First-Person Screen Position")]
    public Vector3 targetLocalPosition = new Vector3(0.4f, -0.4f, 0.7f); // Bottom right of screen
    
    [Header("Rotation Offset (Leaned Forward)")]
    [Tooltip("Adjust the X rotation here to make all weapons lean forward uniformly")]
    public Vector3 leanRotation = new Vector3(15f, 0f, 0f); 

    void OnEnable()
    {
        // Instantly snap to the correct hand position on screen
        transform.localPosition = targetLocalPosition;
        
        // Force them upright and leaning forward uniformly
        transform.localRotation = Quaternion.Euler(leanRotation);
    }
}