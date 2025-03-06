using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int damage = 3;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(damage, transform.position);
        }
        
    }
}
