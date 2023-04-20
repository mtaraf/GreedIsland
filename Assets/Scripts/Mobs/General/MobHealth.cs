using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
