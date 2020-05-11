using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbyScript : MonoBehaviour
{

    float moveSpeed = 9f;
    private float rotationSpeed = 2f;

    Rigidbody2D rb;

    public Transform target;
    public Transform source;
    public float fireTime;
    public bool returning;
    Vector2 moveDirection;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector2(moveDirection.x, 0);
        fireTime = Time.time;
        returning = true;
        //Destroy(gameObject, 5f);
    }

    private void Update()
    {

        transform.Rotate(0, 0, rotationSpeed);

        if (Time.time - fireTime > 4f && returning)
        {
            moveDirection = (source.transform.position - transform.position).normalized * moveSpeed;
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
            returning = false;
        }

        if (!returning && Mathf.Abs(transform.position.x - source.position.x) < 3 )
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Equals("Player"))
        {
            Destroy(gameObject);
        }

        if (col.gameObject.name.Equals("Enemy") || col.gameObject.name.Equals("Boss") && !returning){
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
