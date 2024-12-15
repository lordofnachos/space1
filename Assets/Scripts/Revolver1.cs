using UnityEngine;

public class Revolver1 : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab of the bullet to spawn
    public Transform firePoint; // Point from where the bullet will be fired
    public float bulletSpeed = 10f; // Speed of the bullet
    public float fireRate = 0.2f; // Time between shots
    
    private float nextFireTime = 0f; // Time when the weapon can fire again

    private int bloom = 0;
    public int minBloom = 6;
    private int maxBloom = 30;
    private int bloomChange = 4;

    private float accuracyRecoveryRate = 3f;
    private float bloomReductionTime = 0;

    private void Start()
    {
        bloom = minBloom;
    }
    void Update()
    {
        //SHOOTING INPUT
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
            bloomReductionTime = Time.time + accuracyRecoveryRate;
        }
        //RECOIL REDUCTION
        if(bloom > minBloom && Time.time >= bloomReductionTime)
        {
            bloomReductionTime = Time.time + accuracyRecoveryRate;

            bloom = bloom - bloomChange;
        }
    }
    void Shoot()
    {
        //INSTANTIATE BULLET
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        //GET THE RIGIDBODY OF THE BULLET
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        //IF THE RIGIDBODY IS FOUND, GIVE IT A VELOCITY OF bulletSpeed IN THE RIGHT DIRECTION
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * bulletSpeed;
        }

        //IF THE BLOOM IS NOT YET AT MAXIMUM CAPACITY, INCREASE BLOOM
        if(bloom < maxBloom)
        {
            bloom = bloom + bloomChange;
        }

        Debug.Log(bloom);

        //CHANGE THE DIRECTION OF THE FIRINGPOINT
        ChangeTrajectory();
    }
    void ChangeTrajectory()
    {
        int randomInt = Random.Range(0 - bloom, bloom);
        Quaternion gunrotation = Quaternion.Euler(0f, 0f, randomInt);
        firePoint.rotation = gunrotation;
    }
}