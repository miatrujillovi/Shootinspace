using System;
using UnityEngine;

public static class EnemyEvents
{
    public static event Action<GameObject> OnEnemyDeath;

    public static void NotificarMuerte(GameObject enemigo) => OnEnemyDeath?.Invoke(enemigo);
}
