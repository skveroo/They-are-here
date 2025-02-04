using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    Animator animator;
    private GameObject target;
    private Vector3 lastPosition;
    private float currentAnimationTime;
    void Start()
    {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");

        if (target == null)
        {
            Debug.LogError("No object with the tag 'Player' found.");
        }

        lastPosition = transform.position;
    }
    public void SaveAnimationState()
    {
        currentAnimationTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    public void RestoreAnimationState()
    {
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, currentAnimationTime);
    }

    void Update()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);


        bool isMoving = (transform.position != lastPosition);


        if (distanceToTarget <= 4.1f)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isSwiping", true);
        }
        else
        {

            animator.SetBool("isSwiping", false);
        }

        if (isMoving)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        lastPosition = transform.position;
    }
}
