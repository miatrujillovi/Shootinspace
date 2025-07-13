using UnityEngine;

public class ChaseState : EnemyState
{
    float _lostTimer;

    public override void Enter(Enemy e)
    {
        e.agent.isStopped = false;
        _lostTimer = 0f;
    }

    public override void Tick(Enemy e)
    {
        float dist = Vector3.Distance(e.transform.position, e.player.position);

        // Si está en rango de ataque, cambiar a Attack
        bool meleeReady = (e.attackMode != Enemy.AttackMode.Ranged) && dist <= e.meleeRange;
        bool rangedReady = (e.attackMode != Enemy.AttackMode.Melee) && dist <= e.rangedRange;

        if (meleeReady || rangedReady)
        {
            e.SwitchState(e.attack);
            return;
        }

        // Mantener la persecución
        e.agent.destination = e.player.position;

        // Player perdido
        if (dist > e.sightRadius) _lostTimer += Time.deltaTime;
        else _lostTimer = 0f;

        if (_lostTimer >= e.lostTargetDelay)
            e.SwitchState(e.patrol);
    }
}
