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

    protected override void Start()
    {
        base.Start();
        velocity = new Vector2(-1f, 0f);
    }

    public override void Behavior()
    {
        if (MovePoint == null || MovePoint.Length < 2) return;

        // หาค่า X ซ้ายสุด – ขวาสุด จาก 2 จุด ไม่สนว่าตัวไหนคือ A หรือ B
        float leftX = Mathf.Min(MovePoint[0].position.x, MovePoint[1].position.x);
        float rightX = Mathf.Max(MovePoint[0].position.x, MovePoint[1].position.x);

        // ให้ Slime ขยับไปตาม velocity
        rb.position += velocity * Time.fixedDeltaTime;

        // ถึงขอบซ้าย → กลับขวา
        if (velocity.x < 0 && rb.position.x <= leftX)
        {
            SetDirection(1);    // เดินไปขวา
        }
        // ถึงขอบขวา → กลับซ้าย
        else if (velocity.x > 0 && rb.position.x >= rightX)
        {
            SetDirection(-1);   // เดินไปซ้าย
        }
    }

    private void SetDirection(int dir)   // dir = -1 ซ้าย, 1 ขวา
    {
        // เปลี่ยนความเร็วในแกน X ตามทิศ
        velocity.x = Mathf.Abs(velocity.x) * dir;

        // พลิกสเกลให้หันหน้าไปทางเดียวกับทิศเดิน
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
        if (collision.collider.CompareTag("Player"))
        {
            Player player = collision.collider.GetComponent<Player>();
            if (player != null)
            {
                // ทำดาเมจเท่ากับเลือดที่เหลือ
                player.TakeDamage(player.Health);

                // หรือถ้าอยากให้ใช้ระบบชีวิตใน GameManager ด้วย
                GameManager.instance?.LoseLife();
            }
        }
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
