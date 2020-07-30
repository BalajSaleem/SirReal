using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyScript : MonoBehaviour
{

    Random rand;

    [SerializeField]
    GameObject bullet;


    [SerializeField]
    GameObject FrisbyBullet;

    private Transform Player;
    //public Transform AttackPos;


    public float movementSpeed;
    public bool isBoss; 
    public int enemyLevel;
    public bool frisby;
    public bool dagger;

    //public Animator Enemy;
    private Rigidbody2D rb;


    private Vector2 movement;

    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public float attackRange;

    float fireRate;
    float nextFire;
    float health;
    float followRadius;


    public EnemyScript(int enemyLevel, bool isBoss, bool frisby, bool dagger, float movementSpeed)
    {
        this.enemyLevel = enemyLevel;
        this.isBoss = isBoss;

        if (isBoss)
            enemyLevel = 3;

        this.frisby = frisby;
        this.dagger = dagger;
        if (movementSpeed == null)
            this.movementSpeed = 1;

        this.movementSpeed = movementSpeed;


    }


    // Start is called before the first frame update
    void Start()
    {


        if (enemyLevel == 0)
        {
            fireRate = 3f;
            health = 50;
            followRadius = 40;
        }
        else if(enemyLevel == 1)
        {
            fireRate = 2f;
            health = 75;
            followRadius = 50;
        }
        else if(enemyLevel == 2)
        {
            fireRate = 1f;
            health = 100;
            followRadius = 60;
        }

        Player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        nextFire = Time.time;
    }

    // Update is called once per frame
    void Update()
    {


        //attack();
        checkFire();
        move();
    }
    //void attack()
    //{
    //    if(timeBtwAttack <= 0)
    //    {
    //        if(Mathf.Abs(Player.position.x - transform.position.x) < 7)
    //        {
    //            //Enemy.SetTrigger("EnemyMelee");
    //            Collider2D[] damagePlayer = Physics2D.OverlapCircleAll(AttackPos.position, attackRange);
    //            //Debug.Log("attacked");
    //        }
    //        timeBtwAttack = startTimeBtwAttack;
    //    }
    //    else
    //    {
    //        timeBtwAttack -= Time.deltaTime;
    //    }
    //}

    void move()
    {
        Vector3 direction = Player.position - transform.position;
        //float angle = Mathf.Atan2(direction.x, 0) * Mathf.Rad2Deg;
        //rb.rotation = angle;
        direction.Normalize();
        direction.y = 0;
        movement = direction;
    }


    private void FixedUpdate()
    {
        if( Mathf.Abs(Player.position.x - transform.position.x)  < followRadius)
            moveCharacter(movement);
    }
    void moveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * movementSpeed * Time.deltaTime));
    }


    void checkFire()
    {
        if(Time.time > nextFire)
        {
            if (dagger && bullet != null)
            {
                Instantiate(bullet, transform.position, Quaternion.identity);
            }
            if (frisby && FrisbyBullet != null)
            {
               Instantiate(FrisbyBullet, transform.position, Quaternion.identity);
            }
            nextFire = Time.time + fireRate;
        }
    }


    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.gameObject.name.Equals("Player"))
    //    {

    //        GameObject player = GameObject.Find("Player");
    //        PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
    //        int damage = playerScript.damage;

    //        health = health - damage;


    //        Debug.Log(health);
    //    }

    //    Debug.Log("safa");
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(AttackPos.position, attackRange);
    //}

    public void takeDamage(int damage)
    {
        health = health - damage;
    }
}
