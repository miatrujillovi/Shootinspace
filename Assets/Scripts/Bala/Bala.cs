using UnityEngine;

public class Bala : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject efectoExplosion;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<IDamageable>(out var target))
        {
            target.TomarDano(damage);
        }

        if (efectoExplosion != null)
            Instantiate(efectoExplosion, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
