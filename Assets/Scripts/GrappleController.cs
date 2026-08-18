using UnityEngine;

public class GrappleController : MonoBehaviour
{
    [Header("Grapple Settings")]
    public Camera playerCamera;
    public float maxDistance = 40f;
    public LayerMask grappleLayer;
    public float pullSpeed = 50f;
    
    [Header("Visuals")]
    public LineRenderer lineRenderer;

    private bool isGrappling = false;
    private Vector3 grappleTarget;

    private void Start()
    {
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null) lineRenderer.enabled = false;
    }

    private void Update()
    {
        // Press 'E' to fire grapple
        if (Input.GetKeyDown(KeyCode.E) && !isGrappling)
        {
            TryGrapple();
        }

        if (isGrappling)
        {
            // Yank player toward the target along the +x runner path
            transform.position = Vector3.MoveTowards(transform.position, grappleTarget, pullSpeed * Time.deltaTime);
            
            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, grappleTarget);
            }

            // Stop when close to target
            if (Vector3.Distance(transform.position, grappleTarget) < 3f)
            {
                StopGrapple();
            }
        }
    }

    void TryGrapple()
    {
        if (playerCamera == null) playerCamera = Camera.main;

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxDistance, grappleLayer))
        {
            grappleTarget = hit.point;
            isGrappling = true;

            if (lineRenderer != null)
            {
                lineRenderer.enabled = true;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, grappleTarget);
            }
        }
    }

    public void StopGrapple()
    {
        isGrappling = false;
        if (lineRenderer != null) lineRenderer.enabled = false;
    }
}