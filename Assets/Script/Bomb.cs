using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] int damage = 20;
    [SerializeField] float lifeTime = 6f;
    [SerializeField] bool useTrigger = true;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Collider2D col = GetComponent<Collider2D>();
        if(col != null)
        {
            col.isTrigger = useTrigger;
        }
    }

    public void Init(Vector2 initialVelocity,int damageOverride = -1)
    {
        if(rb != null)
        rb.linearVelocity = initialVelocity;
        if (damageOverride > 0)
            damage = damageOverride;

        if (lifeTime > 0f)
            Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if(player != null)
        {
            player.TakeDamage(damage);
            Explode();
            return;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (useTrigger) return;
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage); 
        }
        Explode();
    }


    void Explode()
    {
        Destroy(gameObject);
    }
}
