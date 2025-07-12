using UnityEngine;

public class AtaqueHitbox : ParryableHitbox
{
    [SerializeField] private float speedAfterParry = 60f;
    [SerializeField] private float perfectMultiplier = 1.5f;

    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    public override void OnParried(Vector3 dir, GameObject source, bool perfect)
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");

        float finalSpeed = speedAfterParry * (perfect ? perfectMultiplier : 1f);
        rb.linearVelocity = dir.normalized * finalSpeed;

        transform.forward = dir;
    }
}
