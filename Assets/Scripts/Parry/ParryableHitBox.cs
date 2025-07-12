using UnityEngine;

public abstract class ParryableHitbox : MonoBehaviour, IParryable
{
    [SerializeField] protected float parryWindow = 0.15f;
    [SerializeField] protected AttackKind kind;
    [SerializeField] protected ParticleSystem parryVFX;
    [SerializeField] protected AudioClip parrySFX;

    public float spawnTime;
    public float ParryWindow => parryWindow;
    public AttackKind Kind => kind;

    protected virtual void Awake() => spawnTime = Time.time;

    public virtual void PlayParryFeedback(bool perfect)
    {
        if (parryVFX) Instantiate(parryVFX, transform.position, Quaternion.identity);
        if (parrySFX) AudioSource.PlayClipAtPoint(parrySFX, transform.position);
    }

    public abstract void OnParried(Vector3 dir, GameObject source, bool perfect);
}
