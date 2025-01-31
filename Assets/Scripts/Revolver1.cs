using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static UnityEngine.GraphicsBuffer;

public class Revolver1 : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public KeyCode fire;
    public Transform otherPlayer;
    public SpriteRenderer thisPlayer;
    public float rotationSpeed;

    public float bulletSpeed = 10f;
    readonly private float bulletLifetime = 3f;

    readonly public int minBloom = 3;
    readonly private int maxBloom = 39;
    private int bloom = 0;
    readonly private int bloomChange = 18;

    readonly private float accuracyRecoveryRate = 0.45f;
    public float fireRate = 0.2f;
    private float nextRecoveryTime = 0f;
    private float nextFireTime = 0f;

    public Animator shotGunAnim;
    public int shotGunShootDuration;

    private bool flipped = false;

    private void Awake()
    {
        bloom = minBloom;
    }
    void Update()
    {

        if (Input.GetKeyDown(fire) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
            if(shotGunAnim != null)
            {
                shotGunAnim.SetInteger("shootDur", shotGunShootDuration);
            }
        }

        if(bloom > minBloom && Time.time >= nextRecoveryTime)
        {
            nextRecoveryTime = Time.time + accuracyRecoveryRate;
            bloom =- bloomChange;
        }

        if (otherPlayer.position.x < transform.parent.position.x)
        {
            flipped = true;
        }
        else
        {
            flipped = false;
        }
    }
    private void FixedUpdate()
    {
        if(shotGunAnim != null)
        {
            if (shotGunAnim.GetInteger("shootDur") > 0)
            {
                shotGunAnim.SetInteger("shootDur", shotGunAnim.GetInteger("shootDur") - 1);
            }
        }
        Vector3 direction = otherPlayer.position - transform.position;
        float directionAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion directionRotation;
        if (flipped)
        {
            directionRotation = Quaternion.Euler(0f, 0f, directionAngle + 180);
        }
        else
        {
            directionRotation = Quaternion.Euler(0f, 0f, directionAngle);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, directionRotation, Time.deltaTime * rotationSpeed);
    }
    void Shoot()
    {
        ChangeTrajectory();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.constraints = RigidbodyConstraints2D.None;

        if (flipped)
        {
            bulletRb.linearVelocity = firePoint.right * bulletSpeed * -1;
        }
        else
        {
            bulletRb.linearVelocity = firePoint.right * bulletSpeed;
        }

        Destroy(bullet, bulletLifetime);
    }
    void ChangeTrajectory()
    {
        if (bloom < maxBloom)
        {
            bloom = bloom + bloomChange;
        }

        float randomInt = Random.Range(transform.eulerAngles.z - bloom, transform.eulerAngles.z + bloom);

        firePoint.eulerAngles = new Vector3(0f, 0f, randomInt);

        nextRecoveryTime = Time.time + accuracyRecoveryRate;
    }

    public bool Flipped()
    {
        if (flipped)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}