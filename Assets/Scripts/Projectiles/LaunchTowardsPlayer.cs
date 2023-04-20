using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTowardsPlayer : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;

    [SerializeField] private float speed;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        Vector3 playerPos = player.transform.position;
        RotateTowardsPlayerPosition(playerPos);
        AddForceTowardsPlayer(playerPos);
    }

    private void RotateTowardsPlayerPosition(Vector3 playerPos)
    {
        float angle = Mathf.Atan2(playerPos.y - transform.position.y, playerPos.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void AddForceTowardsPlayer(Vector3 playerPos)
    {
        Vector2 forcePos = playerPos - transform.position;
        rb.AddForce(forcePos * Time.deltaTime * speed, ForceMode2D.Impulse);
    }
}
