using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Collider))]
public class MeleeHitbox : ParryableHitbox
{
    [Header("Stats")]
    [SerializeField] private float damage;
    [SerializeField] private float activeTime;
    [SerializeField] private LayerMask targetMask;

    [Header("Sounds")]
    [SerializeField] private AudioClip meleeSound;
    [SerializeField] private AudioSource audioSrc;

    private EnemyBase _owner;
    private GameObject owner;

    Collider _col;

    void Awake()
    {
        _col = GetComponent<Collider>();
        _col.isTrigger = true;
        _col.enabled = false;
        _owner = GetComponentInParent<EnemyBase>();
        owner = transform.root.gameObject;
    }


    public void ActivateHitbox()
    {
        StopAllCoroutines();
        StartCoroutine(ActiveWindow());
    }

    IEnumerator ActiveWindow()
    {
        _col.enabled = true;
        if (meleeSound) audioSrc?.PlayOneShot(meleeSound);
        yield return new WaitForSeconds(activeTime);
        _col.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject == owner || other.CompareTag("Enemy"))
        {
            return;
        }

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