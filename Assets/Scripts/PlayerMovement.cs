using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float groundDrag = 5f;
    public float jumpForce = 12f;
    public float airMultiplier = 0.4f;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("Fall Death Check")]
    [Tooltip("If the player drops below this Y height, they die.")]
    public float fallThreshold = -10f; 

    public Transform playerCamera; 
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
    }

    void Update()
    {
        // 1. THE RESPAWN FIX
        if (transform.position.y < fallThreshold)
        {
            GameManager gm = FindAnyObjectByType<GameManager>();
            
            // Take 1 life away
            gm.TakeDamage("FELL INTO THE MAGMA!");
            
            // If we didn't get a Game Over, RESPAWN!
            if (!gm.isGameOver)
            {
                // Teleport the player 10 meters forward and drop them from the sky onto the track
                transform.position = new Vector3(transform.position.x + 10f, 5f, 0f);
                rb.linearVelocity = Vector3.zero; // Instantly kill falling momentum
            }
            return; 
        }

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Jump();
        }
    }

    private void MovePlayer()
    {
        Vector3 forward = playerCamera.forward;
        Vector3 right = playerCamera.right;
        
        forward.y = 0f;
        right.y = 0f;

        moveDirection = forward.normalized * verticalInput + right.normalized * horizontalInput;

        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
}