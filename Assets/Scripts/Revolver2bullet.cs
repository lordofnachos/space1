using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class Revolver2bullet : MonoBehaviour
{
    public string wallTag; //THE TAG OF THE WALL
    public string otherPlayer; //THE TAG OF THE OTHER PLAYER

    public float knockbackForce; //THE FORCE OF KNOCKBACK UPON SHOOTING AN ENEMY
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //DESTROY THE BULLET WHEN IT COLLIDES WITH A WALL
        if (other.CompareTag(wallTag))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag(otherPlayer))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = rb.linearVelocity * knockbackForce;
            }
        }
    }
}