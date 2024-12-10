using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // Dodaj to, aby używać UI
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 move;
    private Vector2 mouseLook;
    private Vector3 rotationTarget;
    private Rigidbody rb;
    public float drag = 5f;

    private bool isSprinting = false;
    public float sprintSpeedMultiplier = 2f;
    public float baseSpeed;

    // Mechanika staminy
    public float maxStamina = 100f;
    public float stamina = 100f;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 10f;
    public float minSprintStamina = 10f;

    // UI Elements
    public Slider staminaSlider; // Referencja do slidera
    public TMP_Text staminaText;     // Referencja do tekstu

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = drag;
        baseSpeed = speed;

        // Inicjalizacja UI
        UpdateStaminaUI();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started && stamina > minSprintStamina)
        {
            isSprinting = true;
        }
        else if (context.canceled)
        {
            isSprinting = false;
        }
    }

    private void Update()
    {
        if (PauseMenu.GameIsPaused) return;

        UpdateRotationToCursor();
        MovePlayer();
        HandleStamina(); // Obsługa staminy
        UpdateStaminaUI(); // Aktualizacja UI
    }

    private void UpdateRotationToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseLook);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        RaycastHit closestHit = default;
        float closestDistance = Mathf.Infinity;
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Transparent") || hit.collider.CompareTag("Room"))
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

        if (isSprinting && stamina > minSprintStamina)
        {
            speed = baseSpeed * sprintSpeedMultiplier;
        }
        else
        {
            speed = baseSpeed;
            isSprinting = false;
        }

        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);
    }

    private void HandleStamina()
    {
        if (isSprinting)
        {
            stamina -= staminaDrainRate * Time.deltaTime;
            if (stamina <= 0)
            {
                stamina = 0;
                isSprinting = false;
            }
        }
        else
        {
            if (stamina < maxStamina)
            {
                stamina += staminaRegenRate * Time.deltaTime;
                if (stamina > maxStamina)
                {
                    stamina = maxStamina;
                }
            }
        }
    }

    private void UpdateStaminaUI()
    {
        // Aktualizacja wartości slidera
        if (staminaSlider != null)
        {
            staminaSlider.value = stamina;
        }

        // Aktualizacja tekstu staminy
        if (staminaText != null)
        {
            staminaText.text = "Stamina: " + $"{Mathf.RoundToInt(stamina)} / {Mathf.RoundToInt(maxStamina)}";
        }
    }
}
