using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Melee : MonoBehaviour
{
    [SerializeField] private Transform meleeWeapon;

    //private Vector3 defaultPosition = new(-0.411f, -0.272f, 0.248f);
    private float defaultPosition = 0.248f;
    private float attackPosition = 0.687f;
    //private Vector3 attackPosition = new(-0.411f, -0.272f, 0.687f);

    private void Awake()
    {
        //meleeWeapon.transform.position = defaultPosition;
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
        meleeWeapon.DOMoveZ(defaultPosition, 0.5f);
        //If the collider hit something with the tag Enemy it damages it...
        StartCoroutine(RetractMelee());
    }

    IEnumerator RetractMelee()
    {
        yield return new WaitForSeconds(0.5f);
        meleeWeapon.DOMoveZ(attackPosition, 0.5f);
    }
}
