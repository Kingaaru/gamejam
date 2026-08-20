using UnityEngine;

public class StartRunDoor : MonoBehaviour
{
    [Header("Run Systems")]
    public GameObject levelManager;
    public GameObject chaserBlock;
    
    [Tooltip("How far behind the player the chaser spawns when the run starts")]
    public float chaserStartDistance = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. THIS IS THE FIX: Tell the GameManager to start the score!
            FindAnyObjectByType<GameManager>().StartScoring();

            // 2. Turn on the endless floor generator
            levelManager.SetActive(true);
            
            // 3. Teleport the chaser directly behind the player's current position
            Vector3 chaserStartPos = new Vector3(other.transform.position.x - chaserStartDistance, chaserBlock.transform.position.y, 0);
            chaserBlock.transform.position = chaserStartPos;
            
            // 4. Activate the chaser
            chaserBlock.SetActive(true);
            
            // 5. Turn off this door so it doesn't trigger twice
            gameObject.SetActive(false); 
        }
    }
}