using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Behaviour : MonoBehaviour
{

    public float currentHealth = 100f;
    public float attackRate =  0.3f;
    public float shotForce = 20f;
    float nextAttackTime = 0f;
    float followSpeed = 3f;
    public GameObject bullet;
    public float searchRadius = 7f;
    public bool shooter = false;


    bool isHurt = false;


    public GameObject[] items;

    //FOR DEBUG
    //public GameObject RespawnPoint ;
    //public GameObject anotherBublu;
    //TILL HERE

    private Transform playerPos;

    Vector2 bulletDirection;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHurt && Time.time >= nextAttackTime && shooter && Vector2.Distance(transform.position, playerPos.position) > searchRadius)
        {

            Shoot();
            nextAttackTime = nextAttackTime + (1f / attackRate);
        }


    }

    public void GetHurt(float hurtValue) {
        
        currentHealth -= hurtValue;
        animator.SetTrigger("hurt");
        isHurt = true;
        if ( currentHealth <= 0f )
        {
            Die();
        }                                     // 
        else if (Time.time >= nextAttackTime && shooter && Vector2.Distance(transform.position, playerPos.position) > searchRadius)
        {
            Shoot();
            nextAttackTime = nextAttackTime + (1f / attackRate);
        }

    }

    public void Die() {

        //Leave a copy at respwan point THIS WILL BE CHANGED
        //Instantiate(anotherBublu, RespawnPoint.transform.position, transform.rotation);

        animator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * 1000f);
        DropItems();
        //kill the object after 5s
        Invoke("Kill", 6);

        //this.enabled = false;
    }

    void Shoot() {
        animator.SetBool("shoot", true);

        if (playerPos.position.x < transform.position.x)
            bulletDirection = Vector2.left;
        else
            bulletDirection = Vector2.right;

        GameObject copy;
        copy = Instantiate(bullet, transform.position, transform.rotation);
        copy.GetComponent<SpriteRenderer>().enabled = true;
        copy.GetComponent<Collider2D>().enabled = true;
        copy.GetComponent<Rigidbody2D>().AddForce(bulletDirection * shotForce); //add force
        

    }

    void DropItems() {

        Debug.Log("Droping");

        //roll a counter
        int dropCounter = Random.Range(0, 100);

        if (dropCounter > 90) {                                         //10%
            items[0].SetActive(true);
            items[0].GetComponent<Collider2D>().enabled = true;
            items[0].GetComponent<Rigidbody2D>().isKinematic = false;
            items[0].transform.parent = null;
            //drop the maxHealth 
        }
        else if (dropCounter < 80 && dropCounter > 40)                 //40%
        {
            items[1].SetActive(true);
            items[1].GetComponent<Collider2D>().enabled = true;
            items[1].GetComponent<Rigidbody2D>().isKinematic = false;
            items[1].transform.parent = null;
            //drop health
        }
        else if (dropCounter < 40 && dropCounter > 20)                  //20%
        {
            items[2].SetActive(true);
            items[2].GetComponent<Collider2D>().enabled = true;
            items[2].GetComponent<Rigidbody2D>().isKinematic = false;
            items[2].transform.parent = null;
            //drop the damage potion
        }
        else
        {
            items[3].SetActive(true);                                   //20%
            items[3].GetComponent<Collider2D>().enabled = true;
            items[3].GetComponent<Rigidbody2D>().isKinematic = false;
            items[3].transform.parent = null;
            //drop the speed potion
        }


    }

    void Kill()
    {
        Destroy(transform.gameObject);
    }

}
