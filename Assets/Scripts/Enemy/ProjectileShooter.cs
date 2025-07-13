using UnityEditor.EditorTools;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [Header("Prefab y salida")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Stats")]
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float fireCooldown = 0.75f;
    [SerializeField] private float spreadAngle = 1.5f;

    [Header("FX (opcionales)")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioClip shotSFX;
    [SerializeField] private AudioSource audioSrc;

    float _nextFireTime = 0f;

    public bool Shoot(Vector3 targetPos)
    {
        if (Time.time < _nextFireTime) return false;
        _nextFireTime = Time.time + fireCooldown;

        Vector3 dir = (targetPos - firePoint.position).normalized;
        dir = Quaternion.Euler(
                Random.Range(-spreadAngle, spreadAngle),
                Random.Range(-spreadAngle, spreadAngle),
                0f) * dir;

        Projectile proj = PoolManager.Instance
            ? PoolManager.Instance.Get(projectilePrefab, firePoint.position, Quaternion.LookRotation(dir))
            : Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(dir));

        proj.Init(dir * projectileSpeed, damage, gameObject);

        muzzleFlash?.Play();
        if (shotSFX) audioSrc?.PlayOneShot(shotSFX);

        return true;
    }
}