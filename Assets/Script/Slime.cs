using UnityEngine;

public class Slime : Enemy
{
    [Header("Movement")]
    [SerializeField] Vector2 velocity;
    public Transform[] MovePoint;
    private float leftX, rightX;

    [Header("Attack")]
    public int damage = 20;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;

    [Header("Stats")]
    public int maxHealth = 100;
    private int currentHealth;

   
    private Rigidbody2D rb;

    private void Awake()
    {
      
        if (maxHealth <= 0) maxHealth = 150;
        currentHealth = maxHealth;

        
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();

       
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        // default velocity
        if (Mathf.Approximately(velocity.sqrMagnitude, 0f))
            velocity = new Vector2(-1f, 0f);

        
        if (MovePoint == null || MovePoint.Length < 2)
        {
            Debug.LogWarning("Slime: MovePoint not set or < 2 — will not patrol.");
            return;
        }

        
        leftX = Mathf.Min(MovePoint[0].position.x, MovePoint[1].position.x);
        rightX = Mathf.Max(MovePoint[0].position.x, MovePoint[1].position.x);
    }

    public override void Behavior()
    {
       
        if (rb == null) return;
        if (MovePoint == null || MovePoint.Length < 2) return;

        
        rb.position += velocity * Time.fixedDeltaTime;

       
        float posX = rb.position.x;

        if (posX <= leftX)
        {
            SetDirection(1);  
        }
        else if (posX >= rightX)
        {
            SetDirection(-1); 
        }
    }

    private void SetDirection(int dir)
    {
        velocity.x = Mathf.Abs(velocity.x) * dir;
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * dir;
        transform.localScale = s;
    }

    private void FixedUpdate()
    {
        Behavior();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            KillPlayer(player);
        }
    }

    private void KillPlayer(Player player)
    {
        if (player == null) return;

        player.TakeDamage(player.Health);
        GameManager.instance?.LoseLife();
        Debug.Log("Slime hit player -> player should die");
    }

    public override void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    
    protected override void Die()
    {
        Debug.Log("Slime die");
        Destroy(gameObject);
    }

    private void TryAttack(Player player)
    {
        if (player == null) return;

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            player.TakeDamage(damage);
            lastAttackTime = Time.time;
        }
    }

    private void Flip()
    {
        velocity.x *= -1;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }
}
