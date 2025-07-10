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

    private float currentScore = 0;
    private float currentMulti = 1;
    [SerializeField]private int killCount = 0;

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

        if (ratio <= 1f / 3f)                                
        {
            scoreImage.color = colorFase1;                   
        }
        else if (ratio <= 2f / 3f)                          
        {
            scoreImage.color = colorFase2;                  
        }
        else                                            
        {
            scoreImage.color = colorFase3;               
        }

        if (multiText != null)
        {
            multiText.text = $"x{currentMulti:F1}";
        }
    }

private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddKill();
        }
    }
}
