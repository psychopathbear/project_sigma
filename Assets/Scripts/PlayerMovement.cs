using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.Boss.Combat.Projectiles; // Add this line to include the Projectile class

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public bool isFacingRight = true;
    public ParticleSystem smokeFX;
    public ParticleSystem speedFX;
  

    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;
    float speedMultiplier = 1f;

    [Header("Dashing")]
    public float dashSpeed = 10f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 0.1f;
    bool isDashing;
    bool canDash = true;

    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxJumps = 2;
    int jumpsRemaining;

    [Header("WallCheck")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.49f, 0.03f);
    public LayerMask wallLayer;

    [Header("GroundCheck")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool isGrounded;
    bool isOnPlatform;

    [Header("WallMovement")]
    public float wallSlideSpeed = 2f;
    bool isWallSliding;

    //Wall Jumping
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);

    [Header("Climbing")]
    public Transform climbCheckPos;
    public Vector2 climbCheckSize = new Vector2(0.5f, 0.05f);
    public float climbSpeed = 5f;
    float verticalMovement;
    public LayerMask climbLayer;
    public Transform climbTopCheckPos;
    public Vector2 climbTopCheckSize = new Vector2(0.5f, 0.05f);
    bool isClimbing;
    bool isInTopClimb;

    [Header("Attack")]
    public Transform attackPointPos;
    public Vector2 attackRange = new Vector2(0.5f, 0.05f);
    public LayerMask enemyLayer;
    public LayerMask bossLayer;
    public LayerMask projectilesLayer;
    public int attackDamage = 1;
    public float attackDuration = 1f;
    bool isAttacking;
    

    [Header("Gravity")]
    public float baseGravity = 2;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpeedItem.OnSpeedCollected += StartSpeedBoost;
    }

    // Update is called once per frame
    void Update()
    { 
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetFloat("magnitude", rb.linearVelocity.magnitude);
        animator.SetBool("isWallSliding", isWallSliding);
        animator.SetBool("isClimbing", isClimbing);
        animator.SetBool("isAttacking", isAttacking);

        if(isDashing)
        {
            return;
        }

        GroundCheck();
        ClimbCheck();
        ProcessGravity();
        ProcessWallSlide();
        ProcessWallJump();
        ProcessClimbing();

        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed * speedMultiplier, rb.linearVelocity.y); // Move the player
            Flip();
        }
    }

    private void StartSpeedBoost(float speedMultiplier)
    {
        StartCoroutine(SpeedBoostCoroutine(speedMultiplier));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier)
    {
        speedMultiplier = multiplier;
        speedFX.Play();
        yield return new WaitForSeconds(2.5f);
        speedMultiplier = 1f;
        speedFX.Stop();
    }

    private void ProcessGravity()
    {
        if(rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier; // Increase gravity when falling
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed)); // Limit the fall speed)
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void ProcessWallSlide()
    {
        if(!isGrounded && WallCheck() && horizontalMovement !=0 )
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed)); // Limit the fall speed
        }
        else
        {
            isWallSliding = false;
        }
    }   

    private void ProcessClimbing()
    {
        if(isClimbing && !isInTopClimb && !PlayerHealth.isDead) 
        {
            jumpsRemaining = maxJumps;
            rb.gravityScale = 0;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalMovement * climbSpeed);
        }
        else if(isClimbing && isInTopClimb)
        {
            jumpsRemaining = maxJumps;
            rb.gravityScale = 0;
            float climbVelocity = verticalMovement * climbSpeed;
            
            // Eğer yukarı hareket (W tuşu, pozitif verticalMovement) yapılmaya çalışılıyorsa, yukarı çıkışı engelle.
            if (verticalMovement > 0)
            {
                climbVelocity = 0;
            }
            
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, climbVelocity);
        }
        else
        {
            rb.gravityScale = baseGravity;   
        }
    }

    private void ProcessWallJump()
    {
        if(isWallSliding && !PlayerHealth.isDead)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x; // Jump in the opposite direction of the wall
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else if(wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    public void ProcessMove(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        verticalMovement = context.ReadValue<Vector2>().y;
    }

    public void Drop(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && isOnPlatform && !isClimbing)
        {
            StartCoroutine(DisablePlayerCollider(0.35f));
        }
    }

    private IEnumerator DisablePlayerCollider(float disableTime)
    {
        gameObject.transform.Find("Collider").GetComponent<Collider2D>().enabled = false;
        //playerCollider.enabled = false;
        yield return new WaitForSeconds(disableTime);
        gameObject.transform.Find("Collider").GetComponent<Collider2D>().enabled = true;
        //playerCollider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isOnPlatform = true;
        }
        if(collision.gameObject.CompareTag("BottomBorder"))
        {
            gameObject.GetComponent<PlayerHealth>().TakeDamage(1, transform.position);
            transform.position = new Vector3(-3.75f, 4.38f, 1f);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isOnPlatform = false;
        }
    }

    public void ProcessAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !isClimbing)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    private IEnumerator AttackCoroutine()
    {
        if (PlayerHealth.isDead || isAttacking)
        {
            yield break;
        }

        isAttacking = true;

        animator.SetTrigger("attack");
        SoundEffectManager.Play("Attack");

        yield return new WaitForSeconds(attackDuration);

        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPointPos.position, attackRange, 0, enemyLayer | bossLayer | projectilesLayer);
        
        if (hitEnemies.Length > 0) // Add safety check
        {
            Debug.Log(hitEnemies.Length + hitEnemies[0].name);
            
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.CompareTag("Arrow"))
                {
                    Projectile arrow = enemy.GetComponent<Projectile>();
                    if (arrow != null)
                    {
                        Vector2 forceDirection = isFacingRight ? Vector2.right : Vector2.left;
                        forceDirection.y = 0;
                        arrow.SetForce(forceDirection * 55f);
                    }
                }
                else
                {
                    // Try to get EnemyHealth from this object or any parent objects
                    EnemyHealth enemyHealth = enemy.GetComponentInParent<EnemyHealth>();
                    Debug.Log(enemyHealth);
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(attackDamage);
                    }
                }
            }
        }

        isAttacking = false;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && !PlayerHealth.isDead)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        Physics2D.IgnoreLayerCollision(3, 11, true); // Ignore collision between player and enemy layers
        Physics2D.IgnoreLayerCollision(3, 15, true); 
        isDashing = true;
        canDash = false;

        animator.SetTrigger("dash");
        
        float dashDirection = isFacingRight ? 1 : -1;

        rb.linearVelocity = new Vector2(dashSpeed * dashDirection, rb.linearVelocity.y); // Dash in the direction the player is facing

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stop the player after the dash

        isDashing = false;

        animator.SetBool("isDashing", false);
        Physics2D.IgnoreLayerCollision(3, 11, false); // Enable collision between player and enemy layers
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void ProcessJump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0 && !PlayerHealth.isDead && !isClimbing)
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                SoundEffectManager.Play("Jump");
                jumpsRemaining--;
                JumpFX();
            }
            else if (context.canceled)
            {
                if (rb.linearVelocity.y > 0)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                    jumpsRemaining--;
                    JumpFX();
                }
            }
        }

        // Wall Jumping
        if(context.performed && wallJumpTimer > 0)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y); //Jump in the direction of the wall
            wallJumpTimer = 0;
            JumpFX();
            //Force Flip

            if(transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }

            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f); // Cancel the wall jump after the wall jump time
        }
    }

    private void JumpFX()
    {
        if(!isAttacking)
        {
        animator.SetTrigger("jump");
        smokeFX.Play();
        } 
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer))
        {
            jumpsRemaining = maxJumps;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    
    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }

    private void ClimbCheck()
    {
        if(Physics2D.OverlapBox(climbCheckPos.position, climbCheckSize, 0, climbLayer) && Physics2D.OverlapBox(climbTopCheckPos.position, climbTopCheckSize, 0, climbLayer))
        {
            isClimbing = true;
            isInTopClimb = false;
        }   
        else if (Physics2D.OverlapBox(climbCheckPos.position, climbCheckSize, 0, climbLayer) && !Physics2D.OverlapBox(climbTopCheckPos.position, climbTopCheckSize, 0, climbLayer))
        {
            isClimbing = true;
            isInTopClimb = true;
        }
        else
        {
            isClimbing = false;
            isInTopClimb = false;
        }   
    }

    private void Flip()
    {
        if(PlayerHealth.isDead)
        {
            return;
        }

        if(isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
            speedFX.transform.localScale = ls;

            if(rb.linearVelocity.y == 0)
            {
                smokeFX.Play();
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(climbCheckPos.position, climbCheckSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(climbTopCheckPos.position, climbTopCheckSize);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(attackPointPos.position, attackRange);
    }
    

}
