using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image scoreImage;
    [SerializeField] private TextMeshProUGUI multiText;

    [Header("Configuración")]
    [SerializeField] private float scorePerKill;
    [SerializeField] private float multiPerKill;
    [SerializeField] private float maxScoreVisible; 
    private float currentScore = 0;
    private float currentMulti = 1;
    private int killCount = 0;

    void Start()
    {
        UpdateUI();
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
        if (scoreImage != null)
        {
            float fillAmount = Mathf.Clamp01(currentScore / maxScoreVisible);
            scoreImage.fillAmount = fillAmount;
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
