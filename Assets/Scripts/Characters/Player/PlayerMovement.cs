using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool facingRight = true;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private LayerMask groundLayer; // Layer để kiểm tra mặt đất
    [SerializeField] private float groundCheckRadius = 0.2f; // Bán kính kiểm tra mặt đất
    [SerializeField] private Transform groundCheck; // Điểm kiểm tra mặt đất

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing!");
        }
    }

    private void Update()
    {
        // Kiểm tra mặt đất
        CheckGrounded();

        // Lấy input từ người chơi
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Xử lý nhảy
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // Kiểm tra và xoay nhân vật
        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveVelocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.velocity = moveVelocity;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    //private void OnDrawGizmos()
    //{
    //    if (groundCheck != null)
    //    {
    //        // Vẽ sphere để debug ground check
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    //    }
    //}
}