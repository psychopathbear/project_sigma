using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    public float chaseSpeed = 2f;
    public float jumpForce = 2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;
    public int damage = 1;
    public int maxHealth = 2;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Color ogColor;

    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        ogColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        TurnToPlayer();
        isGrounded = Physics2D.Raycast(transform.position, UnityEngine.Vector2.down, 1f, groundLayer);
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        bool isPlayerAbove = Physics2D.Raycast(transform.position, UnityEngine.Vector2.up, 5f, 1 << player.gameObject.layer);

        if(isGrounded)
        {
            rb.linearVelocity = new UnityEngine.Vector2(chaseSpeed * direction, rb.linearVelocity.y);
            
            RaycastHit2D groundInfront = Physics2D.Raycast(transform.position, new UnityEngine.Vector2(direction, 0), 2f, groundLayer);
            RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new UnityEngine.Vector3(direction, 0, 0), UnityEngine.Vector2.down, 2f, groundLayer);
            RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, UnityEngine.Vector2.up, 3f, groundLayer);

            if(!groundInfront.collider && !gapAhead.collider)
            {
                shouldJump = true;
            }
            else if(platformAbove.collider && isPlayerAbove)
            {
                shouldJump = true;
            }
            else
            {
                shouldJump = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if(shouldJump && isGrounded)
        {
            shouldJump = false;
            UnityEngine.Vector2 direction = (player.position - transform.position).normalized;

            UnityEngine.Vector2 jumpDirection = direction * jumpForce;

            rb.AddForce(new UnityEngine.Vector2(jumpDirection.x, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void TurnToPlayer()
    {
        if(player.position.x < transform.position.x)
        {
            transform.localScale = new UnityEngine.Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new UnityEngine.Vector3(1, 1, 1);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            StartCoroutine(FlashRed());
            Die();
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    private void Die()
    {
        foreach(LootItem lootItem in lootTable)
        {
            if(Random.Range(0f, 100f) <= lootItem.dropChance)
            {
                InstantiateLoot(lootItem.itemPrefab);
            }
            break;
        }
        Destroy(gameObject);
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = ogColor;
    }

    void InstantiateLoot(GameObject loot)
    {
        if(loot)
        {
            GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);
        }
    }
}

