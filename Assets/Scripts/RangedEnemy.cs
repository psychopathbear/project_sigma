using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;

    [Header ("Ranged Attack Parameters")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed = 50f;
    [SerializeField] private Vector2 rangeCheckSize;
    [SerializeField] private Transform rangeCheckPos;

    [Header ("Collider Parameter")]
    [SerializeField] private BoxCollider2D boxCollider;

    private float coolDownTimer = Mathf.Infinity;

    private Animator anim;

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
            anim.SetBool("isInRange", true);
            if (coolDownTimer >= attackCooldown)
            {
                coolDownTimer = 0;
                anim.SetTrigger("rangedAttack");
            }
        }
        else if (!PlayerInSight())
        {
            anim.SetBool("isInRange", false);
        }

        if(enemyPatrol != null)
        {
            enemyPatrol.enabled = PlayerInSight() ? false : true;
        }

        // Change direction based on player position
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        if (playerPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1); // Face left
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1); // Face right
        }
    }

    private void RangedAttack()
    {
        coolDownTimer = 0;
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        Vector3 shootDirection = (playerPosition - transform.position).normalized; // Normalized to get a vector of length 1

        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg; // Calculate the angle in degrees
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Create a rotation based on the angle

        GameObject arrow = Instantiate(arrowPrefab, transform.position, rotation); // Apply the rotation to the arrow
        arrow.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(shootDirection.x, shootDirection.y) * arrowSpeed;
        Destroy(arrow, 4f);
    }

    private bool PlayerInSight()
    {
        Collider2D hit = Physics2D.OverlapBox(rangeCheckPos.position, rangeCheckSize, 0f, LayerMask.GetMask("Player"));
        return hit != null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rangeCheckPos.position, rangeCheckSize);
    }
}
