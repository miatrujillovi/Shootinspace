using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private Transform[] levelStartLocation;
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform screenTransition;

    private int recoveredFuel;
    private int currentLevel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        currentLevel = 1;
        player = levelStartLocation[0];
    }

    public void FuelRecovered()
    {
        recoveredFuel++;
        currentLevel++;
        NextLevel(currentLevel);
    }

    public void NextLevel(int _nextLevel)
    {
        switch (_nextLevel)
        {
            case 2:
                LevelTransition(levelStartLocation[1]);
                break;

            case 3:
                LevelTransition(levelStartLocation[2]);
                break;

            default:
                Debug.LogError("Levels weren't calculated correctly.");
                break;
        }
    }

    private void Start()
    {
        LevelTransition(levelStartLocation[1]);
    }

    public void LevelTransition(Transform newPosition)
    {
        screenTransition.DOScale(Vector3.one * 3f, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            player.position = newPosition.position;
            screenTransition.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutQuad);
        });
    }

    private void Update()
    {
        if (recoveredFuel >= 3)
        {
            //End Game
        }
    }
}
