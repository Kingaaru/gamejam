using UnityEngine;

public class GiantHandController : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float fastSpeed = 25f;     
    public float creepSpeed = 1.5f;   
    public float retreatSpeed = 40f;  

    [Header("Distance Threshold")]
    public float triggerDistance = 8f; 

    private Transform playerTransform;
    private Vector3 initialSpawnPos;
    private bool isFastApproaching = false;
    private bool isCreeping = false;
    private bool isRetreating = false;

    private void Start()
    {
        // Force the hand to be hidden when the game starts
        gameObject.SetActive(false);
    }

    public void BeginAttack(Vector3 spawnPos, Transform player)
    {
        gameObject.SetActive(true);
        
        foreach (var renderer in GetComponentsInChildren<MeshRenderer>()) renderer.enabled = true;
        foreach (var renderer in GetComponentsInChildren<SkinnedMeshRenderer>()) renderer.enabled = true;

        transform.position = spawnPos;
        initialSpawnPos = spawnPos;
        playerTransform = player;
        
        transform.LookAt(player.position);

        isFastApproaching = true;
        isCreeping = false;
        isRetreating = false;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        if (isFastApproaching)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, fastSpeed * Time.deltaTime);
            transform.LookAt(playerTransform.position);

            if (Vector3.Distance(transform.position, playerTransform.position) <= triggerDistance)
            {
                isFastApproaching = false;
                isCreeping = true;
                FindAnyObjectByType<QTEManager>().StartQTE();
            }
        }
        else if (isCreeping)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, creepSpeed * Time.unscaledDeltaTime);
        }
        else if (isRetreating)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialSpawnPos, retreatSpeed * Time.unscaledDeltaTime);
            if (Vector3.Distance(transform.position, initialSpawnPos) < 1f)
            {
                gameObject.SetActive(false);
                isRetreating = false;
            }
        }
    }

    public void DeflectHand()
    {
        isCreeping = false;
        isRetreating = true;
    }

    public void HandHitPlayer()
    {
        isCreeping = false;
        FindAnyObjectByType<GameManager>().TakeDamage("CRUSHED BY THE HAND!");
    }
}