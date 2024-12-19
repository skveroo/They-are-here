using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    float velocity = 0.0f;
    public float acceleration = 0.1f;
    int VelocityHash;
    public float deceleration = 0.5f;
    void Start()
    {
        animator = GetComponent<Animator>();
        VelocityHash = Animator.StringToHash("Velocity");
    }

    void Update()
    {
        bool isShooting = animator.GetBool("isFiring");
        bool fires = Input.GetKey("mouse 0");
        bool isRunning = animator.GetBool("isRunning");
        bool movePressed = Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d");
        bool stopped = !Input.GetKey("w") && !Input.GetKey("s") && !Input.GetKey("a") && !Input.GetKey("d");
        bool isWalking = animator.GetBool("isWalking");
        bool runPressed = Input.GetKey("left shift");
        if (!isWalking && !isRunning && fires)
        {
            animator.SetBool("isFiring", true);
        }
        if(!fires || isWalking || isRunning)
        {
            animator.SetBool("isFiring", false);
        }
        if (movePressed && velocity < 1.0f)
        {
            velocity += Time.deltaTime*acceleration;
        }
        if (!movePressed && velocity > 0.0f)
        {
            velocity -= Time.deltaTime * deceleration;
        }
        if (!movePressed && velocity<0.0f)
        {
            velocity = 0.0f; 
        }
        animator.SetFloat(VelocityHash, velocity);
        if (!isWalking && movePressed)
        {
            animator.SetBool("isWalking", true);
        }
        if (isWalking && stopped)
        {
            animator.SetBool("isWalking", false);
        }

        if(!isRunning && (movePressed && runPressed))
        {
            animator.SetBool("isRunning", true);
        }

        if (isRunning && (!movePressed || !runPressed))
        {
            animator.SetBool("isRunning", false);
        }
    }
}
