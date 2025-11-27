using UnityEngine;

public class Cannon : Enemy
{
    [Header("projectile")]
    [SerializeField] GameObject bombPrefab;
    [SerializeField] Transform Shootpoint;

    [Header("Fire setting")]
    [SerializeField] float fireRate = 2f;
    [SerializeField] float speed = 8f;
    [SerializeField] Vector2 direction = Vector2.right;
    [SerializeField] float spread = 0f;
    [SerializeField] int bombDamage = 20;

    float nextFireTime = 0f;

    protected override void Awake()
    {
        base.Awake();
        DamageHit = 0;
    }

    protected override void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        Behavior();
    }

    public override void Behavior()
    {
       if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Fire()
    {
        if (bombPrefab == null || Shootpoint == null)
        {
            Debug.Log("Cannon: bombPrefab or Shootpoint is null!");
            return;
        }
        GameObject go = Instantiate(bombPrefab, Shootpoint.position, Quaternion.identity);
        Bomb bomb = go.GetComponent<Bomb>();
        if (bomb == null)
        {
            bomb = go.AddComponent<Bomb>();
        }
        Vector2 dir = direction.normalized;
        if (spread != 0f)
        {
            float half = spread * 0.5f;
            float rand = Random.Range(-half, half);
            dir = Quaternion.Euler(0, 0, rand) * dir;
            dir.Normalize();
        }

        Vector2 vel = dir * speed;
        bomb.Init(vel, bombDamage);
    }

    protected override void Die()
    {
        base.Die();
    }
}
