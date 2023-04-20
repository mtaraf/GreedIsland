using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSlimeHitbox : Hitbox
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHurtbox playerHurtbox = collision.GetComponent<PlayerHurtbox>();
        if (playerHurtbox != null && player != null)
        {
            playerHurtbox.TakeDamage(damage * damageMultiplier, PlayerDirection(player.transform.position));
            playerHurtbox.TakeIceEffect();
            Debug.Log("player hit by mob attack");
        }
    }
}
