﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bublu_walk_Behaviour : StateMachineBehaviour
{

    private Transform playerPos;
    public float followSpeed = 1f;
    public float searchRadius = 7f;
    public float shootRate = 0.5f;
    private float nextShootTime = 0f;    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    Enemy_Behaviour eb;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        eb = animator.GetComponent<Enemy_Behaviour>();
        nextShootTime = Time.time + (1f / shootRate);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(playerPos.position, animator.transform.position) >= searchRadius)
        {
            animator.SetBool("isWalking", false);
        }

        if (Time.time > nextShootTime)
        {
            eb.Shoot();
            nextShootTime = Time.time + (1f / shootRate);
        }

        animator.transform.position = Vector2.MoveTowards(animator.transform.position, playerPos.position, followSpeed * Time.deltaTime);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
