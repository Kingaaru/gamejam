using UnityEngine;

public class TriggerAttack : MonoBehaviour
{
    [Header("References")]
    public GiantHandController giantHand;

    private void Start()
    {
        // FAILSAFE: If the inspector link breaks when it spawns as a clone, this finds it automatically.
        if (giantHand == null)
        {
            giantHand = GetComponentInChildren<GiantHandController>(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (giantHand != null)
            {
                Vector3 spawnPos = new Vector3(other.transform.position.x + 25f, other.transform.position.y + 3f, other.transform.position.z);
                giantHand.BeginAttack(spawnPos, other.transform);
            }
            
            // THE FIX: Only turn off the invisible trigger box, keep the GameObject alive!
            GetComponent<Collider>().enabled = false; 
        }
    }
}