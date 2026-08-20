using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public TextMeshProUGUI interactionText; 
    public float reach = 15f; 

    void Update()
    {
        interactionText.gameObject.SetActive(false);
        
        // 1. THIS IS THE FIX: Grab the actual camera lens that rotates up and down, not the static holder!
        Transform camTransform = Camera.main.transform;
        
        // 2. DEBUG LASER: This draws a solid red line in your Unity SCENE tab while you play. 
        // If it's still missing, you will physically see exactly where the laser is going!
        Debug.DrawRay(camTransform.position, camTransform.forward * reach, Color.red);
        
        // Shoot the X-Ray laser out of the actual camera
        RaycastHit[] hits = Physics.RaycastAll(camTransform.position, camTransform.forward, reach);
        
        foreach (RaycastHit hit in hits)
        {
            // Look for the shop item on whatever we just hit
            ShopItem item = hit.collider.GetComponent<ShopItem>();
            
            if (item != null)
            {
                interactionText.gameObject.SetActive(true);
                interactionText.text = item.GetPrompt();
                
                if (Input.GetKeyDown(KeyCode.E))
                {
                    item.Interact();
                }
                
                return; // Stop the laser once we find our item
            }
        }
    }
}