using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Variables for Health")]
    [SerializeField] private float maxHealth;
    [Space]
    [Header("References for UI")]
    [SerializeField] private Image hpFiller;

    [SerializeField] private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        hpFiller.fillAmount = currentHealth / maxHealth;
        Debug.Log($"Player took {amount} damage, current health: {currentHealth}");
        if (currentHealth <= 0) {
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
