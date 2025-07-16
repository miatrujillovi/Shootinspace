using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : ParryableHitbox
{
    [SerializeField] private float lifeTime = 4f;
    Rigidbody _rb;
    [SerializeField] private float _damage;
    GameObject _owner;

    void Awake() => _rb = GetComponent<Rigidbody>();

    public void Init(Vector3 velocity, float damage, GameObject owner)
    {
        _damage = damage;
        _owner = owner;
        _rb.linearVelocity = velocity;

        Collider col = GetComponent<Collider>();
        col.enabled = false;   
        col.enabled = true;

        CancelInvoke();
        Invoke(nameof(Despawn), lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _owner) return;

        if (other.TryGetComponent<IDamageable>(out var target))
            target.TakeDamage(_damage, transform.position);

        Despawn();
    }

    public override void OnParried(Vector3 dir, GameObject source, bool perfect)
    {
        _owner = source;              
        _rb.linearVelocity = dir.normalized * _rb.linearVelocity.magnitude; 
    }

    void Despawn()
    {
        if (PoolManager.Instance)
            PoolManager.Instance.Release(gameObject);
        else
            Destroy(gameObject);
    }
}
