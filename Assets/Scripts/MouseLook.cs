using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Sensitivity")]
    public float sensX = 200f;
    public float sensY = 200f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Use unscaledDeltaTime to prevent sudden jumps during lag or frame drops
        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.unscaledDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.unscaledDeltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        // Clamp the camera so you can't look past straight up or straight down
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply smooth rotation to the camera
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}