using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Variables for Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [Space]
    [Header("References for UI")]
    [SerializeField] private Image hpFiller;

    //Invincibility at the start of the game
    private float spawnInvincibilityTime = 1f;
    private float spawnTime;

    //Player health regeneration variables
    private float regenerationThreshold = 3f;
    private float regenerationRate = 5f;
    private Coroutine playerRegeneration;

    private void Awake()
    {
        currentHealth = maxHealth;
        spawnTime = Time.time;
    }

    public void TakeDamage(float amount)
    {
        if (Time.time - spawnTime < spawnInvincibilityTime) return; //Invulnerable al inicio por 1s

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        hpFiller.fillAmount = currentHealth / maxHealth;

        //Logic for Player Regeneration
        if (playerRegeneration != null)
        {
            StopCoroutine(playerRegeneration);
        }

        playerRegeneration = StartCoroutine(RegenerateHealth());

        if (currentHealth <= 0) {
            PlayerDeath(); 
        }
    }

    public void TakeDamage(float amount, Vector3 hitPoint)
    {
        TakeDamage(amount);
    }

    private IEnumerator RegenerateHealth()
    {
        yield return new WaitForSeconds(regenerationThreshold);

        while (currentHealth < maxHealth)
        {
            currentHealth += regenerationRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            hpFiller.fillAmount = currentHealth / maxHealth;
            yield return null;
        }

        playerRegeneration = null;
    }

    private void PlayerDeath()
    {
        Debug.Log("Player has died");
        //Dead Logic Here
    }
}
