using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy_Behaviour : MonoBehaviour
{

    public float currentHealth = 100f;
    public float shotForce = 20f;
    public GameObject bullet;
    public bool shooter = false;

    public Transform shootPoint;
    bool isHurt = false;

    public Slider healthSlider;
    public GameObject[] items;
    public int[] dropRates;

    public int dropAmount = 1;

    //FOR DEBUG
    //public GameObject RespawnPoint ;
    //public GameObject anotherBublu;
    //TILL HERE

    private Transform playerPos;

    Vector2 bulletDirection;

    Animator animator;
    EnemyStats stats;

    void Start()
    {
        stats = GetComponent<EnemyStats>();
        currentHealth = stats.maxHealth;
        healthSlider.maxValue = currentHealth;
        healthSlider.value = currentHealth;
        animator = GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetHurt(float hurtValue)
    {

        currentHealth -= hurtValue;
        healthSlider.value = currentHealth;
        animator.SetTrigger("hurt");
        isHurt = true;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {

        animator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * 1000f);
        DropItems();
        //kill the object after 5s
        Destroy(transform.gameObject, 6f);

        //this.enabled = false;
    }

    public void Shoot()
    {
        animator.SetBool("shoot", true);

        bulletDirection = (playerPos.transform.position - bullet.transform.position).normalized;

        GameObject copy;
        copy = Instantiate(bullet, shootPoint.transform.position, transform.rotation);
        //copy.GetComponent<SpriteRenderer>().enabled = true;
        //copy.GetComponent<Collider2D>().enabled = true;
        copy.SetActive(true);
        copy.GetComponent<Rigidbody2D>().velocity = bulletDirection * shotForce;
        //copy.GetComponent<Rigidbody2D>().AddForce(bulletDirection * shotForce); //add force


    }

    void DropItems()
    {
        int dropCounter;
        int rangeStart;
        int rangeEnd;


        //drop X ammount of times
        for (int k = 0; k < dropAmount; k++)
        {
            //roll a counter
            dropCounter = Random.Range(0, 100);

            rangeStart = 0;
            rangeEnd = 0;


            for (int i = 0; i < dropRates.Length; i++)
            {
                rangeStart = rangeEnd;
                rangeEnd = rangeStart + dropRates[i];
                if (dropCounter >= rangeStart && dropCounter <= rangeEnd)
                {
                    items[i].SetActive(true);                                   //drop that item.
                    items[i].GetComponent<Collider2D>().enabled = true;
                    items[i].GetComponent<Rigidbody2D>().isKinematic = false;
                    items[i].transform.parent = null;
                    break;
                }
            }
        }


    }


}
