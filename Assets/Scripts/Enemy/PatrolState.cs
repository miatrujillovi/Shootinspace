using UnityEngine;

public class PatrolState : EnemyState
{
    int _nextPoint;

    public override void Enter(Enemy e)
    {
        if (e.patrolPoints.Length == 0) return;
        e.agent.isStopped = false;
        MoveToNext(e);
    }

    public override void Tick(Enemy e)
    {
        // Transition: player a la vista
        if (Vector3.Distance(e.transform.position, e.player.position) <= e.sightRadius)
        {
            e.SwitchState(e.chase);
            return;
        }

        // Llegó al punto -> ir al siguiente
        if (!e.agent.pathPending && e.agent.remainingDistance <= 0.2f)
            MoveToNext(e);
    }

    void MoveToNext(Enemy e)
    {
        if (e.patrolPoints.Length == 0) return;
        e.agent.destination = e.patrolPoints[_nextPoint].position;
        _nextPoint = (_nextPoint + 1) % e.patrolPoints.Length;
    }
}
