using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    float damage = 10f;
    ParticleSystem particles;
    public GameObject particlePrefab;
    private void Start()
    {
        damage = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>().damage ; //twice the damage
        particles = particlePrefab.GetComponent<ParticleSystem>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Enemy") || collidedObject.CompareTag("Boss"))
        {
            collidedObject.GetComponent<Enemy_Behaviour>().GetHurt(damage); //this is the player bullet damage
            //also destory this.

            particles.Play();
            particlePrefab.transform.parent = null;
            Destroy(particlePrefab, 1f);
            Destroy(this.gameObject);
        }
    }
}
