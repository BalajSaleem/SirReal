using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    float moveSpeed = 100f;

    Rigidbody2D rb;

    private Transform target;
    Vector2 moveDirection;




    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        //Destroy(gameObject, 5f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Equals("Player"))
        {
            Debug.Log("Hit!");
            Destroy(gameObject);
        }
    }
}
