using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradingBehaviour : MonoBehaviour
{
    public Button damageUpgradeButton;
    public Button speedUpgradeButton;
    public Button healthUpgradeButton;

    public PlayerController playerMS;
    public Health playerHealth;
    public List<Weapon> weapons;

    public float damageIncreaseAmount = 2f;
    public float msIncreaseAmount = 2f;
    public float healthIncreaseAmount = 2f;
    public GameObject LevelUpgradeUI;
    private void Start()
    {
        // Przypisywanie metod do przycisków
        damageUpgradeButton.onClick.AddListener(UpgradeDamage);
        speedUpgradeButton.onClick.AddListener(UpgradeSpeed);
        healthUpgradeButton.onClick.AddListener(UpgradeHealth);
    }

    public void UpgradeDamage()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.damageAmount *= damageIncreaseAmount;
        }
    }

    public void UpgradeSpeed()
    {
        playerMS.baseSpeed *= msIncreaseAmount;
    }

    public void UpgradeHealth()
    {
        playerHealth.maxHealth *= healthIncreaseAmount;
        playerHealth.health = playerHealth.maxHealth;
        
    }
    public void CloseScreen()
    {
        LevelUpgradeUI.SetActive(false);
        Time.timeScale = 1f;
    }
}