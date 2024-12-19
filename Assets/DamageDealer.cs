using System.Collections;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damageAmount = 10f;
    public string targetTag = "Player";
    public float damageInterval = 1f;
    public float proximityRange = 1.5f;
    private bool playerInArea = false;
    private Coroutine damageCoroutine;
	public float damageTaken = 0f;
	
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            playerInArea = true;
            ApplyDamageOnce(other);
            StartDamageCoroutine();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            playerInArea = false;
            StopDamageCoroutine();
        }
    }

    private void ApplyDamageOnce(Collider target)
    {
        Health playerHealth = target.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
            Debug.Log("Player took " + damageAmount + " damage.");
			StatsManager.Instance.AddDamageTaken(damageAmount);
        }
    }

    private void StartDamageCoroutine()
    {
        if (damageCoroutine == null)
        {
            damageCoroutine = StartCoroutine(DamageOverTime());
        }
    }

    private void StopDamageCoroutine()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator DamageOverTime()
    {
        while (playerInArea)
        {
            yield return new WaitForSeconds(damageInterval);
            Collider[] hits = Physics.OverlapSphere(transform.position, proximityRange);

            foreach (var hit in hits)
            {
                if (hit.CompareTag(targetTag))
                {
                    ApplyDamageOnce(hit);
                }
            }
        }
    }
}
