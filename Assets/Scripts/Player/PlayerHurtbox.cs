using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    private BoxCollider2D box;
    private Rigidbody2D rb;
    private Vector2 knockbackVector = new Vector2(10, 2);
    private PlayerController controller;
    private SpriteRenderer spriteRenderer;

    public float knockbackMultiplier = 1;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        controller = GetComponentInParent<PlayerController>();
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }


    public void TakeDamage(float damage, int knockbackDirection)
    {
        controller.TakeKnockback(knockbackDirection);
        controller.TakeDamage(damage);
    }

    public void TakeIceEffect()
    {
        Debug.Log("ice effect");
        controller.TriggerIceEffect();
        Debug.Log("ice effect done");
    }

    public void OneHitKill()
    {
        controller.OneHitKill();
    }
}
