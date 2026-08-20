using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("First-Person Transforms")]
    public Vector3 targetLocalPosition = new Vector3(0.4f, -0.4f, 0.7f);
    public Vector3 customRotation = new Vector3(0f, 0f, 0f);
    public Vector3 customScale = new Vector3(1f, 1f, 1f);

    [Header("Idle Sway Settings")]
    public float swaySpeed = 2f;
    public float swayAmount = 0.05f;

    [Header("Swing Settings")]
    public Vector3 swingRotation = new Vector3(55f, -30f, 0f);
    public float swingDuration = 0.15f;

    private bool isSwinging = false;

    void Update()
    {
        if (!isSwinging)
        {
            // THE FIX: Apply position, rotation, and scale live every single frame
            transform.localPosition = targetLocalPosition;
            transform.localRotation = Quaternion.Euler(customRotation);
            transform.localScale = customScale;

            // Add the idle breathing sway on top of your custom position
            float newY = transform.localPosition.y + Mathf.Sin(Time.time * swaySpeed) * swayAmount;
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);

            // Left mouse click to swing
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(SwingWeapon());
            }
        }
    }

    private IEnumerator SwingWeapon()
    {
        isSwinging = true;
        
        // Dynamically grab whatever rotation you currently have set in the Inspector
        Quaternion baseRotation = Quaternion.Euler(customRotation);
        Quaternion targetRot = baseRotation * Quaternion.Euler(swingRotation);

        // Phase 1: Strike downward
        float elapsed = 0f;
        while (elapsed < swingDuration)
        {
            elapsed += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(baseRotation, targetRot, elapsed / swingDuration);
            yield return null;
        }

        // Phase 2: Return smoothly
        elapsed = 0f;
        float returnDuration = swingDuration * 1.5f;
        while (elapsed < returnDuration)
        {
            elapsed += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(targetRot, baseRotation, elapsed / returnDuration);
            yield return null;
        }

        transform.localRotation = baseRotation;
        isSwinging = false;
    }
}