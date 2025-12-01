using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sniper : MonoBehaviour
{

    [SerializeField] private Vector2 topLeftSpawningAreaBound;
    [SerializeField] private Vector2 bottomRightSpawningAreaBound;

    [SerializeField] private float sniperToPositionAnimTime;
    [SerializeField] private int sniperHP;
    [SerializeField] private float sniperBulletSpeed;
    [SerializeField] private int sniperBulletDamage;
    [SerializeField] private float sniperCooldownTime;
    [SerializeField] private float sniperBulletLifespan;
    [SerializeField] private GameObject sniperBulletPre;




    private float sniperTimer;
    private bool canShootSniper;
    private bool isSpawning;



    private GameObject player;
    private Airplane playerScript;

    private GameObject firePoint;

    private Coroutine moveToLoc;


    void Awake()
    {
        moveToLoc = StartCoroutine(MoveToLocation());
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Airplane>();
        firePoint = transform.GetChild(0).gameObject;

        canShootSniper = false;
        sniperTimer = sniperCooldownTime;
    }

    
    void Update()
    {
        if (playerScript.isDead == false)
        {
            if (isSpawning == false)
            {
                if (canShootSniper)
                {
                    ShootSniper();
                    canShootSniper = false;
                    sniperTimer = sniperCooldownTime;
                }


                if (sniperTimer > 0)
                {
                    sniperTimer -= Time.deltaTime;
                }
                else if (canShootSniper == false)
                {
                    canShootSniper = true;
                }
            }
            


            Vector2 difference = player.transform.position - firePoint.transform.position;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 180);
        }
    }

    private IEnumerator MoveToLocation()
    {
        isSpawning = true;
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(Random.Range(topLeftSpawningAreaBound.x, bottomRightSpawningAreaBound.x), Random.Range(topLeftSpawningAreaBound.y, bottomRightSpawningAreaBound.y), transform.position.z);
        float timeElapsed = 0;

        while (timeElapsed < sniperToPositionAnimTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / sniperToPositionAnimTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        isSpawning = false;
    }

    void ShootSniper()
    {
        GameObject bullet = Instantiate(sniperBulletPre, firePoint.transform.position, transform.rotation);

        Vector2 difference = (player.transform.position - firePoint.transform.position).normalized;

        bullet.GetComponent<Rigidbody2D>().velocity = difference * sniperBulletSpeed;
        bullet.GetComponent<Bullet>().BulletDamage = sniperBulletDamage;

        Destroy(bullet, sniperBulletLifespan);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            sniperHP -= collision.gameObject.GetComponent<Bullet>().BulletDamage;
            Destroy(collision.gameObject);

            if (sniperHP <= 0)
            {
                StopCoroutine(moveToLoc);
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
