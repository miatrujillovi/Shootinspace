using UnityEngine;

public class ParryField : MonoBehaviour
{
    private GameObject source;          // quién hizo el parry (jugador)
    private Transform lookTransform;    // de dónde tomamos el forward
    private float perfectWindow;

    public void Init(GameObject _source, Transform _lookTransform,
                     float _perfectWindow, float duration, float radius)
    {
        source = _source;
        lookTransform = _lookTransform;
        perfectWindow = _perfectWindow;

        var col = gameObject.AddComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = radius;

        Destroy(gameObject, duration);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IParryable parryable))
        {
            bool perfect = Time.time - other.GetComponent<ParryableHitbox>().spawnTime
                            <= parryable.ParryWindow;

            Vector3 dir = lookTransform.forward.normalized;

            parryable.OnParried(dir, source, perfect);
            parryable.PlayParryFeedback(perfect);
        }
    }
}
