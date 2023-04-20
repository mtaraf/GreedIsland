using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class NinjaSlime : Mob
{
    private Tuple<bool, Vector3> detectPlayer;
    private Rigidbody2D rb;
    private BoxCollider2D box;
    private Animator slimeAnimator;

    private float jumpForce;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private GameObject throwingStar;

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
            detectPlayer = DetectPlayer(10.0f);
            if (detectPlayer.Item1)
            {
                movementDirection = GetHopDirection(detectPlayer.Item2);
                bool isOnGround = IsOnGround();
                Debug.Log(isOnGround);
                if (isOnGround)
                {
                    int attack = Random.Range(0, 2);
                    RotateSlime(movementDirection);

                    if (attack == 0)
                    {
                        StartCoroutine(LaunchStar(detectPlayer.Item2));
                    }
                    else
                    {
                        Hop(movementDirection);
                    }
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
                        int direction = Random.Range(0, 2);
                        RotateSlime(direction);
                        Hop(direction);
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

    // Rotate right if paramter is 0, left if parameter is 1
    private void RotateSlime(int direction)
    {
        if (direction == 0)
        {
            if (!isFacingRight)
            {
                transform.Rotate(new Vector2(0, 180));
                isFacingRight = true;
            }
        }
        else
        {
            if (isFacingRight)
            {
                transform.Rotate(new Vector2(0, 180));
                isFacingRight = false;
            }
        }
    }

    // Hop right if paramter is 0, hop left if parameter is 1
    private void Hop(int hopDirection)
    {
        if (hopDirection == 0)
        {
            slimeAnimator.SetTrigger("jump");
            StartCoroutine(JumpTimingRight());
        }
        else
        {
            slimeAnimator.SetTrigger("jump");
            StartCoroutine(JumpTimingLeft());
        }
    }

    // Throws star to player position in an arc
    IEnumerator LaunchStar(Vector3 playerPosition)
    {
        // Short delay added before Projectile is thrown
        slimeAnimator.SetTrigger("throw");
        yield return new WaitForSeconds(0.5f);
        float firingAngle = 70;

        // Calculate star spawn position
        Vector3 starSpawnPos = transform.position;
        if (isFacingRight)
        {
            starSpawnPos.x = transform.position.x + 0.75f;
        }
        else
        {
            starSpawnPos.x = transform.position.x - 0.75f;
        }
        starSpawnPos.y = transform.position.y + 0.5f;

        // Instantiate star
        GameObject star = Instantiate(throwingStar, starSpawnPos, transform.rotation);
        Rigidbody2D starRb = star.GetComponent<Rigidbody2D>();


        float throwForce = Math.Abs(playerPosition.x - transform.position.x) * 0.32f;

        if (isFacingRight)
        {
            starRb.AddForce(new Vector2(3, 2) * throwForce, ForceMode2D.Impulse);
        }
        else
        {
            starRb.AddForce(new Vector2(-3, 2) * throwForce, ForceMode2D.Impulse);
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
