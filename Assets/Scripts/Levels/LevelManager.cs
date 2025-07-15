using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private Vector3[] levelStartLocation;
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform screenTransition;

    [HideInInspector] public bool isFuelUnlocked = false;
    private int enemiesRemaining;
    private int recoveredFuel;
    private int currentLevel;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        recoveredFuel = 0;
        currentLevel = 1;
        //NextLevel(currentLevel);
    }

    //When player takes the fuel on the level
    public void FuelRecovered()
    {
        recoveredFuel++;
        currentLevel++;
        NextLevel(currentLevel);
    }

    //Function to change things onto NextLevel
    public void NextLevel(int _nextLevel)
    {
        switch (_nextLevel)
        {
            case 1:
                //Logic for First Level
                LevelTransition(levelStartLocation[0]);
                break;

            case 2:
                //Logic for Second Level
                LevelTransition(levelStartLocation[1]);
                break;

            case 3:
                //Logic for Third Level
                LevelTransition(levelStartLocation[2]);
                break;

            default:
                Debug.LogError("Levels weren't calculated correctly.");
                break;
        }
    }

    //Screen Transition to Move player to the next Level
    public void LevelTransition(Vector3 newPosition)
    {
        screenTransition.DOScale(Vector3.one * 3f, 1.3f).SetEase(Ease.InQuad).OnComplete(() =>
        {

            player.DOMove(newPosition, 0.5f);
            screenTransition.DOScale(Vector3.zero, 1.3f).SetEase(Ease.OutQuad);
        });
    }

    public void OnEnemyDefeated()
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            isFuelUnlocked = true;
        }
    }

    private void Update()
    {
        if (recoveredFuel >= 3)
        {
            //End Game
        }
    }
}
