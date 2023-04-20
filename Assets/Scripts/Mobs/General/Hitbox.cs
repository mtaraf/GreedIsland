using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public GameObject player;
    public float damage = 1;

    public float damageMultiplier = 1;
    public float knockBackAmount = 1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHurtbox playerHurtbox = collision.GetComponent<PlayerHurtbox>();
        if (playerHurtbox != null && player != null)
        {
            playerHurtbox.TakeDamage(damage * damageMultiplier, PlayerDirection(player.transform.position));
            Debug.Log("player hit by mob attack");
        }
    }

    // Returns 1 if player is to the left, 0 if player is to the right
    public int PlayerDirection(Vector3 playerPosition)
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
