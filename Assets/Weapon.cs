using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using static System.Math;
public class Weapon : MonoBehaviour
{
    private InputMenager controls;
    private RaycastHit rayHit;
    public int ammoLeft;
    private int bulletsShot;
    private bool isShooting, readyToShoot, isReloading;

    public GameObject player;
    public TMP_Text ammoInfo;
    [SerializeField] private int bulletsPerBurst;
    [SerializeField] private float bulletRange;
    [SerializeField] private float fireRate, reloadTime;
    [SerializeField] private float bulletHoleLifeSpan;
    [SerializeField] private float horizontalSpread, verticalSpread, burstDelay;
    [SerializeField] private bool isAutomatic;
    [SerializeField] private int magSize;
    [SerializeField] private GameObject bulletHolePrefab;
    [SerializeField] public GameObject currentWeapon;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] string EnemyTag;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private GameObject tracerPrefab;
    [SerializeField] private float bulletSpeed = 20f;
    public float damageAmount;
    public PlayerExperience playerExp;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerExp = player.GetComponent<PlayerExperience>();
        controls = new InputMenager();
        ammoLeft = magSize;
        readyToShoot = true;
        controls.Player.Shoot.started += ctx => StartShot();
        controls.Player.Shoot.canceled += ctx => EndShot();
        controls.Player.Reload.performed += ctx => Reload();
    }

    private void Update()
    {
        if (PauseMenu.GameIsPaused == true) return;
        ammoInfo.text = "Ammo: " + ammoLeft + "/" + magSize;
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
        RaycastHit[] hits = Physics.RaycastAll(ray, bulletRange);

        RaycastHit closestHit = default;
        float closestDistance = Mathf.Infinity;
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Transparent")|| hit.collider.CompareTag("Room"))
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
            targetPoint = closestHit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * bulletRange;
        }
        Vector3 directionToTarget = (targetPoint - bulletOrigin.position).normalized;
        bulletOrigin.rotation = Quaternion.LookRotation(directionToTarget);
        if (currentWeapon != null)
        {
            currentWeapon.transform.rotation = Quaternion.Slerp(currentWeapon.transform.rotation, Quaternion.LookRotation(directionToTarget, Vector3.up), Time.deltaTime * 10f);
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
        RaycastHit[] hits = Physics.RaycastAll(bulletOrigin.position, direction, bulletRange);
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
            Destroy(bullet, 2f);
            if (closestHit.collider.CompareTag(EnemyTag) || closestHit.collider.CompareTag("mainObjective"))
            {
                Health enemyHealth = closestHit.collider.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    weaponDetection();
                    
                    switch (currentWeapon.name)
                    {
                        case "AutomaticRifle":
                            damageAmount = (float)Pow(1.5f,(playerExp.currentLevel-1))*25f;
                            break;

                        case "Pistol":
                            damageAmount = (float)Pow(1.5f,(playerExp.currentLevel-1))*15f;
                            break;

                        default:
                            damageAmount = 0f;
                            break;
                    }
                    enemyHealth.TakeDamage(damageAmount);
                }
            }
            else
            {
                GameObject bulletHole = Instantiate(bulletHolePrefab, closestHit.point + closestHit.normal * 0.001f, Quaternion.identity);
                bulletHole.transform.LookAt(closestHit.point + closestHit.normal);
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

