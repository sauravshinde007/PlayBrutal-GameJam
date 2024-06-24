using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{

    [Header("Multiplayer")]
    [SerializeField] private PhotonView view;

    [Header("Movement Controls")]
    [SerializeField] float speed = 450f;
    [SerializeField] float jump_force = 10f;
    [Range(0f, 1f)][SerializeField] float stop_factor = 0.5f; // Controlled jump stopping factor
    [SerializeField] Transform feetPos; // feet position to check if grounded
    [SerializeField] Vector3 groundCheckSize = new Vector3(1, 0.05f, 1);
    [SerializeField] float gravity = 10; // gravity to apply when in air
    [SerializeField] LayerMask whatIsGround; // can only jump if on ground layer

    [Header("Better Platformer")]
    [SerializeField] private float hangTime = 0.1f;
    private float hangTimeCtr = 0;
    [SerializeField] private float jumpBufferLength = 0.1f;
    private float jumpBufferCtr = 0;

    [Header("Hover Controls")]
    [SerializeField] float hoverForce = 5f;
    [SerializeField] float maxFuel = 5f;
    [SerializeField] float fuelConsumptionRate = 1f;
    [SerializeField] float fuelRestoreRate = 0.5f;
    private float currentFuel;
    private bool isHovering = false;

    // Private variables
    private Rigidbody2D rb;
    private float movement; // movement input
    private bool isGrounded = false;
    private bool can_jump = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentFuel = maxFuel;
    }

    void GetInput()
    {
        movement = Input.GetAxisRaw("Horizontal");

        // Check if grounded by a physics check
        isGrounded = Physics2D.OverlapBoxAll(feetPos.position, groundCheckSize, 0f, whatIsGround).Length > 0;
        // Set gravity to 0 if on ground
        rb.gravityScale = isGrounded ? 0 : gravity;

        // Hang/Coyote Time
        if (isGrounded) { hangTimeCtr = hangTime; }
        else hangTimeCtr -= Time.deltaTime;

        // Jump Buffer
        if (Input.GetButtonDown("Jump")) { jumpBufferCtr = jumpBufferLength; }
        else jumpBufferCtr -= Time.deltaTime;

        // Jump Detection
        if (hangTimeCtr > 0f && jumpBufferCtr > 0)
        {
            can_jump = true;
            jumpBufferCtr = 0;
        }

        // Controlled Jump
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * stop_factor);
        }

        // Hover Input
        if (Input.GetKey(KeyCode.E) && currentFuel > 0)
        {
            isHovering = true;
        }
        else
        {
            isHovering = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!view.IsMine) return;
        GetInput();
    }

    void FixedUpdate()
    {
        if (!view.IsMine) return;
        // Movement
        var vel = rb.velocity;
        // Changing the velocity directly
        vel.x = movement * speed * Time.fixedDeltaTime;
        rb.velocity = vel;

        // Jumping
        if (can_jump)
        {
            rb.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);
            can_jump = false;
        }

        // Hovering
        if (isHovering)
        {
            rb.gravityScale = 0; // Disable gravity while hovering
            rb.AddForce(Vector2.up * hoverForce, ForceMode2D.Force);
            currentFuel -= fuelConsumptionRate * Time.fixedDeltaTime;
            if (currentFuel < 0) currentFuel = 0;
            Debug.Log("Hovering: Fuel remaining: " + currentFuel);
        }
        else
        {
            rb.gravityScale = gravity; // Restore gravity
            if (currentFuel < maxFuel)
            {
                currentFuel += fuelRestoreRate * Time.fixedDeltaTime;
                if (currentFuel > maxFuel) currentFuel = maxFuel;
                Debug.Log("Not hovering: Fuel restoring: " + currentFuel);
            }
        }
    }

    // Debugging
    void OnDrawGizmos()
    {
        // Groundcheckbox
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(feetPos.position, groundCheckSize);
    }
}
