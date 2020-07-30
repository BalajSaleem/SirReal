using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    // Start is called before the first frame update

    bool flipped = false;
    public float runSpeed = 10f;
    public float damage = 10f;
    public float attackSpeed = 0.5f;
    public float attackRadius = 2f;

    private float nextAttackTime = 0f;
    public float shotForce = 100f;
    public float jumpForce = 200f;

    private float initialScaleX = 1.2f;

    Vector2 bulletDirection;

    Rigidbody2D rb;
    Animator animator;
    Collider2D collider;
   //statsController stat;

    public GameObject bullet;
    public GameObject sword;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public LayerMask groundLayer;


    ParticleSystem bloodParticles;

    public GameObject particlePrefab;

    public GameObject projectileBase;
    public GameObject[] shownProjectiles; 


    public int totalProjectiles = 4;
    public int currentProjectiles;
    public int projectileCooldown = 3;
    private float projectileRespawnTime = 0f;





    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        initialScaleX = transform.localScale.x;
        bloodParticles = particlePrefab.GetComponent<ParticleSystem>();

        currentProjectiles = totalProjectiles;
        //shownProjectiles = new GameObject[totalProjectiles];

        //Setup projectiles
        SetupProjectiles(); 
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")));


        if (Input.GetKey(KeyCode.D))
        {
            if (transform.localScale.x != 1 * initialScaleX)
            {
                Flip();
            }
            transform.position = new Vector2(transform.position.x + Vector2.right.x * (runSpeed * Time.deltaTime), transform.position.y);
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (transform.localScale.x != -1 * initialScaleX)
            {
                Flip();
            }
            transform.position = new Vector2(transform.position.x + Vector2.left.x * (runSpeed * Time.deltaTime), transform.position.y);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Attack();

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(IsGrounded())
                Jump();

        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            HvyAttack();

        }

        if (Input.GetKeyDown(KeyCode.L))
        {//shoot the bullet

            if (currentProjectiles > 0) {
                Shoot();
                currentProjectiles--;
                SetupProjectiles();
                projectileRespawnTime = Time.time + projectileCooldown;
            }  
        }

        if (currentProjectiles < totalProjectiles && Time.time > projectileRespawnTime)
        {
            currentProjectiles++;
            SetupProjectiles();
            projectileRespawnTime = Time.time + projectileCooldown;
        }

    }
    void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        bullet.transform.localScale = new Vector3(bullet.transform.localScale.x * -1, bullet.transform.localScale.y, bullet.transform.localScale.z);

        //transform.Rotate(0, 180, 0);

        flipped = !flipped;
    }

    void Attack(){
        if (Time.time >= nextAttackTime) {
            animator.SetTrigger("attack");
            nextAttackTime = Time.time + (1 / attackSpeed);
            CommonAttack(damage);
        }
        
    }
    void HvyAttack()
    {

        if (Time.time >= nextAttackTime)
        {
            animator.SetTrigger("hvyAttack");
            nextAttackTime = Time.time + (1 / attackSpeed);
            CommonAttack(damage * 1.5f);
        }
    }

    void Jump()
    {
        animator.SetTrigger("jump");
        rb.AddForce(Vector2.up * jumpForce);

    }

    void Shoot() {
        GameObject copy;
        if (flipped)
            bulletDirection = Vector2.left;
        else
            bulletDirection = Vector2.right;
        copy = Instantiate(bullet, transform.position, transform.rotation);
        //copy.transform.localScale = new Vector3(transform.localScale.x * -1, bullet.transform.localScale.y, bullet.transform.localScale.z);
        //copy.GetComponent<SpriteRenderer>().enabled = true;
        //copy.GetComponent<Collider2D>().enabled = true;
        copy.SetActive(true);
        copy.GetComponent<Rigidbody2D>().AddForce(bulletDirection * shotForce); //add force
        

    }

    void CommonAttack(float hurtValue ) {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayers);

        if (hitEnemies != null) {
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy_Behaviour>().GetHurt(hurtValue); //add various damages here
                Debug.Log("We attacked:" + enemy.name);
                bloodParticles.Play();
            }
        }


        
    }


    private void SetupProjectiles()
    {

        for (int i = 0; i < currentProjectiles; i++) {
            shownProjectiles[i].SetActive(true);
        }
        for (int j = currentProjectiles; j < totalProjectiles; j++) {
            shownProjectiles[j].SetActive(false);
        }

    }


    private bool IsGrounded() {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .1f, groundLayer);
        return raycastHit2D.collider != null;
    
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
