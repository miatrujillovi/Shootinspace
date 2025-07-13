using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ExplosionTrigger : MonoBehaviour
{
    private float damage;

    public void Init(float dmg, float lifeTime = 0.1f)
    {
        damage = dmg;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeDamage(damage);
        }
    }
}
