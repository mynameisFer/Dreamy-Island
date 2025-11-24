using UnityEngine;

public class SlimePatrol : Enemy
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform[] movePoints;   // จุด A และ B
    private int currentPoint = 0;

    private Rigidbody2D rb;

    protected override void Start()
    {
        base.Start(); // เรียกจาก Enemy ด้วย
        rb = GetComponent<Rigidbody2D>();

        // ตั้งค่าเลือด ดาเมจ หรืออะไรก็ตาม
        attackDamage = 10;
    }

    private void FixedUpdate()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (movePoints.Length < 2) return;

        // จุดเป้าหมายที่กำลังจะเดินไป
        Transform target = movePoints[currentPoint];

        // ทิศทางจากตำแหน่งปัจจุบัน → จุดเป้าหมาย
        Vector2 direction = (target.position - transform.position).normalized;

        // เดินไป
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

        // สลับเป้าหมายเมื่อถึงจุด
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentPoint++;
            if (currentPoint >= movePoints.Length)
                currentPoint = 0;

            Flip(direction.x);
        }
    }

    private void Flip(float dirX)
    {
        if (dirX != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(dirX) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
}
