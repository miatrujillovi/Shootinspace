using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    [Header("Recarga y tiempo de Disparo")]
    [SerializeField] private float fireRate;
    [SerializeField] private float reloadTime;
    [SerializeField] private float shotCooldown;

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

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI currentAmmoTXT;
    [SerializeField] private TextMeshProUGUI ammoAmountTXT;
    [SerializeField] private GameObject reload;

    private Image reloadFiller;
    private float nextFireTime = 0f;
    private bool isReloading = false;

    private void Start()
    {
        if (currentAmmo <= 0)
            currentAmmo = maxAmmo;

        if (currentMagazines <= 0)
            currentMagazines = maxMagazines;

        reloadFiller = reload.GetComponent<Image>();
    }

    void Update()
    {
        if (isReloading)
            return;

        UpdateUI();

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && currentAmmo > 0)
        {
            Disparar();
            nextFireTime = Time.time + shotCooldown;
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && currentMagazines > 0)
        {
            StartCoroutine(Recargar());
            StartCoroutine(ShowReloadUI());
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

    private IEnumerator ShowReloadUI()
    {
        reloadFiller.fillAmount = 0f;
        reload.SetActive(true);

        float timer = 0f;

        while (timer < reloadTime)
        {
            timer += Time.deltaTime;
            reloadFiller.fillAmount = timer / reloadTime;
            yield return null;
        }

        reloadFiller.fillAmount = 1f;
        reload.SetActive(false);
    }

    private void UpdateUI()
    {
        currentAmmoTXT.text = currentAmmo.ToString();
        ammoAmountTXT.text = "/" + maxAmmo.ToString();
    }
}