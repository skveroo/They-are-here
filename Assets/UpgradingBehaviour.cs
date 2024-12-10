using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UpgradingBehaviour : MonoBehaviour
{
public Button UpgradeButton;

public PlayerController playerMS;
public Health playerHealth;
public List<Weapon> weapons;
public float damageIncreaseAmount = 2f;
public float msIncreaseAmount = 2f;
public void Upgrade()
{
    switch (UpgradeButton.name)
    {
        case "DMG":
                foreach (Weapon weapon in weapons)
            {
                weapon.damageAmount *= damageIncreaseAmount;
            }
            break;
        case "MS":
            playerMS.baseSpeed = playerMS.baseSpeed*msIncreaseAmount;
            break;
        case "HP":
            playerHealth.maxHealth = playerHealth.maxHealth*2f;  
            break; 
    }
}
}
