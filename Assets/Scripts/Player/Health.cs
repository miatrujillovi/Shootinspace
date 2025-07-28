using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, IDamageable
{

    [Header("Variables for Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [Space]
    [Header("References for UI")]
    [SerializeField] private Image hpFiller;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;


    //Invincibility at the start of the game
    private float spawnInvincibilityTime = 1f;
    private float spawnTime;

    //Player health regeneration variables
    private float regenerationThreshold = 3f;
    private float regenerationRate = 5f;
    private Coroutine playerRegeneration;

    private void Awake()
    {
        RestoreHealth();
        spawnTime = Time.time;
    }

    public void TakeDamage(float amount)
    {
        if (Time.time - spawnTime < spawnInvincibilityTime) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        hpFiller.fillAmount = currentHealth / maxHealth;

        if (damageSound && audioSource) audioSource.PlayOneShot(damageSound);

        // Lógica de regeneración
        if (playerRegeneration != null)
        {
            StopCoroutine(playerRegeneration);
        }
        playerRegeneration = StartCoroutine(RegenerateHealth());

        if (currentHealth <= 0)
        {
            if (deathSound && audioSource) audioSource.PlayOneShot(deathSound);

            LevelManager.Instance.PlayerDeath();
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

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        hpFiller.fillAmount = currentHealth / maxHealth;
    }
}
