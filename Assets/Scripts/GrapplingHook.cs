using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;
    public Transform gunTip; // The physical point where the visual rope shoots from
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("Grappling Physics")]
    public float maxGrappleDistance = 35f;
    public float springForce = 4.5f;
    public float damper = 7f;
    public float massScale = 4.5f;

    private Vector3 grapplePoint;
    private SpringJoint joint;

    void Start()
    {
        // Get the LineRenderer on start and hide it
        if (lr == null) lr = GetComponent<LineRenderer>();
        lr.positionCount = 0; 
    }

    void Update()
    {
        // Using Right Click (1) to fire the grapple hook
        if (Input.GetMouseButtonDown(1))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            StopGrapple();
        }
    }

    void LateUpdate()
    {
        // Draw the rope in LateUpdate so it doesn't lag behind camera movement
        DrawRope();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        // Shoot a raycast from the center of the camera forward
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            
            // Attach the SpringJoint to the main Player Rigidbody (the parent of the camera)
            joint = playerCamera.parent.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(playerCamera.position, grapplePoint);

            // The magic numbers that make the swing feel bouncy and fast
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = springForce;
            joint.damper = damper;
            joint.massScale = massScale;

            lr.positionCount = 2;
        }
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        if (joint != null)
        {
            Destroy(joint);
        }
    }

    void DrawRope()
    {
        // Don't draw the rope if we aren't actively grappling
        if (joint == null) return;
        
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }
}