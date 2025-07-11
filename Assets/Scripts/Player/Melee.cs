using UnityEngine;

public class Melee : MonoBehaviour
{
    [Header("Melee Variables")]
    [SerializeField] private GameObject slashEffect;
    [SerializeField] private float meleeAttackDistance;
    [SerializeField] private float meleeCooldown;
    [SerializeField] private Transform attackOrigin;

    private ParticleSystem pS;
    private float nextMeleeTime = 0f;

    private void Awake()
    {
        pS = slashEffect.GetComponent<ParticleSystem>();
        Vector3 effectPosition = attackOrigin.position + attackOrigin.forward * meleeAttackDistance;
        slashEffect.transform.position = effectPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && Time.time >= nextMeleeTime)
        {
            MeleeAttack();
            nextMeleeTime = Time.time + meleeCooldown;
        }
    }

    private void MeleeAttack()
    {
        slashEffect.SetActive(true);
        pS.Play();

        RaycastHit hit;
        if (Physics.Raycast(attackOrigin.position, attackOrigin.forward, out hit, meleeAttackDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Melee hit an enemy.");
                slashEffect.SetActive(false);
            }
        }
    }
}
