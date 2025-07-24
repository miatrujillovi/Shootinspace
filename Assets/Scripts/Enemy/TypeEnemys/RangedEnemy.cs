using UnityEngine;

public class RangedEnemy : EnemyBase
{
    [Header("Ranged")]
    public ProjectileShooter shooter;
    public float cooldown;

    public override bool IsInAttackRange()
        => Vector3.Distance(transform.position, player.position) <= rangedRange;

    public override float DoAttack()
    {
        //animator.SetTrigger("Shoot");
        shooter.Shoot(player.position);
        return cooldown;
    }
}
