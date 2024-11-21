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
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] string EnemyTag;
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
        if (PauseMenu.GameIsPaused) return;
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

    private void PerformShot()
    {
        readyToShoot = false;
        float x = Random.Range(-horizontalSpread, horizontalSpread);
        float y = Random.Range(-verticalSpread, verticalSpread);
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Vector3 direction = ray.direction + new Vector3(x, y, 0); 

        if (Physics.Raycast(ray.origin, direction, out rayHit, bulletRange))
        {
            Debug.Log("Hit: " + rayHit.collider.name);

            if (rayHit.collider.CompareTag(EnemyTag))
            {
                Health enemyHealth = rayHit.collider.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    float damageAmount = 25f;
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
