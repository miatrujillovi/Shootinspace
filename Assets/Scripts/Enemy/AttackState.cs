using UnityEngine;

public class AttackState : EnemyState
{
    float _cooldown;

    public override void Enter(EnemyBase e)
    {
        e.agent.isStopped = true;
        _cooldown = 0f;
    }

    public override void Tick(EnemyBase e)
    {
        Vector3 dir = e.player.position - e.transform.position;
        dir.y = 0f;
        if (dir != Vector3.zero)
            e.transform.rotation = Quaternion.Slerp(e.transform.rotation,
                                                    Quaternion.LookRotation(dir),
                                                    5f * Time.deltaTime);

        _cooldown -= Time.deltaTime;
        if (_cooldown > 0f) return;

        if (e.IsInAttackRange())
        {
            _cooldown = Mathf.Max(0.1f, e.DoAttack()); 
        }
        else
        {
            e.SwitchState(e.chase);
        }
    }

    public override void Exit(EnemyBase e) => e.agent.isStopped = false;
}
