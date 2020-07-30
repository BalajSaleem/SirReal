using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBoss : MonoBehaviour
{
    // Start is called before the first frame update

    public float getTiredAfter = 20f;
    private float nextTiredTime = 0f;

    bool isTired = false;

    public float restTime = 10f;
    private float nextFlyTime = 0f;

    Rigidbody2D rb;
    Animator animator;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        nextTiredTime = Time.time + getTiredAfter;


    }

    // Update is called once per frame
    void Update()
    {
        if (!isTired && Time.time > nextTiredTime)
        {
            isTired = true;
            rb.gravityScale = 1f;
            animator.SetBool("fly", false);
            animator.SetBool("tired", true);
            nextFlyTime = Time.time + restTime;
        }
        else if (isTired && Time.time > nextFlyTime) {
            animator.SetBool("fly", true);
            animator.SetBool("tired", false);
            isTired = false;
            nextTiredTime = Time.time + getTiredAfter;

        }

    }

}
