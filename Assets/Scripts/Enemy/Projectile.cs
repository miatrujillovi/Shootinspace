using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : ParryableHitbox
{
    [SerializeField] private float lifeTime = 4f;
    [SerializeField] private float _damage;

    private Rigidbody _rb;
    private GameObject _owner;

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

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _owner)
            return;

        if (other.TryGetComponent<IDamageable>(out var target))
            target.TakeDamage(_damage, transform.position);

        Despawn();
    }
    */

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == _owner)
            return;

        // Use GetComponent instead of TryGetComponent since Collision does not have TryGetComponent
        var target = collision.gameObject.GetComponent<IDamageable>();
        if (target != null)
            target.TakeDamage(_damage, transform.position);

        Despawn();
    }

    public override void OnParried(Vector3 dir, GameObject source, bool perfect)
    {
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
