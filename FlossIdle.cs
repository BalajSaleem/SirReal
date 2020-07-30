using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlossIdle : StateMachineBehaviour
{

    private Transform playerPos;
    public float searchRadius = 4f;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(playerPos.position, animator.transform.position) <= searchRadius)
        {
            animator.GetComponent<FlyingBoss>().enabled = true; //enable the flying script
            animator.SetBool("fly", true);
        }

    }

   // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
