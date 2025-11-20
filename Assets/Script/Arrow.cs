using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 10;
    public float lifeTime = 3f;

    private Rigidbody2D rb;
    private Vector3 originalScale;
  

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    public void Init(float dir)
    {
        Debug.Log($"Arrow.Init dir = {dir}");

        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (dir < 0f ? -1f : 1f);
        transform.localScale = s;

        if(rb != null)
        {
            rb.linearVelocity = new Vector2(dir * speed, 0f);
        }
        else
        {
            Debug.Log($"Arrow: rb is null");
        }

        Destroy(gameObject,lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Character c = other.GetComponent<Character>();
        if (c != null)
        {
            c.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
