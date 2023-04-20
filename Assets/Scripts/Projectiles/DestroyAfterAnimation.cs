using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    [SerializeField] private float seconds;

    void Awake()
    {
        StartCoroutine(DestroyAfterAnimating(seconds));
    }

    IEnumerator DestroyAfterAnimating(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
