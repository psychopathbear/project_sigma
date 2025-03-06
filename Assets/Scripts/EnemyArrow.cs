using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    public int bulletDamage = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(bulletDamage, transform.position);
            Destroy(gameObject);
        }
        
    }
}
