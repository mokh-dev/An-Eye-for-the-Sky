using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;
    [SerializeField] private float parallaxSpeed;

    private Rigidbody2D rb;

    void Start()
    {
        startPos = transform.position.x;
        length = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;

        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-parallaxSpeed, 0);
    }

    void Update()
    {
        if (transform.position.x <= -12)
        {
            transform.position = new Vector3(12, transform.position.y, transform.position.z);
        }
    }
}
