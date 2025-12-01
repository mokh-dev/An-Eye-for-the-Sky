using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    
    [SerializeField] private Vector2 topLeftSpawningAreaBound;
    [SerializeField] private Vector2 bottomRightSpawningAreaBound;

    [SerializeField] private float machineGunToPositionAnimTime;
    [SerializeField] private int machineGunHP;
    [SerializeField] private float machineGunBulletSpeed;
    [SerializeField] private int machineGunBulletDamage;
    [SerializeField] private float machineGunBadAimOffset;
    [SerializeField] private float machineGunCooldownTime;
    [SerializeField] private float machineGunBulletLifespan;
    [SerializeField] private GameObject machineGunBulletPre;



    private float machineGunTimer;
    private bool canShootMachineGun;


    private Coroutine moveToLoc;

    private GameObject player;
    private Airplane playerScript;

    private GameObject firePoint;


    void Awake()
    {
        moveToLoc = StartCoroutine(MoveToLocation());
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Airplane>();
        firePoint = transform.GetChild(0).gameObject;
        canShootMachineGun = false;
        machineGunTimer = machineGunCooldownTime;
    }

    
    void Update()
    {
        if (playerScript.isDead == false)
        {
            if (canShootMachineGun)
            {
                ShootMachineGun();
                canShootMachineGun = false;
                machineGunTimer = machineGunCooldownTime;
            }


            if (machineGunTimer > 0)
            {
                machineGunTimer -= Time.deltaTime;
            }
            else if (canShootMachineGun == false)
            {
                canShootMachineGun = true;
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

        while (timeElapsed < machineGunToPositionAnimTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / machineGunToPositionAnimTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }


    void ShootMachineGun()
    {
        GameObject bullet = Instantiate(machineGunBulletPre, firePoint.transform.position, transform.rotation);

        Vector3 offsetPos = new Vector3(Random.Range(player.transform.position.x - machineGunBadAimOffset, player.transform.position.x + machineGunBadAimOffset), Random.Range(player.transform.position.y - machineGunBadAimOffset, player.transform.position.y + machineGunBadAimOffset), player.transform.position.z);

        Vector2 difference = (offsetPos - firePoint.transform.position).normalized;

        bullet.GetComponent<Rigidbody2D>().velocity = difference * machineGunBulletSpeed;
        bullet.GetComponent<Bullet>().BulletDamage = machineGunBulletDamage;

        Destroy(bullet, machineGunBulletLifespan);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            machineGunHP -= collision.gameObject.GetComponent<Bullet>().BulletDamage;
            Destroy(collision.gameObject);

            if (machineGunHP <= 0)
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
