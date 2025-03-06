using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header ("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    private float coolDownTimer = Mathf.Infinity;
    private Animator anim;
    private  PlayerHealth playerHealth;

    private EnemyPatrol enemyPatrol;

    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    void Update()
    {
        coolDownTimer += Time.deltaTime;

        if(PlayerInSight())
        {
            if (coolDownTimer >= attackCooldown)
            {
                coolDownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }

        if(enemyPatrol != null)
        {
            enemyPatrol.enabled = PlayerInSight() ? false : true;
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.lossyScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0f, Vector2.left, 0f, LayerMask.GetMask("Player"));
        
        if(hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<PlayerHealth>(); // Get the player health component
        }
        
        return hit.collider != null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if(PlayerInSight())
        {
            playerHealth.TakeDamage(damage, transform.position);
        }
    }
}
