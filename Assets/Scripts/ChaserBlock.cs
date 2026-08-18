using UnityEngine;
using System.Collections;

public class ChaserBlock : MonoBehaviour
{
    [Header("Chaser Settings")]
    public float speed = 8.5f;
    public float delayBeforeChase = 3.0f;

    private bool isChasing = false;

    void Start()
    {
        StartCoroutine(StartChaseDelay());
    }

    IEnumerator StartChaseDelay()
    {
        yield return new WaitForSeconds(delayBeforeChase);
        isChasing = true;
    }

    void Update()
    {
        if (!isChasing) return;
        if (Time.timeScale < 1f) return;

        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    // UPDATED: We are using OnTriggerEnter instead of OnCollisionEnter
    private void OnTriggerEnter(Collider other)
    {
        // This explicitly checks for the "Player" tag
        if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<GameManager>().TriggerGameOver("CAUGHT BY THE CHASER!");
        }
    }
}
