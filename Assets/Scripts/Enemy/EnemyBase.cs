using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    [Header("Config gen�rica")]
    public float sightRadius = 15f;
    public float lostTargetDelay = 3f;
    public float vidaMax;
    public float vidaActual;

    [Header("Rangos (Los que uses)")]
    public float meleeRange = 2f;
    public float rangedRange = 8f;

    [Header("Refs")]
    public NavMeshAgent agent;
    public Transform[] patrolPoints;
    public Animator animator;
    public Transform player;

    public bool stuned;

    protected EnemyState _current;
    private bool isJumping;
    public readonly PatrolState patrol = new PatrolState();
    public readonly ChaseState chase = new ChaseState();
    public readonly AttackState attack = new AttackState();

    protected virtual void Awake() => vidaActual = vidaMax;

    protected virtual void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        SwitchState(patrol);
    }

    protected virtual void Update()
    {
        if (agent.isOnOffMeshLink && !isJumping)
        {
            StartCoroutine(HandleJump(agent.currentOffMeshLinkData));
        }

        _current?.Tick(this);
    }


    public void SwitchState(EnemyState next)
    {
        _current?.Exit(this);
        _current = next;
        _current?.Enter(this);
    }

    public virtual void TakeDamage(float amount)
    {
        vidaActual -= amount;
        if (vidaActual <= 0f)
        {
            StartCoroutine(stunCoroutine());
            vidaActual = vidaMax / 2;
        }
    }
    public virtual void TakeDamage(float amount, Vector3 _) => TakeDamage(amount);

    protected virtual void Morir()
    {
        EnemyEvents.NotificarMuerte(gameObject);
        Destroy(gameObject);
    }

    // Corrutinas

    private IEnumerator HandleJump(OffMeshLinkData data)
    {
        isJumping = true;
        agent.isStopped = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = data.endPos + Vector3.up * 0.5f;

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float height = Mathf.Sin(Mathf.PI * t) * 1.5f; 
            transform.position = Vector3.Lerp(startPos, endPos, t) + Vector3.up * height;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        agent.CompleteOffMeshLink();
        agent.isStopped = false;
        isJumping = false;
    }


    public virtual IEnumerator stunCoroutine()
    {
        agent.speed = 0;
        stuned = true;
        Debug.Log("Esta aturdido el enemigo");
        yield return new WaitForSeconds(5f);
        agent.speed = 2.5f;
        stuned = false;
        Debug.Log("Ya no esta aturdido el enemigo");
    }

    // ------------------

    public virtual void OnCollisionEnter(Collision col)
    {
        if (stuned)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                if (Input.GetKey(KeyCode.E))
                {
                    Morir();
                }
            }
        }
    }

    public virtual void OnCollisionStay(Collision col)
    {
        if (stuned)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                if (Input.GetKey(KeyCode.E))
                {
                    Morir();
                }
            }
        }
    }

    public abstract bool IsInAttackRange();

    public abstract float DoAttack();
}