using System.Threading.Tasks;
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

    [Header("Arrow")]
    public GameObject Arrow;
    public Transform shootPoint;

    [Header("Animator")]
    [field: SerializeField] Animator playerAnim;
    [field: SerializeField] Animator handAnim;

    new Rigidbody2D rb;
    public float shootCooldown = 0.2f;
    private float shootTimer = 0f;
    int jumpCount;
    bool isGrounded;

    protected override void Awake()
    {
        base.Awake();
        if (spriteRenderer == null)
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        rb = GetComponentInChildren<Rigidbody2D>();
        if (rb == null)
            Debug.LogWarning("Player: Rigidbody2D not found on Player. Please add Rigidbody2D in Inspector.");

        if (playerAnim == null)
            playerAnim = GetComponentInChildren<Animator>();

        if (handAnim == null)
        {
            Transform hand = transform.Find("HandBow"); 
            if (hand != null)
                handAnim = hand.GetComponent<Animator>();

            if (handAnim == null)
                Debug.LogWarning("Player: handAnim not assigned (HandBow animator not found).");
        }
    }

    protected override void Start()
    {
        base.Start();
        jumpCount = maxJump;
    }


    void Update()
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

        shootTimer += Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && shootTimer >= shootCooldown)
        {
            Spawn();
            shootTimer = 0f;
        }

        playerAnim.SetFloat("Speed", Mathf.Abs(h));
        playerAnim.SetBool("IsGround", isGrounded);
        playerAnim.SetFloat("VSpeed", rb.linearVelocity.y);
        playerAnim.SetTrigger("Shoot");
        playerAnim.SetBool("Jump", true);
        playerAnim.SetTrigger("Dead");

        if (Input.GetButtonDown("Fire1"))
        {
            playerAnim.SetTrigger("Shoot");
            handAnim.SetTrigger("Shoot");
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

    protected void Spawn()
    {
        if (Arrow == null || shootPoint == null) return;

        GameObject go = Instantiate(Arrow, shootPoint.position, Quaternion.identity);
        Arrow a = go.GetComponent<Arrow>();
        if (a == null) return;

        
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        float dir = 1f;
        if (sr != null)
            dir = sr.flipX ? -1f : 1f;
        else
            dir = transform.localScale.x < 0 ? -1f : 1f;

        a.Init(dir);
    } 


}
