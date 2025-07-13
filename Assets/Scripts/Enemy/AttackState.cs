using UnityEngine;

public class AttackState : EnemyState
{
    float _cooldown;

    public override void Enter(Enemy e)
    {
        e.agent.isStopped = true;   // Quieto para atacar
        _cooldown = 0f;
    }

    public override void Tick(Enemy e)
    {
        // Mirar al player
        Vector3 flatDir = (e.player.position - e.transform.position);
        flatDir.y = 0f;
        if (flatDir != Vector3.zero)
            e.transform.rotation = Quaternion.Slerp(e.transform.rotation,
                                                    Quaternion.LookRotation(flatDir),
                                                    5f * Time.deltaTime);

        _cooldown -= Time.deltaTime;
        if (_cooldown > 0f) return;

        float dist = Vector3.Distance(e.transform.position, e.player.position);
        bool didAttack = false;

        switch (e.attackMode)
        {
            case Enemy.AttackMode.Melee:
                if (dist <= e.meleeRange) didAttack = Melee(e);
                break;

            case Enemy.AttackMode.Ranged:
                if (dist <= e.rangedRange) didAttack = Ranged(e);
                break;

            case Enemy.AttackMode.Both:
                // Elegir según distancia
                if (dist <= e.meleeRange) didAttack = Melee(e);
                else if (dist <= e.rangedRange) didAttack = Ranged(e);
                break;
        }

        if (didAttack) _cooldown = 1f; // Cooldown sencillo. Sustitúyelo por anim events
        else
        {
            // Salir si está fuera de rango
            if (dist > e.rangedRange)
                e.SwitchState(e.chase);
        }
    }

    bool Ranged(Enemy e)
    {
        e.animator.SetTrigger("Shoot");
        e.shooter.Shoot(e.player.position);
        return true;
    }

    bool Melee(Enemy e)
    {
        e.animator.SetTrigger("Melee");
        e.melee.ActivateHitbox();   // Tu método para habilitar collider/daño
        return true;
    }

    public override void Exit(Enemy e)
    {
        e.agent.isStopped = false; // Volver a moverse
    }
}
