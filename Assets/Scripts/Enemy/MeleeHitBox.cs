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
        yield return new WaitForSeconds(activeTime);
        _col.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Entr� en el trigger con: {other.name}");

        // Ignorar si es el due�o (por jerarqu�a)
        if (other.transform.root.gameObject == owner)
        {
            Debug.Log("Es el due�o (root), ignorando.");
            return;
        }

        // Verificar que est� en la capa correcta
        if ((targetMask.value & (1 << other.gameObject.layer)) == 0)
        {
            Debug.Log("No es un objetivo v�lido.");
            return;
        }

        // Intentar aplicar da�o
        if (other.TryGetComponent<IDamageable>(out var target))
        {
            Debug.Log("Haciendo da�o al objetivo.");
            target.TakeDamage(damage, transform.position);
        }

        _col.enabled = false;
    }



    public override void OnParried(Vector3 dir, GameObject source, bool perfect)
    {
        _col.enabled = false;
    }

}