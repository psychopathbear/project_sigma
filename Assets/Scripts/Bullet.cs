using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Boss")
        {
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(bulletDamage);
                Destroy(gameObject);
            }
        }
    }
}
