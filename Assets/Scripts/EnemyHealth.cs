using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    [SerializeField] private int maxHealth = 2;
    private Color ogColor;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    public static bool isEnemyDead = false;

    public bool Invincible { get; set; }

    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        ogColor = spriteRenderer.color;
        Invincible = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Current enemy health: " + currentHealth);
        if(currentHealth <= 0 && !isEnemyDead && !gameObject.CompareTag("Boss"))
        {
            Debug.Log("Enemy is dead");
            isEnemyDead = true;
            Die();
        }
        else if(currentHealth > 0 && !Invincible)
        {
            StartCoroutine(FlashRed());
            isEnemyDead = false;
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        anim.SetTrigger("hurt");
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

    private void Die()
    {
        if(GetComponentInParent<EnemyPatrol>() != null)
        {
            GetComponentInParent<EnemyPatrol>().enabled = false;
        }
        if(GetComponent<MeleeEnemy>() != null)
        {
            GetComponent<MeleeEnemy>().enabled = false;
        }

        foreach(LootItem lootItem in lootTable)
        {
            if(Random.Range(0f, 100f) <= lootItem.dropChance)
            {
                InstantiateLoot(lootItem.itemPrefab);
            }
            break;
        }
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        anim.SetTrigger("die");
        yield return new WaitForSeconds(1.4f);
        gameObject.SetActive(false);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isEnemyDead = false;
    }

}
