using UnityEngine;

public class Revolver2 : MonoBehaviour
{
    public string otherPlayerTag;
    public KeyCode chargeHammerVert;
    public float hammerUpChargeRate = 1f;
    public float hammerMaxUpCharge = 50f;
    public float hammerMaxUpSwing = 300f;
    public float chargeToSpeedRatio = 10f;
    public float impactPauseTime = 0.8f;
    public float returnSpeed = 2f;

    public Transform thisPlayer;
    public Transform otherPlayer;

    private Rigidbody2D rb;
    private Collider2D col;

    private SpriteRenderer spriteRenderer;

    public Animator anim;

    private float currentHammerRot = 0f;
    private float hammerUpCharge = 0f;
    private float swingSpeed = 0f;
    private float impactPauseTimer = 999999f;
    private bool swinging = false;
    private bool returning = false;
    private bool flipped = false;
    private bool bonk = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        spriteRenderer.enabled = false;
    }

    private void Update()
    {
        UpdateFlipping();
        HandleCharging();
        HandleSwinging();
        ApplyRotation();
        HandleAnimation();
        HandleHitbox();
    }

    private void UpdateFlipping()
    {
        flipped = thisPlayer.position.x > otherPlayer.position.x;
    }

    private void HandleCharging()
    {
        if (!swinging && !returning)
        {
            bonk = false;

            currentHammerRot = -hammerUpCharge;

            if (Input.GetKey(chargeHammerVert) && hammerUpCharge < hammerMaxUpCharge)
            {
                hammerUpCharge += hammerUpChargeRate * Time.deltaTime * 60f;
            }
            if (Input.GetKeyUp(chargeHammerVert))
            {
                swinging = true;
                swingSpeed = hammerUpCharge * chargeToSpeedRatio;
                hammerUpCharge = 0f;
            }
        }
    }

    private void HandleSwinging()
    {
        if (swinging && !returning)
        {
            currentHammerRot += swingSpeed;

            bonk = true;

            if (currentHammerRot >= hammerMaxUpSwing)
            {
                currentHammerRot = hammerMaxUpSwing;
                impactPauseTimer = Time.time + impactPauseTime;
                swinging = false;
                returning = true;
            }
        }
        else if (returning && Time.time >= impactPauseTimer)
        {
            swingSpeed = 0;

            currentHammerRot -= returnSpeed;

            if (currentHammerRot <= 0f)
            {
                currentHammerRot = 0f;
                returning = false;
            }
        }
    }

    private void ApplyRotation()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, flipped ? -currentHammerRot : currentHammerRot);
    }

    private void HandleAnimation()
    {
        anim.SetBool("isSwinging", swinging);

        anim.gameObject.GetComponent<SpriteRenderer>().flipX = swinging;
    }

    private void HandleHitbox()
    {
        col.isTrigger = bonk;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null && bonk)
        {
            rb.AddForce(flipped? (transform.right * swingSpeed + transform.up * 0) : (transform.right * swingSpeed * -1 + transform.up * 0 * -1), ForceMode2D.Impulse);
        }
    }

    public bool Flip() => flipped;
}
