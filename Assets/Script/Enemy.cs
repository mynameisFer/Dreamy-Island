using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : Character
{
    public int maxHP = 3;
    public int currentHP;
    public int attackDamage = 1;

    protected virtual void Start()
    {
        currentHP = maxHP;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}


