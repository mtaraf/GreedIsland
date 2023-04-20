using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SlimeBossMonster : Mob
{
    private Rigidbody2D rb;
    private Animator slimeAnimator;
    private GameObject megaJumpObject;
    private GameObject hurtBoxObject;
    private GameObject hitBoxObject;
    private Hitbox hitbox;
    private BoxCollider2D hitboxCollider;
    private BoxCollider2D hurtboxCollider;
    private Animator megaJumpAnimator;
    private GameObject player;
    private PlayerController playerController;
    private PlayerHurtbox playerHurtbox;

    private float jumpForce;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject minionPrefab;

    public bool megaJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        slimeAnimator = GetComponent<Animator>();
        megaJumpObject = GameObject.Find("MegaJumpLandingAnimation");
        megaJumpAnimator = megaJumpObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerHurtbox = player.GetComponentInChildren<PlayerHurtbox>();
        hurtBoxObject = transform.GetChild(0).gameObject;
        hitBoxObject = transform.GetChild(1).gameObject;
        hitbox = hitBoxObject.GetComponent<Hitbox>();
        hitboxCollider = hitBoxObject.GetComponent<BoxCollider2D>();
        hurtboxCollider = hurtBoxObject.GetComponent<BoxCollider2D>();

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
        Tuple<bool, Vector3> detectPlayerInAggressionRange;
        Tuple<bool, Vector3> detectPlayerInAttackRange;
        int slimeRandomJumpCounter = 0;
        int movementDirection = 0;
        while (!isDead)
        {
            detectPlayerInAggressionRange = DetectPlayer(25.0f);
            detectPlayerInAttackRange = DetectPlayer(10.0f);
            if (detectPlayerInAggressionRange.Item1 && !detectPlayerInAttackRange.Item1)
            {
                movementDirection = GetHopDirection(detectPlayerInAggressionRange.Item2);
                RotateSlime(movementDirection);
                if (IsOnGround())
                {
                    Hop(movementDirection);
                    yield return new WaitForSeconds(1.5f);
                }
                yield return new WaitForSeconds(0.1f);
            }
            else if (detectPlayerInAttackRange.Item1)
            {
                if (IsOnGround())
                {
                    int attack = Random.Range(0, 5);
                    if (attack == 0)
                    {
                        MegaHopAttack();
                        yield return new WaitForSeconds(3.5f); // wait longer 
                    }
                    else if (attack == 1)
                    {
                        FireballAttack(detectPlayerInAttackRange.Item2);
                        yield return new WaitForSeconds(1.5f);
                    }
                    else if (attack == 2)
                    {
                        SummonMinions(2); // change to 1-3 minions
                        yield return new WaitForSeconds(3f);
                    }
                    else
                    {
                        Heal(5);
                        // play heal effect
                        yield return new WaitForSeconds(2f);
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }
            else  // Hop Randomly every 6 seconds
            {
                if (slimeRandomJumpCounter < 6)
                {
                    yield return new WaitForSeconds(1f);
                    slimeRandomJumpCounter++;
                }
                else
                {
                    //bool isOnGround = IsOnGround();
                    Debug.Log(IsOnGround());
                    if (IsOnGround())
                    {
                        Hop(Random.Range(0, 2));
                        yield return new WaitForSeconds(0.5f);
                        slimeRandomJumpCounter = 0;
                    }
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    // Summons minion slimes
    private void SummonMinions(int numberOfMinions)
    {
        for (int i = 0; i < numberOfMinions; i++)
        {
            CreateAndLaunchMinion();
        }
    }

    // Instantiates and launches minion in random directions
    private void CreateAndLaunchMinion()
    {
        slimeAnimator.SetTrigger("summon");
        Vector3 projectileStartPosition = transform.position;
        Vector2 launchVector;
        if (isFacingRight)
        {
            projectileStartPosition.x += 0.3f;
            launchVector = new Vector2(1, 3);
        }
        else
        {
            projectileStartPosition.x -= 0.3f;
            launchVector = new Vector2(-1, 3);
        }
        projectileStartPosition.y += 1.23f;
        GameObject minion = Instantiate(minionPrefab, projectileStartPosition, transform.rotation);
        Rigidbody2D minionRigidbody = minion.GetComponent<Rigidbody2D>();
        int randomForceAmount = Random.Range(1, 5);
        minionRigidbody.AddForce(launchVector * randomForceAmount, ForceMode2D.Impulse);
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

    private void FireballAttack(Vector3 playerPos)
    {
        Vector3 projectileStartPosition = transform.position;
        if (isFacingRight)
        {
            projectileStartPosition.x += 4;
        }
        else
        {
            projectileStartPosition.x -= 4;
        }
        projectileStartPosition.y += 2;
        Instantiate(fireballPrefab, projectileStartPosition, transform.rotation);
    }

    // Jumps high in the air and damages player if player is on the ground when it lands. Instantly kills player if lands directly on player
    private void MegaHopAttack()
    {
        //slimeAnimator.SetTrigger("megaJump");
        //rb.velocity = Vector2.zero;
        //rb.AddForce(new Vector2(0,10), ForceMode2D.Impulse);
        StartCoroutine(MegaHopDamageCheck());
    }

    IEnumerator MegaHopDamageCheck()
    {
        slimeAnimator.SetTrigger("megaJump");
        yield return new WaitForSeconds(0.3f); // animation timing

        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);

        float areaOfEffect = 9.6f;
        float playerPositionX = player.transform.position.x;
        float bossPositionX = transform.position.x;
        megaJumping = true;

        // turn off colliders
        hitboxCollider.gameObject.SetActive(false);
        hurtboxCollider.gameObject.SetActive(false);

        // Wait for jump
        yield return new WaitForSeconds(0.2f);

        // Check if Boss has landed
        while (transform.position.y > 0.69f)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // Player animation and turn colliders back on
        megaJumpAnimator.SetTrigger("megaJump");
        hitboxCollider.gameObject.SetActive(true);
        hurtboxCollider.gameObject.SetActive(true);

        // if player is directly under boss
        float halfBossWidth = hurtboxCollider.size.x / 2;
        if (playerPositionX > (bossPositionX - halfBossWidth) && playerPositionX < (bossPositionX + halfBossWidth))
        {
            playerHurtbox.OneHitKill();
            Debug.Log("Player got one-shot");
        }
        if (playerController.IsOnGround() && (playerPositionX < bossPositionX + areaOfEffect) && (playerPositionX > bossPositionX - areaOfEffect)) //checks if player is on ground within _ units of the boss when it lands, then takes damage/knockback
        {
            playerHurtbox.TakeDamage(10, hitbox.PlayerDirection(player.transform.position));
        }

        megaJumping = false;
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

    private 

    IEnumerator JumpTimingRight()
    {
        yield return new WaitForSeconds(0.3f);
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(1, 2.5f) * jumpForce, ForceMode2D.Impulse);
    }

    IEnumerator JumpTimingLeft()
    {
        yield return new WaitForSeconds(0.3f);
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(-1, 2.5f) * jumpForce, ForceMode2D.Impulse);
    }

    // Return a bool determining if the entity is on the ground or not
    private bool IsOnGround()
    {
        return Physics2D.BoxCast(hurtboxCollider.bounds.min, new Vector2(0.01f, 0.01f), 0f, Vector2.down, 0.01f, jumpableGround); // creates small boxcast at bottom of collider
    }
}
