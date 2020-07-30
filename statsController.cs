using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class statsController : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed;
    private float damage;
    public float maxHealth = 100f;
    private float unscaledDltTime;
    private float currentHealth;

    private float highestSpeed;
    private float highestDamage;
    private float highestHealth;

    public float slowDownFactor = 0.025f;
    public float slowDownLength = 4f;
    private float nextNegotiationTime = 0f;
    public float negotiationCooldown = 30f;
    public float negotiateRadius = 10f;
    private bool isNegotiating = false;
    private bool canSlowTime = false;

    float negHealth = 0f;
    float negSpeed = 0f;
    float negDamage = 0f;
    float negMaxHealth = 0f;

    Animator animator;
    Player_Controller pc;

    public GameObject negotiatePanel;
    public GameObject criticalPanel;
    public Transform RespawnPoint;
    public LayerMask enemyLayers;

    public Slider healthSlider;
    public Slider damageSlider;
    public Slider speedSlider;
    public Slider negCooldownSlider;


    public Text distanceText;
    public Text healthText;
    public Text damageText;
    public Text speedText;

    Collider2D[] negotiatingEnemies;


    private void Awake()
    {
        unscaledDltTime = Time.fixedDeltaTime;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        pc = GetComponent<Player_Controller>();



        speed = pc.runSpeed;
        damage = pc.damage;
        currentHealth = maxHealth;


        highestDamage = damage;
        highestHealth = maxHealth;
        highestSpeed = speed;



        negCooldownSlider.maxValue = negotiationCooldown;
        negCooldownSlider.value = negotiationCooldown;

        UpdateUi();

    }

    // Update is called once per frame
    void Update()
    {
        distanceText.GetComponent<Text>().text = "Distance : " + Mathf.Clamp(transform.position.x,0, int.MaxValue).ToString("0");
        negCooldownSlider.value = negotiationCooldown - (nextNegotiationTime - Time.time);

        if (Time.timeScale < 1f)
        {
            Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
            Time.fixedDeltaTime = Time.timeScale * unscaledDltTime;
        }

        if (isNegotiating)
        {
            //if he presses F in that window
            if (Input.GetKeyDown(KeyCode.F))
            {//shoot the bullet
                Pay();
            }

        }

        if (currentHealth / maxHealth < 0.4f && !isNegotiating)
        {
            criticalPanel.SetActive(true);
            canSlowTime = true;
        }
        //else {
        //    criticalPanel.SetActive(false);
        //}

        //we need to negotiate
        if (Input.GetKeyDown(KeyCode.N) && Time.time > nextNegotiationTime && !isNegotiating)
        {
            criticalPanel.SetActive(false);
            nextNegotiationTime = Time.time + negotiationCooldown;
            Negotiate();
            isNegotiating = true;
            //only apply slow, if lower than 40pc Health
            if (canSlowTime)
            {
                Time.timeScale = slowDownFactor;
            }
        }

    }
    void OnCollisionStay2D(Collision2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        float touchDamage;
        if (collidedObject.CompareTag("Enemy"))
        {
            animator.SetTrigger("hurt");
            touchDamage = collidedObject.GetComponent<EnemyStats>().touchDamage;
            currentHealth -= touchDamage;
            healthSlider.value = currentHealth;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            healthText.GetComponent<Text>().text = "Health : " + currentHealth.ToString("0") + " / " + maxHealth.ToString("0");


            if (currentHealth <= 0)
            {
                Die();
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("EnemyBullet"))
        {
            //Debug.Log("WE HIT by" + collidedObject.name);
            animator.SetTrigger("hurt");
            currentHealth -= 2f;
            if (currentHealth <= 0)
            {
                Die();
            }
            healthSlider.value = currentHealth;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            healthText.GetComponent<Text>().text = "Health : " + currentHealth.ToString("0") + " / " + maxHealth.ToString("0");

        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Pickup
        
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Drops"))
        {
            animator.SetTrigger("collect");
            if (collidedObject.name.Equals("speedDrop")) {
                StartCoroutine(HighlightUI(speedSlider));
                speed += highestSpeed / 6f;
            }
            if (collidedObject.name.Equals("healthDrop"))
            {
                StartCoroutine(HighlightUI(healthSlider));
                currentHealth += maxHealth / 4f;
            }
            if (collidedObject.name.Equals("damageDrop"))
            {
               StartCoroutine(HighlightUI(damageSlider));
               damage += highestDamage / 8f;
                if(damage>highestDamage) 
                    highestDamage = damage;

                //update the highest values later
                //highestDamage = damage;
            }
            if (collidedObject.name.Equals("maxHealthDrop"))
            {
                StartCoroutine(HighlightUI(healthSlider));
                maxHealth += highestHealth / 4f;
                currentHealth += highestHealth / 4f;
                if (maxHealth > highestHealth)
                    highestHealth = maxHealth;
            }
            Destroy(collidedObject);
            UpdateUi();
            UpdateController();
        }



    }

    void Negotiate()
    {
        //reset variables, from the last negotiation
        negHealth = 0f;
        negSpeed = 0f;
        negDamage = 0f;
        negMaxHealth = 0f;


        negotiatePanel.SetActive(true);
        Button neg_btn = negotiatePanel.transform.Find("neg_btn").GetComponent<Button>();

        Text neg_speed = negotiatePanel.transform.Find("neg_spd_text").GetComponent<Text>();
        Text neg_health = negotiatePanel.transform.Find("neg_hlt_text").GetComponent<Text>();
        Text neg_damage = negotiatePanel.transform.Find("neg_dmg_text").GetComponent<Text>();
        Text neg_max_health = negotiatePanel.transform.Find("neg_mxHlt_text").GetComponent<Text>();


        negotiatingEnemies = Physics2D.OverlapCircleAll(transform.position, negotiateRadius, enemyLayers);



        if (negotiatingEnemies != null)
        {
            foreach (Collider2D enemy in negotiatingEnemies)
            {
                negHealth += enemy.GetComponent<EnemyStats>().neg_health_percent;
                negMaxHealth += enemy.GetComponent<EnemyStats>().neg_maxHealth_percent;
                negDamage += enemy.GetComponent<EnemyStats>().neg_damage_percent;
                negSpeed += enemy.GetComponent<EnemyStats>().neg_speed_percent;
            }
        }
        //you can lose max 60 pc of current stats and 20pc of max stats
        negHealth = Mathf.Clamp(negHealth, 1, 60f);
        negMaxHealth = Mathf.Clamp(negMaxHealth, 1, 20f);
        negDamage = Mathf.Clamp(negDamage, 1, 60f);
        negSpeed = Mathf.Clamp(negSpeed, 1, 60f);

        neg_speed.text = "Speed : " + negSpeed.ToString("0") + "%";
        neg_health.text = "Health: " + negHealth.ToString("0") + "%";
        neg_max_health.text = "MaxHealth : " + negMaxHealth.ToString("0") + "%";
        neg_damage.text = "Damage : " + negDamage.ToString("0") + "%";

        Debug.Log("Lets negotiate bois");
    }

    void Pay()
    {
        criticalPanel.SetActive(false);
        //lose the percentage

        maxHealth = maxHealth * (1f - negMaxHealth / 100f);
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        currentHealth = currentHealth - ( maxHealth * negHealth / 100f );
        if (currentHealth <= 0)
        {
            Die();
        }

        speed = highestSpeed * (1f - negSpeed / 100f);
        damage = highestDamage * (1f - negDamage / 100f);

        UpdateController();

        if (negotiatingEnemies != null)
        {
            foreach (Collider2D enemy in negotiatingEnemies)
            {
                enemy.GetComponent<Enemy_Behaviour>().Die();
            }
        }


        negotiatePanel.SetActive(false);
        isNegotiating = false;

        UpdateUi();

    }


    void UpdateUi()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        damageSlider.maxValue = highestDamage;
        damageSlider.value = damage;
        speedSlider.maxValue = highestSpeed;
        speedSlider.value = speed;

        speed = Mathf.Clamp(speed, 0, 999f);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        damage = Mathf.Clamp(damage, 1, 999f);

        distanceText.GetComponent<Text>().text = "Distance : " + transform.position.x.ToString("0");
        healthText.GetComponent<Text>().text = "Health : " +  Mathf.CeilToInt(currentHealth).ToString("0") + " / " + Mathf.CeilToInt(maxHealth).ToString("0");
        speedText.GetComponent<Text>().text = "Speed : " + Mathf.CeilToInt(speed).ToString("0");
        damageText.GetComponent<Text>().text = "Damage : " + Mathf.CeilToInt(damage).ToString("0");

    }

    void Die() {
        animator.SetBool("isDead", true);

        //respawn at the start
        transform.position = RespawnPoint.position;
        currentHealth += maxHealth;
        damage = highestDamage;
        speed = highestSpeed;

        animator.SetBool("isDead", false);
    }

    void UpdateController()
    {
        pc.damage = damage;
        pc.runSpeed = speed;

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, negotiateRadius);
    }


    IEnumerator HighlightUI(Slider slider)
    {
        //do the animation x times
        for(int i = 0; i < 2; i++)
        {
            while (slider.transform.localScale.y < 1.70f) {
                slider.transform.localScale = new Vector3(slider.transform.localScale.x, slider.transform.localScale.y + 0.01f, slider.transform.localScale.z);
                yield return null;
            }
            while (slider.transform.localScale.y > 1f)
            {
                slider.transform.localScale = new Vector3(slider.transform.localScale.x, slider.transform.localScale.y - 0.01f, slider.transform.localScale.z);
                yield return null;
            }
        }
    }


}
