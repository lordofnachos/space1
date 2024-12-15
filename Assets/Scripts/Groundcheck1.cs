using UnityEngine;

public class Groundcheck1 : MonoBehaviour
{
    public string groundTag;

    private bool onGround;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(groundTag))
        {
            onGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(groundTag))
        {
            onGround = false;
        }
    }

    public bool GetGroundCheck()
    {
        return onGround;
    }
}
