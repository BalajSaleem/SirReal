using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnTrigger : MonoBehaviour
{
    public string tagToPassDieOn;
    ParticleSystem particles;
    public GameObject particlePrefab;

    private void Start()
    {
        particles = particlePrefab.GetComponent<ParticleSystem>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Boss")) {

            particles.Play();
            particlePrefab.transform.parent = null;
            Destroy(particlePrefab, 1f);

            Destroy(gameObject);
        }

        
    }
}
