using UnityEngine;

public class Trap : MonoBehaviour
{
    public float bounceForce = 10f;
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            HandlePlayerBounce(collision.gameObject);
        }
    }
    private void HandlePlayerBounce(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        
        if(rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }
}
