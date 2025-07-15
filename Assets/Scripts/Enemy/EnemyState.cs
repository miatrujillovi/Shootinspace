public abstract class EnemyState
{
    public virtual void Enter(EnemyBase e) { }
    public abstract void Tick(EnemyBase e);
    public virtual void Exit(EnemyBase e) { }
}
