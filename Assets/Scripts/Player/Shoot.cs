using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Shoot : MonoBehaviour
{
    [Header("Recarga y tiempo de Disparo")]
    [SerializeField] private float fireRate;
    [SerializeField] private float reloadTime;

    [Header("Municion y Cargadores")]
    [SerializeField] private int maxAmmo;
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxMagazines;
    [SerializeField] private int currentMagazines;

    [Header("Bala")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private AudioSource bulletAudio;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float lifetime;
    [SerializeField] private bool isExplosive;

    private float nextFireTime = 0f;
    private bool isReloading = false;

    private void Start()
    {
        if (currentAmmo <= 0)
            currentAmmo = maxAmmo;

        if (currentMagazines <= 0)
            currentMagazines = maxMagazines;
    }

    void Update()
    {
        if (isReloading)
            return;

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && currentAmmo > 0)
        {
            Disparar();
            nextFireTime = Time.time + 1f / fireRate;
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && currentMagazines > 0)
        {
            StartCoroutine(Recargar());
        }
    }

    private void Disparar()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = bulletSpawnPoint.forward * bulletSpeed;
        }
        Destroy(bullet, lifetime);
        currentAmmo--;
    }

    private IEnumerator Recargar()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = maxAmmo - currentAmmo;
        if (currentMagazines > 0 && ammoNeeded > 0)
        {
            if (ammoNeeded <= currentMagazines * maxAmmo)
            {
                currentMagazines--;
                currentAmmo = maxAmmo;
            }
            else
            {
                currentAmmo += currentMagazines * maxAmmo;
                currentMagazines = 0;
            }
        }
        isReloading = false;
    }
}