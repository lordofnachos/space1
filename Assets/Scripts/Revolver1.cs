using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering.Universal;
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
    private readonly float bulletLifetime = 3f;

    public int minBloom = 3;
    private readonly int maxBloom = 39;
    private int bloom = 0;
    private readonly int bloomChange = 18;

    private readonly float accuracyRecoveryRate = 0.45f;
    public float fireRate = 0.2f;
    private float nextRecoveryTime = 0f;
    private float nextFireTime = 0f;

    public int bulletsPerShot = 4; // Number of bullets fired per shot

    public Light2D bloomEffect;

    public Animator shotGunAnim;
    public int shotGunShootDuration;

    private bool flipped = false;

    private void Awake()
    {
        bloom = minBloom;
    }

    void Update()
    {
        HandleShooting();
        HandleFlipping();
        PointGunTowardsOtherPlayer();
    }

    void HandleFlipping()
    {
        flipped = otherPlayer.position.x < transform.parent.position.x;
    }

    void HandleShooting()
    {
        if (Input.GetKeyDown(fire) && Time.time >= nextFireTime)
        {
            for (int i = 0; i < bulletsPerShot; i++)
            {
                Shoot();
            }

            if (bloom < maxBloom)
            {
                bloom += bloomChange;
            }

            if (shotGunAnim != null)
            {
                shotGunAnim.SetInteger("shootDur", shotGunShootDuration);
            }

            nextFireTime = Time.time + fireRate;
        }

        if (bloom > minBloom && Time.time >= nextRecoveryTime)
        {
            nextRecoveryTime = Time.time + accuracyRecoveryRate;
            bloom -= bloomChange;
        }
    }

    void PointGunTowardsOtherPlayer()
    {
        if (shotGunAnim != null && shotGunAnim.GetInteger("shootDur") > 0)
        {
            shotGunAnim.SetInteger("shootDur", shotGunAnim.GetInteger("shootDur") - 1);
            bloomEffect.enabled = true;
        }
        else
        {
            bloomEffect.enabled = false;
        }

        Vector3 direction = otherPlayer.position - transform.position;
        float directionAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion directionRotation = flipped ? Quaternion.Euler(0f, 0f, directionAngle + 180) : Quaternion.Euler(0f, 0f, directionAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, directionRotation, Time.deltaTime * rotationSpeed);
    }

    void Shoot()
    {
        float randomBloom = Random.Range(-bloom, bloom);
        Quaternion bulletRotation = Quaternion.Euler(0f, 0f, transform.eulerAngles.z + randomBloom);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.constraints = RigidbodyConstraints2D.None;
        bulletRb.linearVelocity = bullet.transform.right * (flipped ? -bulletSpeed : bulletSpeed);

        Destroy(bullet, bulletLifetime);
    }

    public bool Flipped()
    {
        return flipped;
    }
}
