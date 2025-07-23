using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private Vector3[] levelStartLocation;
    [SerializeField] private Transform player;
    [SerializeField] private CharacterController playerCR;
    [SerializeField] private RectTransform screenTransition;
    [SerializeField] private GameObject gameplayScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private Camara playerCamaraScript;

    [SerializeField] private List<EnemySpawner> levelSpawners;

    [HideInInspector] public bool isFuelUnlocked = false;
    private int enemiesRemaining;
    private int recoveredFuel;
    private int currentLevel;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        RestartGame();
    }

    public void RestartGame()
    {
        //Restarting Level Logic
        recoveredFuel = 0;
        currentLevel = 1;

        //Restarting UI/Screen Logic
        winScreen.SetActive(false);
        deathScreen.SetActive(false);
        gameplayScreen.SetActive(true);

        //Restarting Player Logic
        playerCamaraScript.enabled = true;
        NextLevel(currentLevel);
    }

    public void PlayerDeath()
    {
        gameplayScreen.SetActive(false);
        deathScreen.SetActive(true);
        playerCamaraScript.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    //When player takes the fuel on the level (CAN ONLY HAPPEN WHEN ALL ENEMIES IN LEVEL HAVE BEEN DEFEATED)
    public void FuelRecovered()
    {
        recoveredFuel++;
        currentLevel++;
        Debug.Log("Se recupero fuel y avanzo a nivel: " + currentLevel);
        NextLevel(currentLevel);
    }

    //Function to change things onto NextLevel
    public void NextLevel(int _nextLevel)
    {
        if (_nextLevel > levelStartLocation.Length)
        {
            Debug.Log("Se completaron todos los niveles.");
            return;
        }

        enemiesRemaining = 0;
        isFuelUnlocked = false;

        foreach (var spawner in levelSpawners)
        {
            spawner.gameObject.SetActive(false);
        }

        levelSpawners[_nextLevel - 1].gameObject.SetActive(true);

        Debug.Log("Calling NextLevel with: " + _nextLevel);
        LevelTransition(levelStartLocation[_nextLevel - 1]);
    }

    //Screen Transition to Move player to the next Level
    public void LevelTransition(Vector3 newPosition)
    {
        playerCR.enabled = false;
        Debug.Log("It passed");
        Debug.Log("Posición antes: " + player.position);
        player.position = newPosition;
        Debug.Log("Posición después: " + player.position);
        playerCR.enabled = true;
    }

    public void OnEnemySpawned()
    {
        enemiesRemaining++;
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
            PlayerHasWon();
        }
    }

    private void PlayerHasWon()
    {
        gameObject.SetActive(false);
        winScreen.SetActive(true);
        playerCamaraScript.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }
}
