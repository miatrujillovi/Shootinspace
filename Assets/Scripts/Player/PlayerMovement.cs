using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Variables for Normal Movement")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpForce;
    [Space]
    [Header("Variables for Boost Movement")]
    [SerializeField] private bool sprint;
    [SerializeField] private bool doubleJump;
    [SerializeField] private float boostedPlayerSpeed;
    [SerializeField] private float boostedJumpForce;

    private Rigidbody rb;
    private Vector3 movement;
    private Vector3 velocity;
    private bool canDoubleJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sprint = false; 
        doubleJump = false;
    }

    //Player Movement Functions
    private void FixedUpdate()
    {
        if (sprint)
        {
            playerSpeed = boostedPlayerSpeed;
        }
        MovePlayer();
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movement = transform.right * horizontal + transform.forward * vertical;
        movement.Normalize();

        velocity = new Vector3(movement.x * playerSpeed, rb.linearVelocity.y, movement.z * playerSpeed);
        rb.linearVelocity = velocity;
    }

    //Player Jumping Functions
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                Jump(jumpForce);
                canDoubleJump = true;
            }
            else if (doubleJump && canDoubleJump)
            {
                Jump(boostedJumpForce);
                canDoubleJump = false;
            }
        }
    }

    private void Jump(float force)
    {
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f))
        {
            return hit.collider.CompareTag("Ground");
        }
        return false;
    }

    //Boost Activation Functions
    public void ActivateSprint()
    {
        sprint = true;
    }

    public void DeactivateSprint()
    {
        sprint = false;
    }

    public void ActivateDoubleJump()
    {
        doubleJump = true;
    }

    public void DeactivateDoubleJump()
    {
        doubleJump = false;
    }
}
