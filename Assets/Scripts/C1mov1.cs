using UnityEngine;

public class C1mov1 : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of horizontal movement
    public float jumpForce = 10f; // Force of the jump
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private bool isGrounded = false; // Is the player touching the ground?

    public GameObject groundCheck;

    void Start()
    {
        // Get the Rigidbody2D component attached to this GameObject
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Groundcheck1 groundCheckScript = groundCheck.GetComponent<Groundcheck1>();

        if(groundCheckScript.GetGroundCheck())
        {
            isGrounded = true;
        }
        else
        {
            isGrounded= false;
        }

        // Handle horizontal movement
        float horizontal = Input.GetAxis("Horizontal1"); // A/D or Left/Right Arrow keys
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);

        // Handle jumping
        if (Input.GetButtonDown("Jump1") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}