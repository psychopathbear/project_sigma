using System;
using System.Collections;
using Core.UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 4;
    public int currentHealth;
    public HealthUI healthUI;
    public static event Action OnPlayedDied;
    public static Animator animator;
    public static bool isDead;
    public Rigidbody2D body;
    public float recoilMultiplier;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetHealth();
        GameController.OnReset += ResetHealth; // Subscribe to the OnReset event
        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        HealthItem.OnHealthCollected += Heal;
        body = GetComponent<Rigidbody2D>();
        animator.SetBool("isDead", isDead);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        Trap trap = collision.GetComponent<Trap>();
        if(trap && trap.damage > 0)
        {
            TakeDamage(trap.damage, Vector2.zero);
        }
    }

    void ResetHealth()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);
    }

    public void TakeDamage(int damage, Vector2 damageSourcePosition)
    {
        currentHealth -= damage;
        healthUI.UpdateHearts(currentHealth);
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);
        if(currentHealth > 0)
        {
            Hurt();
            Vector2 recoilDirection = (transform.position - (Vector3)damageSourcePosition).normalized;
            DoRecoil(recoilDirection * recoilMultiplier);
        }

        if(currentHealth <= 0)
        {
            Debug.Log("Player died");
            DeathAnimation();
            if (OnPlayedDied != null)
            {
                OnPlayedDied.Invoke();
            }
        }
    }

    void Heal(int healAmount)
    {
        if (currentHealth >= maxHealth)
        {
        Debug.Log("Player is already at max health.");
        return;
        }
    
        int previousHealth = currentHealth;
        currentHealth += healAmount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        int effectiveHeal = currentHealth - previousHealth;
        Debug.Log("Player healed " + effectiveHeal + " health. Current health: " + currentHealth);
        healthUI.UpdateHearts(currentHealth);
    }

    void Hurt()
    {
        GuiManager.Instance.FadeHurtVignette(0.2f);
        animator.SetTrigger("hurt");
        SoundEffectManager.Play("Hurt");
        isDead = false;
    }

    public void DoRecoil(Vector2 recoilForce, bool resetVelocity = false)
    {
        if (resetVelocity)
            body.linearVelocity = Vector2.zero;

        // Determine the direction the player is facing
        float direction = transform.localScale.x > 0 ? 1 : -1;

        // Apply recoil force in the opposite direction of the player's facing direction
        Vector2 finalRecoilForce = new Vector2(-direction * recoilForce.x, recoilForce.y);
        body.AddForce(finalRecoilForce);
    }

    void DeathAnimation()
    {
        isDead = true;
        SoundEffectManager.Play("Death");
        Debug.Log(isDead);
        animator.SetTrigger("die");
    }
}
