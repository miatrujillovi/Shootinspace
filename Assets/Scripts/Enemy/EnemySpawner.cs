using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemigos con pesos")]
    [SerializeField] private List<EnemySpawnData> enemyTypes = new List<EnemySpawnData>();

    [Header("Puntos de spawn")]
    [SerializeField] private Vector3 center = Vector3.zero;   
    [SerializeField] private float spawnRadius;         
    [SerializeField] private float maxNavSampleDistance; 


    [Header("Configuración")]
    [SerializeField] private float spawnInterval;
    [SerializeField] public int maxEnemies;

    private int spawnedEnemies = 0;
    private float timer;

    private void Start()
    {
        SpawnEnemy();
        timer = 0f;
    }

    private void Update()
    {
        if (spawnedEnemies >= maxEnemies) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    public void Restarting()
    {
        spawnedEnemies = 0;
        timer = 0f;

        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        LevelManager.Instance.OnEnemySpawned();
        GameObject selectedPrefab = GetRandomEnemyByWeight();

        Vector3 randomPosition;
        if (GetRandomPointOnNavMesh(center, spawnRadius, out randomPosition))
        {
            Instantiate(selectedPrefab, randomPosition, Quaternion.identity);
            spawnedEnemies++;
        }
        else
        {
            Debug.LogWarning("No se encontró un punto válido sobre el NavMesh.");
        }
    }

    private bool GetRandomPointOnNavMesh(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++) 
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            randomPoint.y = center.y; 

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, maxNavSampleDistance, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }


    private GameObject GetRandomEnemyByWeight()
    {
        float totalWeight = 0f;
        foreach (var enemy in enemyTypes)
        {
            totalWeight += enemy.weight;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulative = 0f;

        foreach (var enemy in enemyTypes)
        {
            cumulative += enemy.weight;
            if (randomValue <= cumulative)
            {
                return enemy.prefab;
            }
        }

        return enemyTypes[0].prefab;
    }

    private void OnDrawGizmosSelected()
    {
        // Esfera sólida semitransparente
        Gizmos.color = new Color(1f, 0.6f, 0.6f, 0.6f);
        Gizmos.DrawSphere(center, spawnRadius);

        // Contorno más fuerte
        Gizmos.color = new Color(1f, 0.6f, 0.6f, 1f);
        Gizmos.DrawWireSphere(center, spawnRadius);
    }

}


[System.Serializable]
public class EnemySpawnData
{
    public GameObject prefab;
    [Range(0, 100)]
    public float weight;
}

