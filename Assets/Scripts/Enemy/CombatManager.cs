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
        if (AudioManager.Instance != null && AudioManager.Instance.IsInMenu)
        {
            AudioManager.Instance.PlayInterludeMusic();
            return;
        }

        AudioManager.Instance?.PlayFightMusic();

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
