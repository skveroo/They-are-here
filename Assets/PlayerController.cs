using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector2 move, mouseLook;
    private Vector3 rotationTarget;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseLook);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            rotationTarget = hit.point;
        }
    }

    void FixedUpdate()
    {
        movePlayerWithAim();
    }

    public void movePlayerWithAim()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y).normalized * speed * Time.fixedDeltaTime;
        Vector3 lookPos = rotationTarget - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);

        if (movement != Vector3.zero)
        {
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, rotation, 0.15f));
            rb.MovePosition(rb.position + movement); // Move the player with Rigidbody
        }
    }

}
