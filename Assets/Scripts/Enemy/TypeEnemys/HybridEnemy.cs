using UnityEngine;

public class HybridEnemy : EnemyBase
{
    [Header("Melee")]
    public MeleeHitbox melee;
    [Header("Ranged")]
    public ProjectileShooter shooter;

    public float meleeCooldown = 1f;
    public float rangedCooldown = 1.2f;

    public override bool IsInAttackRange()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        return dist <= meleeRange || dist <= rangedRange;
    }

    public override float DoAttack()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= meleeRange)          
        {
            melee.ActivateHitbox();
            return meleeCooldown;
        }
        if (dist <= rangedRange)
        {
            shooter.Shoot(player.position);
            return rangedCooldown;
        }
        return 0f; 
    }
}
