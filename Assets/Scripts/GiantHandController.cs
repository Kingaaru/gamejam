using UnityEngine;
using System.Collections;

public class GiantHandController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float attackSpeed = 15f;
    public float retreatSpeed = 25f;
    public Transform playerTarget;
    
    [Header("Attack States")]
    private bool isAttacking = false;
    private bool isRetreating = false;
    private Vector3 startPosition;

    private void Start() { startPosition = transform.position; }

    public void TriggerAttack()
    {
        if (!isAttacking && !isRetreating)
        {
            isAttacking = true;
            Debug.Log("Giant Hand Incoming!");
            
            // This line plays the slow-mo sound!
            // AudioManager.Instance.PlaySlowMoEntry(); 
            
            FindAnyObjectByType<QTEManager>().StartQTE();
        }
    }

    private void Update()
    {
        if (isAttacking && playerTarget != null)
        {
            float step = attackSpeed * Time.unscaledDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, step);
            
            if (Vector3.Distance(transform.position, playerTarget.position) < 2.0f)
            {
                isAttacking = false; 
            }
        }
        
        if (isRetreating)
        {
            float step = retreatSpeed * Time.unscaledDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, startPosition, step);
            
            if (Vector3.Distance(transform.position, startPosition) < 1.0f)
            {
                isRetreating = false;
                gameObject.SetActive(false); 
            }
        }
    }

    public void DeflectHand()
    {
        Debug.Log("Hand Deflected!");
        isAttacking = false;
        isRetreating = true;
    }

    public void HandHitPlayer()
    {
        Debug.Log("Player got squashed!");
        isAttacking = false;
    }
}