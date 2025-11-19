using UnityEngine;

public class Player : Character
{
    [Header("Movement")]
    [field: SerializeField] public float speed = 4f;
    [field: SerializeField] public float jumpForce = 7f;
    [field: SerializeField] int maxJump = 2;

    [Header("Ground Check")]
    public Transform groundCheck;         
    public float groundCheckRadius = 0.08f;
    public LayerMask groundLayer;          

    [Header("Other")]
    public SpriteRenderer spriteRenderer;  
    public float fallDeathY = -20f;

    int jumpCount;
    bool isGrounded;

    protected override void Awake()
    {
        base.Awake();
        if(spriteRenderer == null)
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        jumpCount = maxJump;
    }


    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (spriteRenderer != null && h != 0)
            spriteRenderer.flipX = (h < 0);

        
        if (Input.GetButtonDown("Jump"))
            TryJump();

        
        if (transform.position.y <= fallDeathY)
        {
            
            GameManager.instance?.LoseLife();
        }
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        Vector2 vel = rb.linearVelocity;
        vel.x = h * speed;
        rb.linearVelocity = vel;

       
        bool wasGrounded = isGrounded;
        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && !wasGrounded)
        {
            jumpCount = maxJump;
        }
    }

    protected void TryJump()
    {
        if (isGrounded || jumpCount > 0)
        {
            Vector2 vel = rb.linearVelocity;
            vel.y = 0f;
            rb.linearVelocity = vel;

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount--;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            GameManager.instance?.LoseLife();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    protected override void Die()
    {
        GameManager.instance?.LoseLife();
    }

}
