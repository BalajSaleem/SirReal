using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update

    public bool getsStronger = false;
    //public float totalMapLength = 250f;
    public float multiplier = 1f;
    public float maxHealth = 100f;
    public float touchDamage = 0.5f;
    public float bulletDamage = 10f;
    public float meleeDamage = 20f;

    public float neg_health_percent = 20f;
    public float neg_maxHealth_percent = 10f;
    public float neg_speed_percent = 20f;
    public float neg_damage_percent = 20f;

    private void Awake()
    {
        if (getsStronger)
        {

            multiplier = multiplier + (transform.position.x / GameObject.FindGameObjectWithTag("Boss").transform.position.x ) * 1f;
            maxHealth = maxHealth * multiplier;
            touchDamage = touchDamage * multiplier;


        }
    }


}
