using UnityEngine;
using UnityEngine.UI;
using System;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Variables for Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [Space]
    [Header("References for UI")]
    [SerializeField] private Image hpFiller;

    private float spawnInvincibilityTime = 1f;
    private float spawnTime;

    private void Awake()
    {
        currentHealth = maxHealth;
        spawnTime = Time.time;
    }

    public void TakeDamage(float amount)
    {
        if (Time.time - spawnTime < spawnInvincibilityTime) return; //Invulnerable al inicio por 1s

        currentHealth -= amount;
        hpFiller.fillAmount = currentHealth / maxHealth;
        //Debug.Log($"[Health] TakeDamage called with {amount} at frame {Time.frameCount}\nStack Trace:\n{Environment.StackTrace}");
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
