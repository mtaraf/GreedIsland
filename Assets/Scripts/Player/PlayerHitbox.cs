using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    private PlayerController playerController;
    private float damage;

    public float damageMultiplier = 1;
    public float knockBackAmount = 1;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        damage = playerController.playerDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hurtbox enemyHurtbox = collision.GetComponent<Hurtbox>();
        if (enemyHurtbox != null)
        {
            enemyHurtbox.TakeDamage(damage * damageMultiplier, knockBackAmount);
            Debug.Log("basic attack hit enemy");
        }
    }
}
