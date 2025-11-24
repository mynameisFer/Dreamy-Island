using UnityEngine;

public class Slime : Enemy
{
    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Attack")]
    public int damage = 100;   // ดาเมจของ Slime
    public float attackCooldown = 1f; // เวลาระหว่างโจมตี
    private float lastAttackTime = 0f;

    private Rigidbody2D rb;
    private Transform player;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    private void Update()
    {
        if (player == null || rb == null) return;

        // เดินไล่ Player
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            TryAttack(collision.collider.GetComponent<Player>());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            TryAttack(collision.collider.GetComponent<Player>());
        }
    }

    private void TryAttack(Player player)
    {
        if (player == null) return;

        // กันสไปค์ดาเมจถี่ๆ
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            player.TakeDamage(damage);
            lastAttackTime = Time.time;
        }
    }
}
