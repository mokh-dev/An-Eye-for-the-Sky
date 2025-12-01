using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Charger : MonoBehaviour
{
    [SerializeField] private int chargerHP;
    [SerializeField] private float chargerHorizontalSpeed;
    [SerializeField] private float chargerVerticalSpeed;


    private GameObject player;
    private Airplane playerScript;
    private Rigidbody2D rb;

    private bool passed;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Airplane>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        Vector2 difference = player.transform.position - transform.position;

        rb.velocity = new Vector2(-chargerHorizontalSpeed, difference.y * chargerVerticalSpeed);

            
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        if ((((rotZ+180) >= 0) && ((rotZ+180) <= 45)) || (((rotZ+180) >= 315) && ((rotZ+180) <= 360)))
        {
            if (passed == false)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 180);
            }
            
        }
        else
        {
            passed = true;
        }

        if (transform.position.x < -7)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            chargerHP -= collision.gameObject.GetComponent<Bullet>().BulletDamage;
            Destroy(collision.gameObject);

            if (chargerHP <= 0)
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
