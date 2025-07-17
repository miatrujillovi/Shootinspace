using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private Vector3[] levelStartLocation;
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform screenTransition;
    [SerializeField] private GameObject gameplayScreen;
    [SerializeField] private GameObject winScreen;

    [SerializeField] private List<EnemySpawner> levelSpawners;
    [SerializeField] private List<Terrain> terrains;

    [HideInInspector] public bool isFuelUnlocked = false;
    private int enemiesRemaining;
    private int recoveredFuel;
    private int currentLevel;
    private TerrainTextureBlender terrainTextureBlender;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        terrainTextureBlender = GetComponent<TerrainTextureBlender>();

        recoveredFuel = 0;
        currentLevel = 1;

        winScreen.SetActive(false);
        gameObject.SetActive(true);

        player.position = levelStartLocation[0];

        for (int i = 0; i < levelSpawners.Count; i++)
        {
            levelSpawners[i].gameObject.SetActive(false);
        }
        levelSpawners[0].gameObject.SetActive(true);

        isFuelUnlocked = false;
        enemiesRemaining = 0;
    }

    private void Start()
    {
        StartCoroutine(terrainTextureBlender.BlendTexturesRoutine(1, 0, terrains[0]));
        StartCoroutine(terrainTextureBlender.BlendTexturesRoutine(2, 0, terrains[1]));
        StartCoroutine(terrainTextureBlender.BlendTexturesRoutine(3, 0, terrains[2]));
    }

    //When player takes the fuel on the level (CAN ONLY HAPPEN WHEN ALL ENEMIES IN LEVEL HAVE BEEN DEFEATED)
    public void FuelRecovered()
    {
        recoveredFuel++;
        currentLevel++;
        NextLevel(currentLevel);
    }

    //Function to change things onto NextLevel
    public void NextLevel(int _nextLevel)
    {
        if (_nextLevel > levelStartLocation.Length || _nextLevel > levelSpawners.Count)
        {
            Debug.LogError("No hay mas niveles disponibles.");
        }

        enemiesRemaining = 0;
        isFuelUnlocked = false;

        foreach (var spawner in levelSpawners)
        {
            spawner.gameObject.SetActive(false);
        }

        levelSpawners[_nextLevel - 1].gameObject.SetActive(true);

        LevelTransition(levelStartLocation[_nextLevel - 1]);
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

    public void OnEnemySpawned()
    {
        enemiesRemaining++;
    }

    public void OnEnemyDefeated()
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            ChangeTerrainTexture(currentLevel);
            isFuelUnlocked = true;
        }
    }

    private void ChangeTerrainTexture(int currentLevel)
    {
        switch (currentLevel)
        {
            case 1:
                StartCoroutine(terrainTextureBlender.BlendTexturesRoutine(0, 1, terrains[0]));
                break;

            case 2:
                StartCoroutine(terrainTextureBlender.BlendTexturesRoutine(0, 2, terrains[1]));
                break;

            case 3:
                StartCoroutine(terrainTextureBlender.BlendTexturesRoutine(0, 3, terrains[2]));
                break;

            default:
                Debug.LogError("Error while blending textures of terrain.");
                break;
        }
    }

    private void Update()
    {
        if (recoveredFuel >= 3)
        {
            gameObject.SetActive(false);
            winScreen.SetActive(true);
        }
    }
}
