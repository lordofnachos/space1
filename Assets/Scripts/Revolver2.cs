using UnityEngine;

public class Revolver2 : MonoBehaviour
{
    public string otherPlayerTag;
    public KeyCode chargeHammerVert;
    public float hammerUpChargeRate = 1f;
    public float hammerMaxUpCharge = 50f;
    public float hammerMaxUpSwing = 320f;
    public float chargeToSpeedRatio = 2f;
    public float impactPauseTime = 0.8f;
    public float returnSpeed = 2f;

    public Transform thisPlayer;
    public Transform otherPlayer;

    private SpriteRenderer spriteRenderer;

    private Animator anim;

    private float currentHammerRot = 0f;
    private float hammerUpCharge = 0f;
    private float swingSpeed = 0f;
    private float impactPauseTimer = 0f;
    private bool swinging = false;
    private bool returning = false;
    private bool flipped = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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
    }

    private void UpdateFlipping()
    {
        flipped = thisPlayer.position.x > otherPlayer.position.x;
    }

    private void HandleCharging()
    {
        if (!swinging && !returning)
        {
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
            currentHammerRot = -hammerUpCharge;
        }
    }

    private void HandleSwinging()
    {
        if (swinging && !returning)
        {
            currentHammerRot += swingSpeed;
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
        if (currentHammerRot > 180f)
        {
            currentHammerRot -= 360f;
        }
        transform.rotation = Quaternion.Euler(0f, 0f, flipped ? -currentHammerRot : currentHammerRot);
    }

    private void HandleAnimation()
    {
        if (swinging)
        {
            anim.SetBool("isSwinging", true);
        }
        else
        {
            anim.SetBool("isSwinging", false);
        }
        if (returning)
        {
            anim.SetBool("isReturning", true);
        }
        else
        {
            anim.SetBool("isReturning", false);
        }
        if (hammerUpCharge > 0f)
        {
            anim.SetBool("isResting", true);
        }
        else
        {
            anim.SetBool("isResting", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(otherPlayerTag))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.AddForce(transform.right * hammerUpCharge + transform.up * hammerUpCharge, ForceMode2D.Impulse);
            }
        }
    }

    public bool Flip() => flipped;
}
