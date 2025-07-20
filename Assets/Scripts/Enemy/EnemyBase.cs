using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    [Header("Config generica")]
    public float sightRadius = 15f;
    public float lostTargetDelay = 3f;
    public float vidaMax;
    public float vidaActual;

    [Header("Rangos (Los que uses)")]
    public float meleeRange;
    public float rangedRange;

    [Header("Refs")]
    public NavMeshAgent agent;
    public Animator animator;
    public Transform player;
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private Renderer enemyRenderer;

    [Header("UI Destierro")]
    [SerializeField] private GameObject destierroUI;
    [SerializeField] private float uiActivationDistance = 5f;

    private Color originalColor;

    public bool stuned;

    protected EnemyState _current;
    private bool isJumping;
    public readonly ChaseState chase = new ChaseState();
    public readonly AttackState attack = new AttackState();

    protected virtual void Awake() => vidaActual = vidaMax;

    protected virtual void Start()
    {
        originalColor = enemyRenderer.material.color;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (destierroUI != null)
            destierroUI.SetActive(false);

        SwitchState(chase);
    }


    protected virtual void Update()
    {
        if (destierroUI != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            bool shouldShowUI = stuned && distanceToPlayer <= uiActivationDistance;
            destierroUI.SetActive(shouldShowUI);

            if (shouldShowUI)
            {
                destierroUI.transform.LookAt(player);
                destierroUI.transform.Rotate(0, 180, 0); 
            }
        }

        if (agent.isOnOffMeshLink && !isJumping)
        {
            StartCoroutine(HandleJump(agent.currentOffMeshLinkData));
        }

        _current?.Tick(this);
        if (this is HybridEnemy hybrid)
        {
            hybrid.TickAttackTimer(Time.deltaTime);

            if (!hybrid.HasAttacked && hybrid.TimeSinceLastAttack >= hybrid.ModeTimeout)
            {
                Debug.Log("No ha atacado en 5 segundos, cambiando modo...");
                hybrid.ForceRecalculateAttackMode();
            }
        }

    }

    public void SwitchState(EnemyState next)
    {
        _current?.Exit(this);
        _current = next;
        _current?.Enter(this);
    }

    public virtual void TakeDamage(float amount)
    {
        ShowDamageText(amount);

        vidaActual -= amount;
        if (vidaActual <= 0f)
        {
            StartCoroutine(stunCoroutine());
            vidaActual = vidaMax / 2;
        }
    }

    public virtual void TakeDamage(float amount, Vector3 _) => TakeDamage(amount);

    private void ShowDamageText(float amount)
    {
        if (floatingTextPrefab == null) return;

        GameObject go = Instantiate(floatingTextPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
        FloatingDamageText text = go.GetComponent<FloatingDamageText>();
        text.SetDamage(amount);
    }

    protected virtual void Morir()
    {
        EnemyEvents.NotificarMuerte(gameObject);
        LevelManager.Instance.OnEnemyDefeated();
        Destroy(gameObject);
    }

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
        enemyRenderer.material.color = Color.green;
        Debug.Log("Esta aturdido el enemigo");

        yield return new WaitForSeconds(5f);

        agent.speed = 2.5f;
        stuned = false;
        enemyRenderer.material.color = originalColor;
        Debug.Log("Ya no esta aturdido el enemigo");
    }

    public virtual void OnCollisionEnter(Collision col)
    {
        if (stuned && col.gameObject.CompareTag("Player") && Input.GetKey(KeyCode.E))
        {
            Morir();
        }
    }

    public virtual void OnCollisionStay(Collision col)
    {
        if (stuned && col.gameObject.CompareTag("Player") && Input.GetKey(KeyCode.E))
        {
            Morir();
        }
    }

    public abstract bool IsInAttackRange();

    public abstract float DoAttack();
}
