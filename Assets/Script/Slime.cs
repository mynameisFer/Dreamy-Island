using UnityEngine;

public class Slime : Enemy
{
    [Header("Movement")]
    [SerializeField] Vector2 velocity;
    public Transform[] MovePoint;

    [Header("Attack")]
    public int damage = 20;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;

    [Header("Stats")]
    public int maxHealth = 150;
    private int currentHealth;

    private void Awake()
    {
        maxHealth = 150;
    }
    protected override void Start()
    {
        base.Start();
        velocity = new Vector2(-1f, 0f);
        currentHealth = maxHealth;
    }



    public override void Behavior()
    {
        if (MovePoint == null || MovePoint.Length < 2) return;

        float leftX = Mathf.Min(MovePoint[0].position.x, MovePoint[1].position.x);
        float rightX = Mathf.Max(MovePoint[0].position.x, MovePoint[1].position.x);

        rb.position += velocity * Time.fixedDeltaTime;

        if (velocity.x < 0 && rb.position.x <= leftX)
        {
            SetDirection(1);  
        }
        else if (velocity.x > 0 && rb.position.x >= rightX)
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
        // ไม่สน Tag แล้ว ดูจากว่ามี Player component ไหม
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

    protected virtual void Die()
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
