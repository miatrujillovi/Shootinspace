public abstract class EnemyState
{
    // Called once when the state starts
    public virtual void Enter(Enemy e) { }

    // Called every Update
    public abstract void Tick(Enemy e);

    // Called once when the state ends
    public virtual void Exit(Enemy e) { }
}
