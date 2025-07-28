using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

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
    [SerializeField] private CharacterDeathHandler deathHandler;

    [Header("UI Destierro")]
    //private TextMeshProUGUI destierroUI;
    [SerializeField] private float uiActivationDistance = 5f;

    [Header("Sounds")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip stunnedSound;
    [SerializeField] private AudioClip destierroSound;
    [SerializeField] private AudioSource audioSrc;

    private Color originalColor;

    public bool stuned;
    private bool isDead = false;


    public EnemyState _current;
    private bool isJumping;
    public readonly ChaseState chase = new ChaseState();
    public virtual AttackState attack { get; protected set; } = new AttackState();
    private bool playerInsideTrigger = false;

    protected virtual void Awake() => vidaActual = vidaMax;

    protected virtual void Start()
    {
        originalColor = enemyRenderer.material.color;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        /*if (destierroUI != null)
            destierroUI.gameObject.SetActive(false);*/

        SwitchState(chase);

        Invoke(nameof(RegisterSelf), 0.1f);
    }

    private void RegisterSelf()
    {
        CombatManager.Instance?.RegisterEnemy(this);
    }

    protected virtual void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

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
        if (stuned || isDead)
        {
            return;
        }

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

    protected virtual IEnumerator MorirCoroutine()
    {
        isDead = true;
        stuned = true;

        EnemyEvents.NotificarMuerte(gameObject);
        CombatManager.Instance?.UnregisterEnemy(this);

        if (deathSound)
            audioSrc?.PlayOneShot(deathSound);

        if (destierroSound)
            audioSrc?.PlayOneShot(destierroSound);

        if (deathHandler != null)
        {
            deathHandler.Die();
        }
        else
        {
            Destroy(gameObject);
        }

        yield return new WaitForSeconds(deathSound != null ? deathSound.length + 0.5f : 0.5f);
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
        float originalSpeed = agent.speed;
        agent.speed = 0;
        stuned = true;

        enemyRenderer.material.color = Color.green;
        if (stunnedSound) audioSrc?.PlayOneShot(stunnedSound);

        animator.ResetTrigger("Attacking");
        animator.SetBool("Stunned", true);
        animator.SetBool("Chasing", false);

        yield return new WaitForSeconds(5f);

        if (!isDead) 
        {
            agent.speed = originalSpeed;
            stuned = false;

            enemyRenderer.material.color = originalColor;
            animator.SetBool("Stunned", false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInsideTrigger = true;

        if (stuned)
        {
            UIManager.Instance.ExileEnemy();

            if (Input.GetKey(KeyCode.E))
            {
                //LevelManager.Instance.TriggerShake();
                StartCoroutine(MorirCoroutine());
            }
        }
        else
        {
            UIManager.Instance.HideExileEnemy();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (stuned)
        {
            UIManager.Instance.ExileEnemy();

            if (Input.GetKey(KeyCode.E))
            {
                //LevelManager.Instance.TriggerShake();
                StartCoroutine(MorirCoroutine());
            }
        }
        else
        {
            UIManager.Instance.HideExileEnemy();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInsideTrigger = false;
        UIManager.Instance.HideExileEnemy();
    }

    public abstract bool IsInAttackRange();

    public abstract float DoAttack();

    public virtual bool IsInCombatState()
    {
        return _current != null && (_current is ChaseState || _current is AttackState);
    }
}
