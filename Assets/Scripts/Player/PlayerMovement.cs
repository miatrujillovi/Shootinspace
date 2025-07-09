using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Variables for Movement")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpForce;

    private Rigidbody rb;
    private Vector3 movement;
    private Vector3 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }
    }

    private void Jump()
    {
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
}
