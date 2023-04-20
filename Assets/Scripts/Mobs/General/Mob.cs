using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    public float damage;
    public float speed;
    public float xDetectionValue;
    public float yDetectionValue;
    public bool isFacingRight;
    public bool isDead;

    public MobHealth mobHealth;
    private GameObject player;

    private void Awake()
    {
        isFacingRight = true;
        xDetectionValue = 7.0f;
        yDetectionValue = 2.0f;
        player = GameObject.FindGameObjectWithTag("Player");
        mobHealth = GetComponent<MobHealth>();
        isDead = false;
    }

    public virtual void TakeDamge(float damageTaken)
    {
        mobHealth.TakeDamage(damageTaken);
    }

    public virtual void Heal(float healAmount)
    {
        mobHealth.currentHealth += healAmount;
    }

    public virtual Tuple<bool,Vector3> DetectPlayer(float detectionWidth)
    {
        Vector3 playerPostion = player.transform.position;
        Vector3 currentPosition = transform.position;

        if (playerPostion.y < currentPosition.y + yDetectionValue && playerPostion.y > currentPosition.y - yDetectionValue) // Check if player is within y radius
        {
            if (playerPostion.x < currentPosition.x + detectionWidth && playerPostion.x > currentPosition.x - detectionWidth) // Check if player is within x radius
            {
                return new Tuple<bool, Vector3>(true, playerPostion); // return true and player position
            }
        }
        return new Tuple<bool,Vector3>(false, currentPosition); // returns false and mob position
    }
}
