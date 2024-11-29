using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
public class WeaponSwitch : MonoBehaviour
{
    private InputMenager controls;
    [SerializeField] private bool isAbleToPickUp;
    [SerializeField] private GameObject currentWeapon;
    [SerializeField] private GameObject pickupWeapon;


    private void Awake()
    {
        controls = new InputMenager();
        weaponDetection();
        controls.Player.Take.performed += ctx => takeWeapon();
    }
    void weaponDetection()
    {
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
        foreach (GameObject obj in weapons)
        {
            if (obj.activeSelf)
            {
                currentWeapon = obj;
            }
        }
    }
    void takeWeapon()
    {
        if (isAbleToPickUp)
        {
        
            GameObject[] allWeaponModels = GameObject.FindGameObjectsWithTag("weaponSpotGun");
            currentWeapon.SetActive(false);
            pickupWeapon.SetActive(true);
            pickupWeapon = currentWeapon;
            weaponDetection();
            foreach (GameObject weapon in allWeaponModels)
            {
                MeshRenderer weaponMeshRenderer = weapon.GetComponent<MeshRenderer>();
                if (weapon.name == pickupWeapon.name)
                {
                    weaponMeshRenderer.enabled = true;
                }
                else
                {
                    weaponMeshRenderer.enabled = false;
                }
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        if (other.gameObject.tag == "Player")
        {
            isAbleToPickUp = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isAbleToPickUp = false;
        }
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
