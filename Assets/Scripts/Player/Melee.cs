using UnityEngine;
using System.Collections;

public class Melee : MonoBehaviour
{
    [Header("Melee")]
    [SerializeField] private GameObject slashEffect;
    [SerializeField] private float meleeAttackDistance = 3f;
    [SerializeField] private float meleeCooldown = 0.5f;
    [SerializeField] private Transform attackOrigin;

    [Header("Sounds")]
    [SerializeField] private AudioClip parryStartSFX;
    [SerializeField] private AudioClip parryPerfectSFX;
    [SerializeField] private AudioClip slashSFX;
    [SerializeField] private AudioSource audioSrc;

    [Header("Parry")]
    [SerializeField] private bool canParry = true;
    [SerializeField] private float parryActiveFrames = 0.12f;   
    [SerializeField] private float parryRadius = 3f;              
    [SerializeField] private ParticleSystem parrySlashVFX;    

    private ParticleSystem pS;
    private float nextMeleeTime = 0f;

    void Awake()
    {
        pS = slashEffect.GetComponent<ParticleSystem>();

        Vector3 effectPosition = attackOrigin.position + attackOrigin.forward * meleeAttackDistance * 0.5f;
        slashEffect.transform.position = effectPosition;
        slashEffect.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && Time.time >= nextMeleeTime)
        {
            StartCoroutine(MeleeRoutine());
            if (slashSFX) audioSrc?.PlayOneShot(slashSFX);
            nextMeleeTime = Time.time + meleeCooldown;
        }
    }

    private IEnumerator MeleeRoutine()
    {
        slashEffect.SetActive(true);
        pS.Play();
        if (parryStartSFX) AudioSource.PlayClipAtPoint(parryStartSFX, attackOrigin.position);
        if (parrySlashVFX) Instantiate(parrySlashVFX, attackOrigin.position, attackOrigin.rotation);

        if (canParry)
        {
            var field = new GameObject("ParryField");
            field.transform.position = attackOrigin.position;

            field.AddComponent<ParryField>()
                 .Init(gameObject,
                       Camera.main.transform,   
                       parryActiveFrames,
                       parryActiveFrames,
                       parryRadius);
        }

        if (Physics.Raycast(attackOrigin.position,
                            attackOrigin.forward,
                            out RaycastHit hit,
                            meleeAttackDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Melee hit an enemy.");
            }

            if (hit.collider.TryGetComponent(out IParryable parryable))
            {
                bool perfect = Time.time - Time.time <= parryable.ParryWindow;
                Vector3 dir = (hit.point - attackOrigin.position).normalized;
                parryable.OnParried(dir, gameObject, perfect);
                parryable.PlayParryFeedback(perfect);
            }
        }

        yield return new WaitForSeconds(pS.main.duration * 0.9f);
        slashEffect.SetActive(false);
    }
}
