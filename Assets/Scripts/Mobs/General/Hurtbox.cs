using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public MobHealth mobHealth;
    private BoxCollider2D box;
    private Rigidbody2D rb;
    private Vector2 knockbackVector = new Vector2(1, 2);
    private GameObject player;

    public float knockbackMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mobHealth = GetComponentInParent<MobHealth>();
        rb = GetComponentInParent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    public void TakeDamage(float damage, float knockback)
    {
        mobHealth.TakeDamage(damage);
        TakeKnockback(knockback);
        //StartCoroutine(InvisibilityFrames());
    }

    private void TakeKnockback(float knockback)
    {
        int knockbackDirection = PlayerDirection(player.transform.position);
        Debug.Log(knockbackDirection);
        if (knockbackDirection == 1)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(knockbackVector * knockback * knockbackMultiplier, ForceMode2D.Impulse);
        }
        else
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(-1, 2) * knockback * knockbackMultiplier, ForceMode2D.Impulse);
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

    IEnumerator InvisibilityFrames()
    {
        box.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        box.gameObject.SetActive(true);
    }
}
