using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Variables for Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [Space]
    [Header("References for UI")]
    [SerializeField] private Image hpFiller;
    [SerializeField] private RectTransform _heart;

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

        //Shake the heart icon
        _heart.DOShakeAnchorPos(
            duration: 0.6f,        // total shake time
            strength: 40f,         // how far it moves
            vibrato: 60,           // how many times it vibrates
            randomness: 90,        // variation in direction
            snapping: false,
            fadeOut: true          // shake less over time
        ).SetEase(Ease.InOutExpo);

        currentHealth -= amount;
        hpFiller.fillAmount = currentHealth / maxHealth;
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
