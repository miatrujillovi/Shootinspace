using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("References for the Manager")]
    [SerializeField] [Tooltip("The three starting locations for each level.")] private Vector3[] levelStartLocation;
    [SerializeField] [Tooltip("Position of the player to later change it for the level.")] private Transform player;
    [SerializeField] [Tooltip("Character Controller of the player to deactivate and activate.")] private CharacterController playerCR;
    [SerializeField] [Tooltip("Canvas with the gameplay UI elements.")] private GameObject gameplayScreen;
    [SerializeField] [Tooltip("Canvas with the win UI elements.")] private GameObject winScreen;
    [SerializeField] [Tooltip("Canvas with the lose UI elements")] private GameObject deathScreen;
    [SerializeField] [Tooltip("Camara script of the Player to stop it during death and win screens.")] private Camara playerCamaraScript;
    [SerializeField] private Health playerHealth;
    [SerializeField] private Camera playerCamera;
    [SerializeField] [Tooltip("List of existing enemy spawners per level.")] private List<EnemySpawner> levelSpawners;

    [HideInInspector] public bool isFuelUnlocked = false;
    [HideInInspector] public bool hasTriggered = false;
    public int enemiesRemaining;
    private int recoveredFuel;
    private int currentLevel;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } 
        else if (Instance != this)
        {
            Destroy(gameObject); //Destroying duplicated Instance
            return;
        }

        RestartGame();
    }

    //Restarting Game Variables, Positions and Screens
    public void RestartGame()
    {
        //Restarting Level Logic
        recoveredFuel = 0;
        currentLevel = 1;
        isFuelUnlocked = false;
        hasTriggered = false;

        playerHealth.RestoreHealth();

        //Restarting UI/Screen Logic
        winScreen.SetActive(false);
        deathScreen.SetActive(false);
        gameplayScreen.SetActive(true);

        //Restarting Player Logic

        AudioManager.Instance.IsInMenu = false;

        //Restarting
        NextLevel(currentLevel);
    }

    //When players health reaches zero
    public void PlayerDeath()
    {
        gameplayScreen.SetActive(false);
        deathScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
        AudioManager.Instance.IsInMenu = true;
    }


    //When player takes the fuel on the level (CAN ONLY HAPPEN WHEN ALL ENEMIES IN LEVEL HAVE BEEN DEFEATED)
    public void FuelRecovered()
    {
        recoveredFuel++;
        currentLevel++;
        UIManager.Instance.HideFuel();
        NextLevel(currentLevel);
    }

    //Function to change things onto NextLevel
    public void NextLevel(int _nextLevel)
    {
        UIManager.Instance.HideFuel();

        if (_nextLevel > levelStartLocation.Length)
        {
            return;
        }

        isFuelUnlocked = false;
        hasTriggered = false;

        foreach (var spawner in levelSpawners)
        {
            spawner.gameObject.SetActive(false);
        }

        // Activamos el spawner del nivel actual
        EnemySpawner currentSpawner = levelSpawners[_nextLevel - 1];
        currentSpawner.gameObject.SetActive(true);
        currentSpawner.Restarting();

        enemiesRemaining = currentSpawner.maxEnemies;

        LevelTransition(levelStartLocation[_nextLevel - 1]);
    }

    //Screen Transition to Move player to the next Level
    public void LevelTransition(Vector3 newPosition)
    {
        playerCR.enabled = false;
        player.position = newPosition;
        playerCR.enabled = true;
    }

    //If a enemy dies, substracts from enemiesRemaining and verifies if its 0 to unlock fuel
    public void OnEnemyDefeated()
    {
        enemiesRemaining--;

        if (enemiesRemaining == 0)
        {
            UIManager.Instance.CollectFuel();
            isFuelUnlocked = true;
        }
    }

    //Verifies the amount of recoveredFuel
    private void Update()
    {
        if (recoveredFuel >= 3)
        {
            PlayerHasWon();
        }
    }

    //Player got all three fuels
    private void PlayerHasWon()
    {
        winScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
        AudioManager.Instance.IsInMenu = true;
    }

    public void TriggerShake()
    {
        Quaternion originalRotation = playerCamera.transform.rotation;

        playerCamera.transform.DOComplete();

        playerCamera.transform
            .DOShakeRotation(0.5f, 5f, 10, 90f)
            .SetEase(Ease.OutQuad)
            .SetUpdate(false)
            .OnComplete(() =>
            {
                playerCamera.transform.rotation = originalRotation;
            });
    }
}
