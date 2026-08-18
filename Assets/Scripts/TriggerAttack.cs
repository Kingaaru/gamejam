using UnityEngine;

public class TriggerAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<GiantHandController>().TriggerAttack();
            gameObject.SetActive(false); // Turns off the trigger so it only happens once
        }
    }
}