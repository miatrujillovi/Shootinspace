using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Collider))]
public class MeleeHitbox : ParryableHitbox
{
    [Header("Stats")]
    [SerializeField] private float damage = 20f;
    [SerializeField] private float activeTime = 0.15f;
    [SerializeField] private LayerMask targetMask;

    private EnemyBase _owner;

    Collider _col;

    void Awake()
    {
        _col = GetComponent<Collider>();
        _col.isTrigger = true;
        _col.enabled = false;
        _owner = GetComponentInParent<EnemyBase>();
    }


    public void ActivateHitbox()
    {
        StopAllCoroutines();
        StartCoroutine(ActiveWindow());
    }

    IEnumerator ActiveWindow()
    {
        _col.enabled = true;
        yield return new WaitForSeconds(activeTime);
        _col.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if ((targetMask.value & (1 << other.gameObject.layer)) == 0) return;

        if (other.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeDamage(damage, transform.position);
        }

        _col.enabled = false;
    }

    public override void OnParried(Vector3 dir, GameObject source, bool perfect)
    {
        _col.enabled = false;
    }

}