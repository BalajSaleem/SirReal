using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    // Start is called before the first frame update


    public float respawnTime = 5f;
    float nextSpawnTime = 10f;
    public GameObject bublu;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextSpawnTime)
        {
            Instantiate(bublu, transform.position, transform.rotation);
            nextSpawnTime += respawnTime;
        }
    }
}
