using UnityEngine;

public class TriggerAttack : MonoBehaviour
{
    [Header("References")]
    public GiantHandController giantHand;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (giantHand != null)
            {
                // Player runs towards +x, so spawn ahead on X, matching player's height + an offset to avoid ground clipping
                Vector3 spawnPos = new Vector3(other.transform.position.x + 25f, other.transform.position.y + 3f, other.transform.position.z);
                
                giantHand.BeginAttack(spawnPos, other.transform);
            }
            gameObject.SetActive(false); 
        }
    }
}