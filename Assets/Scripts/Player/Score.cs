using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Score : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image scoreImage;
    [SerializeField] private TextMeshProUGUI multiText;
    [SerializeField] private Color colorFase1;
    [SerializeField] private Color colorFase2;
    [SerializeField] private Color colorFase3;

    [Header("Configuración")]
    [SerializeField] private float scorePerKill;
    [SerializeField] private float multiPerKill;
    [SerializeField] private float maxScoreVisible;

    [Header("Referencias")]
    [SerializeField] private PlayerMovement player;

    private float currentScore = 0;
    private float currentMulti = 1;
    private int killCount = 0;
    public int currentPhase = 0;

    private float timeSinceLastKill = 0f;
    private const float killTimeout = 20f;

    private void OnEnable()
    {
        EnemyEvents.OnEnemyDeath += OnEnemyDeath;
    }

    private void OnDisable()
    {
        EnemyEvents.OnEnemyDeath -= OnEnemyDeath;
    }

    void Start()
    {
        UpdateUI();
    }

    public void OnEnemyDeath(GameObject enemy)
    {
        AddKill(); 
    }

    public void AddKill()
    {
        killCount++;
        currentMulti = 1 + (killCount * multiPerKill);

        float scoreToAdd = scorePerKill * currentMulti;
        currentScore += scoreToAdd;

        timeSinceLastKill = 0f; //Reseting timer

        UpdateUI();
    }

    public void ResetKill()
    {
        killCount = 0;
        currentMulti = 1;
        currentScore = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (maxScoreVisible <= 0f) return;

        float ratio = Mathf.Clamp01(currentScore / maxScoreVisible);
        scoreImage.fillAmount = ratio;

        if (ratio < 0.33f) { }
        else if (ratio < 0.66f)
            scoreImage.color = colorFase2;       // Fase 1
        else if (ratio < 0.99f)
            scoreImage.color = colorFase3;       // Fase 2
        else
            scoreImage.color = colorFase3;       // Fase 3 (mismo color u otro si deseas)

        if (multiText)
            multiText.text = $"x{currentMulti:F1}";

        int newPhase;
        if (ratio < 0.33f) newPhase = 0;
        else if (ratio < 0.66f) newPhase = 1;
        else if (ratio < 0.99f) newPhase = 2;
        else newPhase = 3;

        if (newPhase != currentPhase)
        {
            currentPhase = newPhase;
            ApplyAbilities(currentPhase);
        }
    }


    private void ApplyAbilities(int phase)
    {
        switch (phase)
        {
            // Fase 0: Sin habilidades
            case 0:
                break;

             // Fase 1: Sprint
            case 1:
                player.ActivateSprint();
                break;

            // Fase 2: Sprint + Doble salto
            case 2:
                player.ActivateDoubleJump();
                break;

            // Fase 3: Sprint + Doble salto + Balas explosivasww
            case 3: 
                Bala.ExplosiveBullets = true;
                break;
        }
    }

    private void Update()
    {
        timeSinceLastKill += Time.deltaTime;

        if (timeSinceLastKill >= killTimeout)
        {
            ResetKill();
            timeSinceLastKill = 0f;
        }

        /*if (Input.GetKeyDown(KeyCode.P))
        {
            AddKill();
        }*/
    }
}
