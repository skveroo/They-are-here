using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI;

public class HealthBar: MonoBehaviour 
{
    public Slider healthBar;
    public Health playerHealth;

    private void Start(){
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = playerHealth.health;
        healthBar.value = playerHealth.health;
    }
    public void SetHealth(float hp){
        healthBar.value = hp;
    }
}