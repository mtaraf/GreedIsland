using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Slime : Mob
{
    private Tuple<bool, Vector3> detectPlayer;
    private Rigidbody2D rb;
    private BoxCollider2D box;
    private Animator slimeAnimator;

    private float jumpForce;
    [SerializeField] private LayerMask jumpableGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponentInChildren<BoxCollider2D>();
        slimeAnimator = GetComponent<Animator>();


        damage = 4;
        speed = 2;
        jumpForce = 2;
        StartCoroutine(SlimeMovement());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Hop(1);
        }
    }

    // Checks for player every second, if found it will hop towards player every 1.5 seconds, if not it will hop in a random direction every 6 seconds
    IEnumerator SlimeMovement()
    {
        int slimeRandomJumpCounter = 0;
        int movementDirection = 0;
        while (!isDead)
        {
            detectPlayer = DetectPlayer(7.0f);
            if (detectPlayer.Item1)
            {
                movementDirection = GetHopDirection(detectPlayer.Item2);
                bool isOnGround = IsOnGround();
                Debug.Log(isOnGround);
                if (isOnGround)
                {
                    Hop(movementDirection);
                    yield return new WaitForSeconds(2.5f);
                }
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                if (slimeRandomJumpCounter < 6) // Hop Randomly every 6 seconds
                {
                    yield return new WaitForSeconds(1f);
                    slimeRandomJumpCounter++;
                }
                else
                {
                    bool isOnGround = IsOnGround();
                    if (isOnGround)
                    {
                        Hop(Random.Range(0, 2));
                        yield return new WaitForSeconds(1.5f);
                        slimeRandomJumpCounter = 0;
                    }
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    // Returns 1 if player is to the left, 0 if player is to the right
    private int GetHopDirection(Vector3 playerPosition)
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

    // Hop right if paramter is 0, hop left if parameter is 1
    private void Hop(int hopDirection)
    {
        if (hopDirection == 0)
        {
            if (!isFacingRight)
            {
                transform.Rotate(new Vector2(0, 180));
                isFacingRight = true;
            }
            slimeAnimator.SetTrigger("jump");
            StartCoroutine(JumpTimingRight());
        }
        else
        {
            if (isFacingRight)
            {
                transform.Rotate(new Vector2(0, 180));
                isFacingRight = false;
            }
            slimeAnimator.SetTrigger("jump");
            StartCoroutine(JumpTimingLeft());
        }
    }

    IEnumerator JumpTimingRight()
    {
        yield return new WaitForSeconds(0.5f);
        rb.AddForce(new Vector2(1, 2.5f) * jumpForce, ForceMode2D.Impulse);
    }

    IEnumerator JumpTimingLeft()
    {
        yield return new WaitForSeconds(0.5f);
        rb.AddForce(new Vector2(-1, 2.5f) * jumpForce, ForceMode2D.Impulse);
    }

    // Return a bool determining if the entity is on the ground or not
    private bool IsOnGround()
    {
        return Physics2D.BoxCast(box.bounds.min, box.bounds.size, 0f, Vector2.down, 0.2f, jumpableGround); // creates small boxcast at bottom of collider
    }
}
