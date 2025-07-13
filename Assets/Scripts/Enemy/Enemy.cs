using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour, IDamageable
{
    public enum AttackMode { Melee, Ranged, Both }

    [Header("Config")]
    public AttackMode attackMode = AttackMode.Melee;
    public float sightRadius;
    public float meleeRange;
    public float rangedRange;
    public float lostTargetDelay;
    public float vidaMax;
    private float vidaActual;

    [Header("Refs")]
    public NavMeshAgent agent;
    public Transform[] patrolPoints;
    public Animator animator;
    public ProjectileShooter shooter;
    public MeleeHitbox melee;
    public Transform player;

    EnemyState _current;
    public readonly PatrolState patrol = new PatrolState();
    public readonly ChaseState chase = new ChaseState();
    public readonly AttackState attack = new AttackState();

    void Start()
    {
        // Opcional: encontrar player automático
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;

        // Arrancamos en patrulla
        SwitchState(patrol);
    }

    void Update() => _current?.Tick(this);

    public void SwitchState(EnemyState next)
    {
        _current?.Exit(this);
        _current = next;
        _current?.Enter(this);
    }
    private void Awake() => vidaActual = vidaMax;

    public void TakeDamage(float amount)
    {
        vidaActual -= amount;

        if (vidaActual <= 0f)
        {
            Morir();
        }
    }

    public void TakeDamage(float amount, Vector3 hitPoint)
    {
        TakeDamage(amount);
    }


    private void Morir()
    {
        EnemyEvents.NotificarMuerte(gameObject);
        Destroy(gameObject);
    }

}
