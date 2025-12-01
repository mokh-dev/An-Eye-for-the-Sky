using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Airplane : MonoBehaviour
{
    [SerializeField] private float deathAnimDuration;
    [SerializeField] private float deathYValue;
    public int playerHP;
    public float moveSpeed;
    public float bulletSpeed;
    public int bulletDamage;
    public float shootCooldownTime;
    public float bulletLifespan;
    public GameObject bulletPre;

    [SerializeField] private AudioSource laserSound;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource hitSound;


    private int originalPlayerHP;
    private float originalMoveSpeed;
    private float originalBulletSpeed;
    private int originalBulletDamage;
    private float originalShootCooldownTime;
    private float originalBulletLifespan;
    private GameObject originalBulletPre;


    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform firePoint;

    private float horizontalInput;
    private float verticalInput;

    private bool canShoot;
    private float shootTimer;

    public bool isDead;
    public bool gameOver;
    
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        firePoint = transform.GetChild(0).transform;
        canShoot = true;

        originalPlayerHP = playerHP;
        originalMoveSpeed = moveSpeed;
        originalBulletSpeed = bulletSpeed;
        originalBulletDamage = bulletDamage;
        originalShootCooldownTime = shootCooldownTime;
        originalBulletLifespan = bulletLifespan;
        originalBulletPre = bulletPre;
    }

    
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (isDead == false)
        {
            if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire1")) && canShoot)
            {
                Shoot();
                canShoot = false;
                shootTimer = shootCooldownTime;
            }

            if (shootTimer > 0)
            {
                shootTimer -= Time.deltaTime;
            }
            else if (canShoot == false)
            {
                canShoot = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (isDead == false)
        {
            rb.velocity = new Vector2(horizontalInput, verticalInput).normalized * moveSpeed;
        }
    }

    void Shoot()
    {
        laserSound.Play();
        GameObject bullet = Instantiate(bulletPre, firePoint.position, Quaternion.identity);

        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, 0);
        bullet.GetComponent<Bullet>().BulletDamage = bulletDamage;

        Destroy(bullet, bulletLifespan);
    }

    public void ResetValues()
    {
        playerHP = originalPlayerHP;
        moveSpeed = originalMoveSpeed;
        bulletSpeed = originalBulletSpeed;
        bulletDamage = originalBulletDamage;
        shootCooldownTime = originalShootCooldownTime;
        bulletLifespan = originalBulletLifespan;
        bulletPre = originalBulletPre;
    }

    private IEnumerator Death()
    {
        isDead = true;

        sr.color = Color.red;
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(transform.position.x, deathYValue, transform.position.z);
        float timeElapsed = 0;

        while (timeElapsed < deathAnimDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / deathAnimDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;

        gameOver = true;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            playerHP -= collision.gameObject.GetComponent<Bullet>().BulletDamage;
            Destroy(collision.gameObject);

            if (playerHP <= 0)
            {
                deathSound.Play();

                StartCoroutine(Death());
            }
            else
            {
                hitSound.Play();
            }
        }
    }
}
