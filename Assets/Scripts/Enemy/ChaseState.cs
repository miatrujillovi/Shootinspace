using UnityEngine;

public class ChaseState : EnemyState
{
    float _lostTimer;

    public override void Enter(EnemyBase e)
    {
        e.agent.isStopped = false;
        _lostTimer = 0f;
    }

    public override void Tick(EnemyBase e)
    {
        if (e.IsInAttackRange())
        {
            e.SwitchState(e.attack);
            return;
        }

        e.agent.destination = e.player.position;

        float dist = Vector3.Distance(e.transform.position, e.player.position);

        if (dist > e.sightRadius) _lostTimer += Time.deltaTime;
        else _lostTimer = 0f;

        if (_lostTimer >= e.lostTargetDelay)
            e.SwitchState(e.patrol);
    }
}
