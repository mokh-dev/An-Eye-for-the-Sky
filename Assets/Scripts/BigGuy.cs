using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuy : MonoBehaviour
{

    [SerializeField] private Vector2 topLeftSpawningAreaBound;
    [SerializeField] private Vector2 bottomRightSpawningAreaBound;

    [SerializeField] private float bigGuyToPositionAnimTime;
    [SerializeField] private int bigGuyHP;
    [SerializeField] private float bigGuyBulletSpeed;
    [SerializeField] private int bigGuyBulletDamage;
    [SerializeField] private float bigGuyCooldownTime;
    [SerializeField] private float bigGuyBulletLifespan;
    [SerializeField] private GameObject bigGuyBulletPre;



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
        gunTimer = bigGuyCooldownTime;
    }

    
    void Update()
    {
        if (playerScript.isDead == false)
        {

            if (canShootGun)
            {
                ShootGun();
                canShootGun = false;
                gunTimer = bigGuyCooldownTime;
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
        Vector3 endPos = new Vector3(Random.Range(topLeftSpawningAreaBound.x, bottomRightSpawningAreaBound.x), Random.Range(topLeftSpawningAreaBound.y, bottomRightSpawningAreaBound.y), transform.position.z);
        float timeElapsed = 0;

        while (timeElapsed < bigGuyToPositionAnimTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / bigGuyToPositionAnimTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }

    void ShootGun()
    {
        GameObject bullet = Instantiate(bigGuyBulletPre, firePoint.transform.position, transform.rotation);

        Vector2 difference = (player.transform.position - firePoint.transform.position).normalized;

        bullet.GetComponent<Rigidbody2D>().velocity = difference * bigGuyBulletSpeed;
        bullet.GetComponent<Bullet>().BulletDamage = bigGuyBulletDamage;

        Destroy(bullet, bigGuyBulletLifespan);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            bigGuyHP -= collision.gameObject.GetComponent<Bullet>().BulletDamage;
            Destroy(collision.gameObject);

            if (bigGuyHP <= 0)
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
