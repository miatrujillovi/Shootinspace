using UnityEngine;

public class PatrolState : EnemyState
{
    int _nextPoint;

    public override void Enter(EnemyBase e)
    {
        if (e.patrolPoints.Length == 0) return;
        e.agent.isStopped = false;
        MoveToNext(e);
    }

    public override void Tick(EnemyBase e)
    {
        if (e.IsInAttackRange())          
        {
            e.SwitchState(e.attack);
            return;
        }

        if (!e.agent.pathPending && e.agent.remainingDistance <= 0.2f)
            MoveToNext(e);
    }

    void MoveToNext(EnemyBase e)
    {
        if (e.patrolPoints.Length == 0) return;
        e.agent.destination = e.patrolPoints[_nextPoint].position;
        _nextPoint = (_nextPoint + 1) % e.patrolPoints.Length;
    }
}
