
using UnityEngine;

public class ShopItem : MonoBehaviour
{

    [Header("All Hand Weapons to Clear")]
    public GameObject[] allHandWeapons; // Drag EVERY hand weapon into this list in the Inspector
    [Header("Item Settings")]
    public string itemName;
    public int cost;
    public bool isArmor;
    
    [Tooltip("0 for Default, 1 for Iron, 2 for Obsidian")]
    public int bonusLives; 
    
    [Tooltip("Drag the actual hidden weapon from the Player Camera here (leave blank for armor)")]
    public GameObject weaponToEquip; 

    public string GetPrompt()
    {
        // Default armor (cost 0) is automatically unlocked
        bool unlocked = PlayerPrefs.GetInt(itemName + "_Unlocked", cost == 0 ? 1 : 0) == 1;
        
        if (unlocked) return "Press [E] to Equip " + itemName;
        return "Press [E] to Buy " + itemName + " (" + cost + " Coins)";
    }

    public void Interact()
    {
        bool unlocked = PlayerPrefs.GetInt(itemName + "_Unlocked", cost == 0 ? 1 : 0) == 1;
        
        if (!unlocked)
        {
            int bank = PlayerPrefs.GetInt("TotalCoins", 0);
            if (bank >= cost)
            {
                // Buy it and save the purchase
                PlayerPrefs.SetInt("TotalCoins", bank - cost);
                PlayerPrefs.SetInt(itemName + "_Unlocked", 1);
                PlayerPrefs.Save();
            }
            else return; // Not enough money
        }

        // Equip the item
        if (isArmor)
        {
            PlayerPrefs.SetInt("BonusLives", bonusLives);
            PlayerPrefs.Save();
            
            // Tell the GameManager to instantly update the blue hearts on the HUD
            FindAnyObjectByType<GameManager>().ApplyArmorLives(bonusLives);
            Debug.Log("Equipped " + itemName + ". Extra lives: " + bonusLives);
        }
        else if (weaponToEquip != null)
        {
            // The newest, warning-free syntax for the latest Unity engine
            ShopItem[] allShopItems = FindObjectsByType<ShopItem>(FindObjectsInactive.Include);
            
            // Turn OFF every single hand weapon tied to any shop item first
            foreach (ShopItem item in allShopItems)
            {
                if (item.weaponToEquip != null)
                {
                    item.weaponToEquip.SetActive(false);
                }
            }

            // Turn ON only the newly selected weapon
            weaponToEquip.SetActive(true);
        }
    }
}