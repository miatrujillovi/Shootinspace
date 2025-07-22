using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Bala : MonoBehaviour
{
    [Header("Daño")]
    [SerializeField] private float damage = 10f;

    [Header("Explosión")]
    [SerializeField] private float explosionRadius;
    [SerializeField] private GameObject explosionVFX;
    public static bool ExplosiveBullets = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") &&
            collision.collider.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeDamage(damage);
        }

        if (ExplosiveBullets) SpawnExplosionTrigger();

        Destroy(gameObject);
    }


    private void SpawnExplosionTrigger()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider col in hitColliders)
        {
            if (col.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(damage);
            }
        }

        if (explosionVFX != null)
        {
            GameObject vfxInstance = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(vfxInstance, 0.25f);
        }
    }
}