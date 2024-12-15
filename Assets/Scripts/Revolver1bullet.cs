using UnityEngine;

public class Revolver1bullet : MonoBehaviour
{
    public float damage = 10f; // Damage dealt by the bullet
    public float lifespan = 5f; // Time before the bullet is destroyed

    void Start()
    {
        // Destroy the bullet after its lifespan expires
        Destroy(gameObject, lifespan);
    }

    void OnTriggerEnter2D(Collision2D collision2D)
    {
        // Destroy the bullet on impact
        Destroy(gameObject);
    }
}