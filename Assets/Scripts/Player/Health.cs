using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Variables for Health")]
    [SerializeField] private float maxHealth;
    [Space]
    [Header("References for UI")]
    [SerializeField] private Image hpFiller;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth > 0f)
        {
            currentHealth -= amount;
            hpFiller.fillAmount = currentHealth;
        }
        else
        {
            PlayerDeath();
        }
    }

    public void TakeDamage(float amount, Vector3 hitPoint)
    {
        TakeDamage(amount);
    }

    private void PlayerDeath()
    {
        Debug.Log("Player has died");
        //Dead Logic Here
    }
}
