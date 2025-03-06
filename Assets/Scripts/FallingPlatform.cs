using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallWait = 2f;
    public float destoryWait = 1f;

    private bool isFalling;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        isFalling = true;
        yield return new WaitForSeconds(fallWait);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destoryWait);
    }
}
