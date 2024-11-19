using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    private InputMenager controls;
    private RaycastHit rayHit;
    private int ammoLeft;
    private int bulletsShot;
    private bool isShooting, readyToShoot, isReloading;

    [SerializeField] private int bulletsPerBurst;
    [SerializeField] private float bulletRange;
    [SerializeField] private float fireRate, reloadTime;
    [SerializeField] private float bulletHoleLifeSpan;
    [SerializeField] private float horizontalSpread, verticalSpread, burstDelay;
    [SerializeField] private bool isAutomatic;
    [SerializeField] private int magSize;
    [SerializeField] private GameObject bulletHolePrefab;
    [SerializeField] private GameObject currentWeapon;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] string EnemyTag;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private GameObject tracerPrefab;
    [SerializeField] private float bulletSpeed = 20f; 
    private void Awake()
    {
        controls = new InputMenager();
        ammoLeft = magSize;
        readyToShoot = true;
        controls.Player.Shoot.started += ctx => StartShot();
        controls.Player.Shoot.canceled += ctx => EndShot();
        controls.Player.Reload.performed += ctx => Reload();
    }

    private void Update()
    {
        AlignBulletOriginToAim();
        if (isShooting && readyToShoot && !isReloading && ammoLeft > 0)
        {
            bulletsShot = bulletsPerBurst;
            PerformShot();
        }
    }

    private void StartShot()
    {
        isShooting = true;
    }

    private void EndShot()
    {
        isShooting = false;
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
    private void AlignBulletOriginToAim()
    {
        weaponDetection();
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, bulletRange))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * bulletRange;
        }
        Vector3 directionToTarget = (targetPoint - bulletOrigin.position).normalized;
        bulletOrigin.rotation = Quaternion.LookRotation(directionToTarget);
        if (currentWeapon != null)
        {
            currentWeapon.transform.rotation = Quaternion.LookRotation(directionToTarget);
        }
    }
    private void PerformShot()
    {

        readyToShoot = false;
        float x = Random.Range(-horizontalSpread, horizontalSpread);
        float y = Random.Range(-verticalSpread, verticalSpread);
        Vector3 direction = bulletOrigin.forward + new Vector3(x, y, 0);
        GameObject bullet = Instantiate(tracerPrefab, bulletOrigin.position, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = direction.normalized * bulletSpeed;
        }
        Destroy(bullet, 5f);
        if (Physics.Raycast(bulletOrigin.position, direction, out rayHit, bulletRange))
        {
            Destroy(bullet, 2f);
            if (rayHit.collider.CompareTag(EnemyTag) || rayHit.collider.CompareTag("mainObjective"))
            {

                Health enemyHealth = rayHit.collider.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    weaponDetection();
                    float damageAmount;
                    switch (currentWeapon.name)
                    {
                        case "AutomaticRifle":
                            damageAmount = 25f;
                            break;

                        case "Pistol":
                            damageAmount = 15f;
                            break;

                        default:
                            Debug.Log("Unknown weapon.");
                            damageAmount = 0f;
                            break;
                    }
                    enemyHealth.TakeDamage(damageAmount);
                }
            }
            else
            {
                GameObject bulletHole = Instantiate(bulletHolePrefab, rayHit.point + rayHit.normal * 0.001f, Quaternion.identity);
                bulletHole.transform.LookAt(rayHit.point + rayHit.normal);
                Destroy(bulletHole, bulletHoleLifeSpan);
            }
        }
        muzzleFlash.Play();
        ammoLeft--;
        bulletsShot--;
        if (bulletsShot > 0 && ammoLeft > 0)
        {
            Invoke("ResumeBurst", burstDelay);
        }
        else
        {
            Invoke("ResetShot", fireRate);
            if (!isAutomatic)
            {
                EndShot();
            }
        }
        if (ammoLeft == 0)
        {
            Reload();
        }
    }



    private void ResumeBurst()
    {
        readyToShoot = true;
        PerformShot();
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        Debug.Log("Reloading");
        isReloading = true;
        Invoke("ReloadFinish", reloadTime);
    }

    private void ReloadFinish()
    {
        ammoLeft = magSize;
        isReloading = false;
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

