using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damageAmount = 10f;
    public string targetTag = "Player";
    public float damageInterval = 1f;
    private bool playerInArea = false;
    private float damageTimer = 0f;


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


    private void Update()
    {
        if (playerInArea)
        {

            damageTimer += Time.deltaTime;


            if (damageTimer >= damageInterval)
            {

                ApplyDamage();


                damageTimer = 0f;
            }
        }
    }


    private void ApplyDamage()
    {

        GameObject player = GameObject.FindGameObjectWithTag(targetTag);
        if (player != null)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("Player took " + damageAmount + " damage.");
            }
        }
    }
}
