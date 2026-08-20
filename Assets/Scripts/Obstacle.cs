using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Settings")]
    public string deathMessage = "IMPALED ON RED MAGMA!";

    // Handles physical collisions (if the obstacle is solid)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            KillPlayer();
        }
    }

    // Handles overlaps (if you accidentally check "Is Trigger" on the obstacle)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        GameManager gm = FindAnyObjectByType<GameManager>();
        if (gm != null)
        {
            gm.TakeDamage(deathMessage);
        }
        else
        {
            Debug.LogError("Obstacle hit the player, but no GameManager was found!");
        }
    }
}