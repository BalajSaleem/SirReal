using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlossFly : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    private Transform playerPos;
    public float shootRate = 0.5f;
    private float nextShootTime = 0f;

    public float flightHeight = 15f;
    public float flighSpeed = 7f;
    public float ascendingSpeed = 20f;


    Rigidbody2D rb;
    Enemy_Behaviour eb;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flightHeight = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - 4f; //highest possible height
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        eb = animator.GetComponent<Enemy_Behaviour>();

        //nextTiredTime = Time.time + getTiredAfter;
        nextShootTime = Time.time + (1f / shootRate);
        rb.gravityScale = 0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.transform.position.y <= flightHeight) {

            animator.transform.position = new Vector2(animator.transform.position.x, animator.transform.position.y + ascendingSpeed * Time.deltaTime);
            
        }
        else
        {
            animator.transform.position = new Vector2( Vector2.MoveTowards(animator.transform.position, playerPos.transform.position, flighSpeed * Time.deltaTime).x , animator.transform.position.y);
        }

        if (Time.time > nextShootTime)
        {
            eb.Shoot();
            nextShootTime = Time.time + (1f / shootRate);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        

    }

}
