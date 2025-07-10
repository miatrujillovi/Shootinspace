using UnityEngine;

public class Melee : MonoBehaviour
{
    [Header("Melee Variables")]
    [SerializeField] private GameObject slashEffect;
    [SerializeField] private float meleeAttackDistance;
    [SerializeField] private Transform attackOrigin;

    private ParticleSystem pS;
    private Vector3 effectPosition;

    private void Awake()
    {
        pS = slashEffect.GetComponent<ParticleSystem>();
        Vector3 effectPosition = attackOrigin.position + attackOrigin.forward * meleeAttackDistance;
        slashEffect.transform.position = effectPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            MeleeAttack();
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
