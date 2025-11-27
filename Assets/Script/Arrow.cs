using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 20;
    public float lifeTime = 3f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(float dir)
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);
        }

        // เปลี่ยนทิศทางลูกศรตามการหันตัวละคร
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (dir < 0f ? -1f : 1f);
        transform.localScale = s;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ⭐ สำคัญ: Slime ต้องมี Tag = "Enemy" ไม่งั้นธนูจะไม่ทำงาน
        if (other.CompareTag("Enemy"))
        {
            // ⭐ สำคัญ: Slime ต้องมี Script ที่สืบทอดจาก Character
            // เช่น class Slime : Enemy หรือ class Slime : Character
            var c = other.GetComponent<Character>();
            if (c != null)
            {
                // ⭐ ตรงนี้เรียกลดเลือดศัตรู
                c.TakeDamage(damage);
            }

            // ทำลายลูกศรหลังชน
            Destroy(gameObject);
            return;
        }
    }
}

