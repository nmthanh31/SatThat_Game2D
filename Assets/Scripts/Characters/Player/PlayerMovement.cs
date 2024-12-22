using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool facingRight = true;
    [SerializeField] Animator animator;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private LayerMask groundLayer; // Layer để kiểm tra mặt đất
    [SerializeField] private float groundCheckRadius = 0.2f; // Bán kính kiểm tra mặt đất
    [SerializeField] private Transform groundCheck; // Điểm kiểm tra mặt đất
    [SerializeField] private LayerMask boxLayer; // Layer để kiểm tra Hòm
    [SerializeField] private float boxCheckRadius = 0.2f; // Bán kính kiểm tra mặt đất
    [SerializeField] private Transform boxCheck; // Điểm kiểm tra mặt đất


    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    private bool isBox;

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

        // Kiểm tra thùng
        CheckBoxed();

        // Lấy input từ người chơi
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Xử lý nhảy

        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
            animator.SetBool("isJumping", true);
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

        animator.SetFloat("isMoving", Mathf.Abs(horizontalInput));

        
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer | boxLayer);
    }

    private void CheckBoxed()
    {
        isBox = Physics2D.OverlapCircle(boxCheck.position, boxCheckRadius, boxLayer);
        Collider2D boxCollider = Physics2D.OverlapCircle(boxCheck.position, boxCheckRadius, boxLayer);

        if (boxCollider != null)
        {
            isBox = true;

            // Lấy Rigidbody2D của thùng
            Rigidbody2D boxRb = boxCollider.GetComponent<Rigidbody2D>();

            if (boxRb != null)
            {
                // Áp dụng lực đẩy thùng
                float pushForce = horizontalInput * moveSpeed * 0.5f; // Điều chỉnh lực đẩy tùy ý
                boxRb.velocity = new Vector2(pushForce, boxRb.velocity.y);
            }
        }
        else
        {
            isBox = false;
        }
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