using UnityEngine;
using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpForce;

    [Header("Dash Variables")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    [SerializeField] private GameObject dashIcon;
    [SerializeField] private GameObject dashFiller;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private float gravity = -9.81f;
    private float fallMultiplier = 2.5f;

    private bool isDashing = false;
    private bool canDash = true;
    private Image dashIconImage;
    private Image dashFillerImage;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        dashIconImage = dashIcon.GetComponent<Image>();
        dashFillerImage = dashFiller.GetComponent<Image>();

    }

    private void Update()
    {
        isGrounded = characterController.isGrounded;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = (transform.right * horizontal + transform.forward * vertical).normalized;

        if (!isDashing)
        {
            characterController.Move(moveDirection * playerSpeed * Time.deltaTime);
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        if (velocity.y < 0)
        {
            velocity.y += gravity * fallMultiplier * Time.deltaTime;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        characterController.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && moveDirection.magnitude > 0f)
        {
            StartCoroutine(PerformDash(moveDirection));
        }
    }

    private IEnumerator PerformDash(Vector3 direction)
    {
        isDashing = true;
        canDash = false;

        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            characterController.Move(direction * dashForce * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    /*private IEnumerator ShowDashCooldown()
    {
        dashFillerImage.
    }*/
}
