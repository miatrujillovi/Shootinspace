using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float vidaMax = 30f;
    private float vidaActual;

    private void Awake() => vidaActual = vidaMax;

    public void TomarDano(float amount)
    {
        vidaActual -= amount;

        if (vidaActual <= 0f)
        {
            Morir();
        }
    }

    private void Morir()
    {
        EnemyEvents.NotificarMuerte(gameObject);
        Destroy(gameObject);
    }

}
