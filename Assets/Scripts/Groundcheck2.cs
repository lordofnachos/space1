using UnityEngine;

public class Groundcheck2 : MonoBehaviour
{
    public string groundTag;
    private int groundContacts = 0;
    public Animator playerAnimator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(groundTag))
        {
            groundContacts++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(groundTag))
        {
            groundContacts--;
        }
    }

    public bool GetGroundCheck()
    {
        return groundContacts > 0;
    }

    private void FixedUpdate()
    {
        if (groundContacts > 0)
        {
            playerAnimator.SetBool("isGrounded", true);
        }
        else
        {
            playerAnimator.SetBool("isGrounded", false);
        }
    }
}
