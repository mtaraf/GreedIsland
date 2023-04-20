using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHitbox : DestroyAfterAnimation
{
    private GameObject player;
    private BoxCollider2D box;
    public float damage = 1;

    public float damageMultiplier = 1;
    public float knockBackAmount = 1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        box = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHurtbox"))
        {
            PlayerHurtbox playerHurtbox = collision.GetComponent<PlayerHurtbox>();
            playerHurtbox.TakeDamage(damage * damageMultiplier, PlayerDirection(player.transform.position));
            box.enabled = false;
        }
    }

    // Returns 1 if player is to the left, 0 if player is to the right
    private int PlayerDirection(Vector3 playerPosition)
    {
        if (playerPosition.x > transform.position.x)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
}
