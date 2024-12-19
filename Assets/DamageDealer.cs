using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damageAmount = 10f;
    public string targetTag = "Player";
    public float damageInterval = 1f;
    public float proximityRange = 1.5f;
    private bool playerInArea = false;
    private float damageTimer = 0f;
    public float damageTaken = 0f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {

            ApplyDamage();


            playerInArea = true;
            damageTimer = 0f;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {

            playerInArea = false;
            damageTimer = 0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            ApplyDamage();
        }
    }

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag(targetTag);

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= proximityRange)
            {
                damageTimer += Time.deltaTime;

                if (damageTimer >= damageInterval)
                {
                    ApplyDamage();
                    damageTimer = 0f;
                }
            }
            else if (playerInArea)
            {
                damageTimer += Time.deltaTime;

                if (damageTimer >= damageInterval)
                {
                    ApplyDamage();
                    damageTimer = 0f;
                }
            }
            else
            {
                damageTimer = 0f;
            }
        }
    }


    private void ApplyDamage()
    {

        GameObject player = GameObject.FindGameObjectWithTag(targetTag);
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
            Debug.Log("Player took " + damageAmount + " damage.");
            StatsManager.Instance.AddDamageTaken(damageAmount);
        }
    }
}