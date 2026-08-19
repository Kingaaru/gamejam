using System.Collections;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [Header("Idle Sway Settings")]
    [Tooltip("How fast the sword breathes up and down")]
    public float swaySpeed = 2f; 
    [Tooltip("How far the sword moves during the sway")]
    public float swayAmount = 0.05f; 

    [Header("Swing Settings")]
    [Tooltip("The rotation angles applied during the swing attack")]
    public Vector3 swingRotation = new Vector3(55f, -30f, 0f); 
    [Tooltip("How fast the sword strikes downward")]
    public float swingDuration = 0.15f; 

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isSwinging = false;

    void Start()
    {
        // Lock in the exact position and rotation you set in the Inspector
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    void Update()
    {
        // Only apply the idle sway if we are not actively attacking
        if (!isSwinging)
        {
            // Calculate procedural idle bobbing using a Sine wave
            float newY = startPosition.y + Mathf.Sin(Time.time * swaySpeed) * swayAmount;
            transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);

            // Trigger the attack on Left Mouse Click
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(SwingSword());
            }
        }
    }

    private IEnumerator SwingSword()
    {
        isSwinging = true;

        // Calculate the target rotation by combining the start rotation with the swing angles
        Quaternion targetRot = startRotation * Quaternion.Euler(swingRotation);

        // Phase 1: The fast downward strike
        float elapsed = 0f;
        while (elapsed < swingDuration)
        {
            elapsed += Time.deltaTime;
            // Slerp mathematically smoothly curves the rotation between two angles
            transform.localRotation = Quaternion.Slerp(startRotation, targetRot, elapsed / swingDuration);
            yield return null;
        }

        // Phase 2: The slightly slower recovery/return to resting position
        elapsed = 0f;
        float returnDuration = swingDuration * 1.5f; 
        while (elapsed < returnDuration)
        {
            elapsed += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(targetRot, startRotation, elapsed / returnDuration);
            yield return null;
        }

        // Snap back exactly to the start to prevent mathematical drift over time
        transform.localRotation = startRotation;
        isSwinging = false;
    }
}