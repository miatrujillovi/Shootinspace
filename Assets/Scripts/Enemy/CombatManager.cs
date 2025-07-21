using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    private List<EnemyBase> activeEnemies = new List<EnemyBase>();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        bool anyInCombat = false;

        foreach (var enemy in activeEnemies)
        {
            if (enemy == null) continue;

            if (enemy.IsInCombatState())
            {
                anyInCombat = true;
                Debug.Log($"Enemy {enemy.name} está en combate ({enemy._current.GetType().Name})");
                break;
            }
        }

        if (anyInCombat)
        {
            AudioManager.Instance?.PlayFightMusic();
        }
        else
        {
            AudioManager.Instance?.PlayInterludeMusic();
        }
    }


    public void RegisterEnemy(EnemyBase e)
    {
        if (!activeEnemies.Contains(e))
            activeEnemies.Add(e);
    }

    public void UnregisterEnemy(EnemyBase e)
    {
        activeEnemies.Remove(e);
    }
}
