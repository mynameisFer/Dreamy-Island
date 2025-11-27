using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class Character : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] protected int startHealth = 3;
    [SerializeField] protected bool unattackableAfterHit = true;
    [SerializeField] protected float unattackableTime = 0.1f;

    [Header("References")]
    [SerializeField] protected Animator anim;
    [SerializeField] protected MonoBehaviour healthBar;

    protected int currentHealth; 
    protected Rigidbody2D rb; 

    protected bool isUnattackableAfterHit = false;
    protected float isUnattackableTime = 0f;

    public int Health => currentHealth;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        Initialize(startHealth);
    }

    public virtual void Initialize(int hp)
    {
        startHealth = hp;
        currentHealth = Mathf.Max(0, hp);  
    }


    public virtual void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        currentHealth -= amount;

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateHearts(currentHealth);

        if (currentHealth <= 0)
        {
            GameManager.instance?.LoseLife();
            Die();
        }
    }

    void Update()
    {
        if (isUnattackableAfterHit)
        {
            isUnattackableTime -= Time.deltaTime;
            if (isUnattackableTime <= 0f)
                isUnattackableAfterHit = false;
        }

    }


    public virtual bool IsDead()
    {
        return currentHealth <= 0;
    }

    
    protected virtual void Die()
    {
        if (anim != null)
            anim.SetTrigger("Die");

        Destroy(gameObject);
    }

    public virtual void Heal(int amount)
    {
        if (amount <= 0) return;

        currentHealth += amount;
    }
}
