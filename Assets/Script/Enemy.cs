using UnityEngine;

public class Enemy : Character
{
    public int DamageHit { get; protected set; }

    

    public virtual void Behavior()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var p = other.gameObject.GetComponent<Player>();
        if (p != null)
        {
            p.TakeDamage(DamageHit);
        }
    }
}
