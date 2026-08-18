using UnityEngine;

public class TriggerAttack : MonoBehaviour
{
    [Header("Link your SINGLE Giant Hand here")]
    public GiantHandController singleGiantHand;
    
    [Header("Where should it spawn relative to the trigger?")]
    public Vector3 spawnOffset = new Vector3(-15f, 2f, 0f); 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (singleGiantHand != null)
            {
                Vector3 attackStartPos = transform.position + spawnOffset;
                singleGiantHand.TriggerAttack(attackStartPos);
            }
            gameObject.SetActive(false); 
        }
    }
}
