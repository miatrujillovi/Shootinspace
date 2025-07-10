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

    private Rigidbody rb;
    private Vector3 movement;
    private Vector3 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sprint = false; doubleJump = false;
    }

    //Player Movement Functions
    [System.Obsolete]
    private void FixedUpdate()
    {
        if (sprint)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                playerSpeed += 5;
            }
            else
            {
                playerSpeed -= 5;
            }
        }
        MovePlayer();
    }

    [System.Obsolete]
    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement = transform.right * horizontal + transform.forward * vertical;
        movement.Normalize();

        velocity = new Vector3(movement.x * playerSpeed, rb.velocity.y, movement.z * playerSpeed);
        rb.velocity = velocity;
    }

    //Player Jumping Functions
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (doubleJump)
        {
            //Double Jump Here
        }
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
