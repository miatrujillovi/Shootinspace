using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    [Header("Melee")]
    public MeleeHitbox melee;
    public float cooldown = 1f;

    public override bool IsInAttackRange()
        => Vector3.Distance(transform.position, player.position) <= meleeRange;

    public override float DoAttack()
    {
        //animator.SetTrigger("Melee");
        melee.ActivateHitbox();
        return cooldown;
    }
}
