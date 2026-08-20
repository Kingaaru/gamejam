using UnityEngine;

public class WeaponAutoAlign : MonoBehaviour
{
    [Header("First-Person Screen Position")]
    public Vector3 targetLocalPosition = new Vector3(0.4f, -0.4f, 0.7f); 
    
    [Header("Rotation Fixer (Tweak X, Y, Z in Inspector!)")]
    public Vector3 customRotation = new Vector3(0f, 0f, 0f); 

    [Header("Scale Fixer (Make weapons bigger/smaller)")]
    [Tooltip("Default is 1,1,1. Lower values (like 0.5, 0.5, 0.5) make the weapon smaller.")]
    public Vector3 customScale = new Vector3(1f, 1f, 1f);

    void OnEnable()
    {
        ApplyTransform();
    }

    void OnValidate()
    {
        ApplyTransform();
    }

    public void ApplyTransform()
    {
        transform.localPosition = targetLocalPosition;
        transform.localRotation = Quaternion.Euler(customRotation);
        transform.localScale = customScale; // Automatically updates the size live!
    }
}