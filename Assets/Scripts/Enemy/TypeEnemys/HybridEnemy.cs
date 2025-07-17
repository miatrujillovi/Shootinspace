using UnityEngine;

public class HybridEnemy : EnemyBase
{
    public enum AttackMode { Melee, Ranged }

    [Header("Melee")]
    public MeleeHitbox melee;

    [Header("Ranged")]
    public ProjectileShooter shooter;

    public float meleeCooldown = 1f;
    public float rangedCooldown = 1.2f;

    private AttackMode currentMode;

    private float meleeChance = 0.5f;
    private float rangedChance => 1f - meleeChance;

    private bool hasAttackedInCurrentMode = false;

    public float TimeSinceLastAttack { get; private set; } = 0f;
    public float ModeTimeout => 5f;

    protected override void Start()
    {
        base.Start();
        ChooseAttackMode();
    }

    protected override void Update()
    {
        base.Update();

        TickAttackTimer(Time.deltaTime);

        if (!HasAttacked && TimeSinceLastAttack >= ModeTimeout)
        {
            Debug.Log("Forzando cambio de modo por inactividad");
            ForceRecalculateAttackMode();
        }
    }


    public void TickAttackTimer(float deltaTime)
    {
        TimeSinceLastAttack += deltaTime;
    }

    public void PrepareForNextAttackCycle()
    {
        hasAttackedInCurrentMode = false;
        ResetAttackTimer();
    }

    public void ResetAttackTimer()
    {
        TimeSinceLastAttack = 0f;
    }

    public void MarkAttackSuccessful()
    {
        hasAttackedInCurrentMode = true;
        ResetAttackTimer();
    }

    public bool HasAttacked => hasAttackedInCurrentMode;

    public void ForceRecalculateAttackMode()
    {
        AdjustProbabilities();
        ChooseAttackMode();
    }

    private void ChooseAttackMode()
    {
        float roll = Random.value;
        currentMode = roll <= meleeChance ? AttackMode.Melee : AttackMode.Ranged;

        Debug.Log($"Nuevo modo de ataque: {currentMode}");

        hasAttackedInCurrentMode = false;
        ResetAttackTimer();
    }

    private void AdjustProbabilities()
    {
        const float adjustAmount = 0.2f;

        if (currentMode == AttackMode.Melee)
        {
            meleeChance = Mathf.Clamp01(meleeChance - adjustAmount);
        }
        else if (currentMode == AttackMode.Ranged)
        {
            meleeChance = Mathf.Clamp01(meleeChance + adjustAmount);
        }

        Debug.Log($"Probabilidades ajustadas -> Melee: {meleeChance}, Ranged: {rangedChance}");
    }

    public override bool IsInAttackRange()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        return currentMode == AttackMode.Melee && dist <= meleeRange
            || currentMode == AttackMode.Ranged && dist <= rangedRange;
    }

    public override float DoAttack()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (currentMode == AttackMode.Melee && dist <= meleeRange)
        {
            melee.ActivateHitbox();
            MarkAttackSuccessful();
            return meleeCooldown;
        }

        if (currentMode == AttackMode.Ranged && dist <= rangedRange)
        {
            shooter.Shoot(player.position);
            MarkAttackSuccessful();
            return rangedCooldown;
        }

        return 0f;
    }
}
