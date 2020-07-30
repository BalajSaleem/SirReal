using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update

    public bool following = false;

    private Vector2 playerPos;
    public float bulletSpeed = 10f;

    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (following) {
            transform.position = Vector3.Slerp(transform.position, playerPos, bulletSpeed * Time.deltaTime);
        }

    }
}
