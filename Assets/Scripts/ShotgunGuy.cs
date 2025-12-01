using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShotgunGuy : MonoBehaviour
{
    [SerializeField] private Vector2 topLeftSpawningAreaBound;
    [SerializeField] private Vector2 bottomRightSpawningAreaBound;

    [SerializeField] private float shotgunToPositionAnimTime;
    [SerializeField] private int shotgunHP;
    [SerializeField] private float shotgunBulletSpeed;
    [SerializeField] private int shotgunBulletDamage;
    [SerializeField] private float shotgunCooldownTime;
    [SerializeField] private float shotgunBulletLifespan;
    [SerializeField] private GameObject shotgunBulletPre;
    [SerializeField] private float shotgunSpread;



    private float gunTimer;
    private bool canShootGun;


    private GameObject player;
    private Airplane playerScript;

    private GameObject firePoint;


    void Awake()
    {
        StartCoroutine(MoveToLocation());
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Airplane>();
        firePoint = transform.GetChild(0).gameObject;

        canShootGun = false;
        gunTimer = shotgunCooldownTime;
    }

    
    void Update()
    {
        if (playerScript.isDead == false)
        {

            if (canShootGun)
            {
                ShootGun();
                canShootGun = false;
                gunTimer = shotgunCooldownTime;
            }


            if (gunTimer > 0)
            {
                gunTimer -= Time.deltaTime;
            }
            else if (canShootGun == false)
            {
                canShootGun = true;
            }
            
            

            Vector2 difference = player.transform.position - firePoint.transform.position;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 180);
        }
    }

    private IEnumerator MoveToLocation()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(UnityEngine.Random.Range(topLeftSpawningAreaBound.x, bottomRightSpawningAreaBound.x), UnityEngine.Random.Range(topLeftSpawningAreaBound.y, bottomRightSpawningAreaBound.y), transform.position.z);
        float timeElapsed = 0;

        while (timeElapsed < shotgunToPositionAnimTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / shotgunToPositionAnimTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }

    void ShootGun()
    {
        for (int i = -2; i < 3; i++)
        {
            GameObject bullet = Instantiate(shotgunBulletPre, firePoint.transform.position, transform.rotation);

            Vector2 difference = (player.transform.position - firePoint.transform.position).normalized;

            if (i>=0)
            {
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(difference.x - (shotgunSpread * math.abs(i)), difference.y + (shotgunSpread * math.abs(i))).normalized * shotgunBulletSpeed;
            }
            else
            {
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(difference.x - (shotgunSpread * math.abs(i)), difference.y - (shotgunSpread * math.abs(i))).normalized * shotgunBulletSpeed;
            }

            bullet.GetComponent<Bullet>().BulletDamage = shotgunBulletDamage;

            Destroy(bullet, shotgunBulletLifespan);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            shotgunHP -= collision.gameObject.GetComponent<Bullet>().BulletDamage;
            Destroy(collision.gameObject);

            if (shotgunHP <= 0)
            {
                Destroy(gameObject);
            }

        }
    }

    void OnDestroy()
    {
        if (GameObject.FindGameObjectWithTag("WaveSpawner") != null)
        {
            GameObject.FindGameObjectWithTag("WaveSpawner").GetComponent<WaveSpawner>().spawnedEnemies.Remove(gameObject);
        }
     
    }
}
