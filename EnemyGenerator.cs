using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    // Start is called before the first frame update


    public float respawnTime = 5f;
    float nextSpawnTime = 10f;
    float highestDist = 0f;
    public GameObject[] enemies;
    private GameObject enemyToSpawn;
    void Start()
    {
        highestDist = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Time.time > nextSpawnTime && transform.position.x > highestDist)
        {
            enemyToSpawn = enemies[ (int) Random.Range(0, enemies.Length) ];
            highestDist = transform.position.x;
            Instantiate(enemyToSpawn, transform.position, transform.rotation);
            nextSpawnTime += Time.time + respawnTime;
        }
    }
}
