using Unity.VisualScripting;
using UnityEngine;
public class C1mov1 : MonoBehaviour
{
    public float moveAccel = 5f;
    public float moveAccelAir = 5f;
    public float jumpForce = 10f;
    public float jumpBoostEffect = 0.03f;
    public float maxVertSpeed = 10f;
    public float maxHoriSpeed = 5f;
    public float maxHoriSpeedAir = 2f;
    public float airDamping = 0f;
    public float groundDamping = 0f;
    public KeyCode jumpButton;
    public KeyCode left;
    public KeyCode right;

    private bool isJumpBoosting = false;
    private bool isJumping = false;
    private float maxHoriSpeedReal;
    private float moveAccelReal;
    private float jumpBoost;
    private float jumpBoostFade = 0.92f;
    private float jumpBoostEffectReal;
    private int jumpBoostDuration = 42;

    private bool isGrounded = false;
    public GameObject groundCheck;
    public Transform otherPlayer;

    private Rigidbody2D rb;

    private Animator animator;

    public SpriteRenderer shotGunSprite;

    public Revolver1 shotGunScript;

    private Vector2 rightFacingScale;

    private bool flipped;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rightFacingScale = transform.localScale;
    }
    void Update()
    {
        flipped = shotGunScript.Flipped();

        if (flipped)
        {
            transform.localScale = new Vector2(rightFacingScale.x * -1, rightFacingScale.y);
        }
        else
        {
            transform.localScale = new Vector2(rightFacingScale.x, rightFacingScale.y);
        }

        Groundcheck1 groundCheckScript = groundCheck.GetComponent<Groundcheck1>();
        if (groundCheckScript.GetGroundCheck())
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    void FixedUpdate()
    {
        float horizontal = 0f;
        animator.SetBool("isWalking", false);
        if (Input.GetKey(left))
        {
            horizontal = -1f;
            animator.SetBool("isWalking", true);
        }
        else if (Input.GetKey(right))
        {
            horizontal = 1f;
            animator.SetBool("isWalking", true);
        }
        else if (Input.GetKey(left) && Input.GetKey(right))
        {
            horizontal = 0f;
            animator.SetBool("isWalking", false);
        }
        rb.AddForce(Vector2.right * horizontal * moveAccelReal);

        if (isGrounded)
        {
            maxHoriSpeedReal = maxHoriSpeed;
            moveAccelReal = moveAccel;
            rb.linearDamping = groundDamping;
        }
        else
        {
            moveAccelReal = moveAccelAir;
            maxHoriSpeedReal = maxHoriSpeedAir;
            rb.linearDamping = airDamping;
        }

        rb.linearVelocity = new Vector2(
            Mathf.Clamp(rb.linearVelocity.x, -maxHoriSpeedReal, maxHoriSpeedReal),
            Mathf.Clamp(rb.linearVelocity.y, -maxVertSpeed, maxVertSpeed)
        );

        if (Input.GetKey(jumpButton))
        {
            if (isJumping == false && isGrounded)
            {
                rb.linearVelocity = new Vector2(0f, jumpForce);
                isJumping = true;
                isJumpBoosting = true;
                jumpBoost = jumpBoostDuration;
                jumpBoostEffectReal = jumpBoostEffect;
            }
            if (jumpBoost > 0 && isJumpBoosting)
            {
                rb.AddForce(transform.up * jumpForce * jumpBoostEffectReal, ForceMode2D.Impulse);
                jumpBoost--;
                jumpBoostEffectReal *= jumpBoostFade;
            }
        }
        if (!Input.GetKey(jumpButton))
        {
            isJumping = false;
            isJumpBoosting = false;
        }

        animator.SetFloat("vertVel", rb.linearVelocityY);

        animator.SetBool("isGrounded", isGrounded);
    }
}