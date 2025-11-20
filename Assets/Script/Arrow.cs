using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 10;
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

       
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (dir < 0f ? -1f : 1f);
        transform.localScale = s;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var c = other.GetComponent<Character>(); 
            if (c != null)
            {
                c.TakeDamage(damage);
            }
            Destroy(gameObject);
            return;
        }
    }
}
