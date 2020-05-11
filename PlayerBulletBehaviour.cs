using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    float damage = 10f;
    private void Start()
    {
        damage = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>().damage ; //twice the damage
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Enemy"))
        {
            collidedObject.GetComponent<Enemy_Behaviour>().GetHurt(damage); //this is the player bullet damage

        }
    }
}
