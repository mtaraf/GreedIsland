using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float playerMaxHealth;
    public float playerSpeed;
    public float jumpPower;
    public float playerDamage;
    public float playerCurrentHealth;
    public float playerDefence;

    private bool isFacingRight;
    public bool gettingKnockbacked = false;

    private Rigidbody2D rb;
    private BoxCollider2D playerBox;
    private Animator animator;
    private GameObject hurtbox;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private LayerMask jumpableGround;

    public GameObject lowerRaycast;
    public GameObject higherRaycast;
    public GameObject fireballProjectile;
    public float stepHeight = 0.3f;
    public float stepSmooth = 0.001f;
    public bool isGameActive;

    void Start()
    {
        playerBox = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isFacingRight = true;
        isGameActive = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        hurtbox = GameObject.FindGameObjectWithTag("PlayerHurtbox");
        playerSpeed = 4.0f;
        jumpPower = 5.0f;

        if (MainManager.instance != null)
        {
            if (MainManager.instance.isNewGame)
            {
                playerCurrentHealth = 10;
                playerMaxHealth = 10;
                playerDamage = 5;
                playerDefence = 5;
                playerSpeed = 4.0f;
                jumpPower = 5.0f;
            }
            else
            {
                if (MainManager.instance.loadLocal)
                {
                    LoadPlayerDataBetweenScenes();
                }
                else
                {
                    LoadPlayerData();
                }
            }
        }
    }

    
    void Update()
    {
        if (isGameActive)
        {
            PlayerAbilities();
            //PlayerMovement();
        }
    }

    private void FixedUpdate()
    {
        if (isGameActive)
        {
            PlayerMovement();
        }
    }

    private void PlayerMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput < 0 && isFacingRight)
        {
            ChangeDirection(isFacingRight);
        }
        else if (horizontalInput > 0 && !isFacingRight)
        {
            ChangeDirection(isFacingRight);
        }

        if (!gettingKnockbacked)
        {
            rb.velocity = new Vector2(playerSpeed * horizontalInput, rb.velocity.y);
        }

        if (horizontalInput != 0)
        {
            animator.SetBool("isWalking", true);
            RampClimb();
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    public void TakeKnockback(int direction)
    {
        gettingKnockbacked = true;
        rb.velocity.Set(1, rb.velocity.y);
        if (direction == 1) // left
        {
            rb.AddForce(new Vector2(-2.5f, 2), ForceMode2D.Impulse);
        }
        else // right
        {
            rb.AddForce(new Vector2(2.5f, 2), ForceMode2D.Impulse);
        }
        StartCoroutine(Knockbacked());
    }

    public void TakeDamage(float damage)
    {
        playerCurrentHealth -= damage;
        StartCoroutine(InvisibilityFrames());
    }

    public void OneHitKill()
    {
        playerCurrentHealth = 0;
        // Set death trigger for animator
    }

    IEnumerator InvisibilityFrames()
    {
        if (hurtbox != null)
        {
            hurtbox.SetActive(false);
            Debug.Log("box deactivated");
            // Set Sprite Color
            yield return new WaitForSeconds(2.0f);
            Debug.Log("box active");
            hurtbox.SetActive(true);
        }
    }

    IEnumerator Knockbacked()
    {
        yield return new WaitForSeconds(0.6f);
        gettingKnockbacked = false;
    }

    IEnumerator IceEffect()
    {
        playerSpeed /= 2;
        spriteRenderer.color = new Color(0, 121, 255);
        yield return new WaitForSeconds(2);
        playerSpeed *= 2;
        spriteRenderer.color = new Color(255, 255, 255);
    }

    public void TriggerIceEffect()
    {
        StartCoroutine(IceEffect());
    }

    private void PlayerAbilities()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsOnGround())
            {
                PlayerJump();
                animator.SetTrigger("jump");
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightControl))
        {
            animator.SetTrigger("basicAttack");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            ProjectileCast();
        }
    }

    void ChangeDirection(bool currentDirection)
    {
        transform.Rotate(new Vector2(0, 180));
        if (currentDirection)
        {
            isFacingRight = false;
            return;
        }
        isFacingRight = true;
    }

    void PlayerJump()
    {
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    void ProjectileCast()
    {
        animator.SetTrigger("cast");
        Vector3 spawnPos;
        if (isFacingRight)
        {
            spawnPos = new Vector3(transform.position.x + 0.7f, transform.position.y - 0.8f, transform.position.z);
        }
        else
        {
            spawnPos = new Vector3(transform.position.x - 0.7f, transform.position.y - 0.8f, transform.position.z);
        }
        Instantiate(fireballProjectile, spawnPos, transform.rotation);
    }


    void RampClimb()
    {
        if (isFacingRight)
        {
            RaycastHit2D hitLower = Physics2D.Raycast(lowerRaycast.transform.position, Vector2.right, 0.1f);
            if (hitLower.collider != null)
            {
                RaycastHit2D hitUpper = Physics2D.Raycast(lowerRaycast.transform.position, Vector2.right, 0.2f);
                if (hitUpper.collider != null)
                {
                    rb.position -= new Vector2(0, -stepSmooth);
                }
            }
        }
        else
        {
            RaycastHit2D hitLower = Physics2D.Raycast(lowerRaycast.transform.position, Vector2.left, 0.1f);
            if (hitLower.collider != null)
            {
                RaycastHit2D hitUpper = Physics2D.Raycast(lowerRaycast.transform.position, Vector2.left, 0.2f);
                if (hitUpper.collider != null)
                {
                    rb.position -= new Vector2(0, -stepSmooth);
                }
            }
        }
    }

    // Return a bool determining if the entity is on the ground or not, true if entity is on the ground
    public bool IsOnGround()
    {
        //return Physics2D.BoxCast(playerBox.bounds.min, new Vector2(0.1f,0.1f), 0f, Vector2.down, 0.01f, jumpableGround); // creates small boxcast at bottom of collider
        return true; // for testing
    }

    public void SavePlayerData()
    {
        if (MainManager.instance != null)
        {
            MainManager.instance.playerAttack = playerDamage;
            MainManager.instance.playerDefense = playerDefence;
            MainManager.instance.playerMaxHealth = playerMaxHealth;
            MainManager.instance.playerCurrentHealth = playerCurrentHealth;
            MainManager.instance.playerPosition = transform.position;
            Scene scene = new Scene();
            scene = SceneManager.GetActiveScene();
            MainManager.instance.sceneNumber = scene.buildIndex;
            MainManager.instance.SavePlayerData();
            MainManager.instance.loadLocal = false;
        }
    }

    public void LoadPlayerData()
    {
        if (MainManager.instance != null)
        {
            SceneManager.LoadScene(MainManager.instance.sceneNumber);
            playerDamage = MainManager.instance.playerAttack;
            playerDefence = MainManager.instance.playerDefense;
            playerMaxHealth = MainManager.instance.playerMaxHealth;
            playerCurrentHealth = MainManager.instance.playerCurrentHealth;
            transform.position = MainManager.instance.playerPosition;
            playerSpeed = MainManager.instance.playerSpeed;
            jumpPower = MainManager.instance.playerJump;
        }
        //playerHealth.SetHealth(currentHealth, maxHealth);
    }

    public void SavePlayerDataBetweenScenes()
    {
        MainManager.instance.playerAttack = playerDamage;
        MainManager.instance.playerDefense = playerDefence;
        MainManager.instance.playerMaxHealth = playerMaxHealth;
        MainManager.instance.playerCurrentHealth = playerCurrentHealth;
        MainManager.instance.playerJump = jumpPower;
        MainManager.instance.playerSpeed = playerSpeed;
        MainManager.instance.loadLocal = true;
    }

    public void LoadPlayerDataBetweenScenes()
    {
        Debug.Log("Loaded locally");
        playerDamage = MainManager.instance.playerAttack;
        playerDefence = MainManager.instance.playerDefense;
        playerMaxHealth = MainManager.instance.playerMaxHealth;
        playerCurrentHealth = MainManager.instance.playerCurrentHealth;
        playerSpeed = MainManager.instance.playerSpeed;
        jumpPower = MainManager.instance.playerJump;
    }
}
