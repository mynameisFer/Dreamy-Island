using UnityEngine;

public class Coin : Item
{
    public override void Use(Player player)
    {
        if (player != null)
        {
            player.AddCoin(ItemValue);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var p = other.GetComponent<Player>();
        if (p != null)
            PickUp(p);
    }
}
