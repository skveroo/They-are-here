using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 move;
    private Vector2 mouseLook;
    private Vector3 rotationTarget;
    private Rigidbody rb;
    public float drag = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = drag;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        UpdateRotationToCursor();
        MovePlayer();
    }

    private void UpdateRotationToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseLook);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        RaycastHit closestHit = default;
        float closestDistance = Mathf.Infinity;
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Transparent"))
            {
                continue;
            }
            if (hit.distance < closestDistance)
            {
                closestHit = hit;
                closestDistance = hit.distance;
            }
        }

        if (closestHit.collider != null)
        {
            rotationTarget = closestHit.point;
            Vector3 lookPos = rotationTarget - transform.position;
            lookPos.y = 0;
            if (lookPos != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookPos);
                targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.2f);
            }
        }
    }

    private void MovePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y).normalized;
        rb.angularVelocity = Vector3.zero;

        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);
    }
}
