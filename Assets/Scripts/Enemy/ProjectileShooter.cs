using UnityEditor.EditorTools;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [Header("Prefab y salida")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Stats")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float fireCooldown;
    [SerializeField] private float spreadAngle;

    [Header("Sounds")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioSource audioSrc;
    
    [Header("Owner")]
    [SerializeField] private GameObject gameObjectOwner;

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


        proj.Init(dir * projectileSpeed, damage, gameObjectOwner);

        if (shootSound) audioSrc?.PlayOneShot(shootSound);

        return true;
    }
}