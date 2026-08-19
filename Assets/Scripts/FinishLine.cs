using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [Header("Victory Settings")]
    public string winText = "GAME WON! ESCAPED THE MAGMA!";

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object passing through is the Player
        if (other.CompareTag("Player"))
        {
            GameManager gm = FindAnyObjectByType<GameManager>();
            if (gm != null)
            {
                gm.TriggerGameWon(winText);
            }
        }
    }
}
