using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 50f;
    public static int currentBullets = 5;
    public int maxBullets = 5;

    public BulletUI bulletUI;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        ResetBullets();

        CollacTableShuriken.OnShurikenCollected += AddBullet;

        GameController.OnReset += ResetBullets;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && currentBullets > 0 && !PlayerHealth.isDead && !GameController.isPaused && !BossGameController.isPaused)
        {
            StartCoroutine(Throw());
            currentBullets--;
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            if(playerHealth.currentHealth > 0)
            {
                Shoot();
            }
            bulletUI.UpdateBullets(currentBullets);
        }
    }

    void AddBullet(int worth)
    {
        currentBullets += worth;
        if(currentBullets > maxBullets)
        {
            currentBullets = maxBullets;
        }
        bulletUI.UpdateBullets(currentBullets);
        bulletUI.SetMaxBullets(currentBullets);
    }

    void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in the world

        Vector3 shootDirection = (mousePosition - transform.position).normalized; // Normalized to get a vector of length 1

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity); // Quaternion.identity means no rotation
        SoundEffectManager.Play("Shoot");
        bullet.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(shootDirection.x, shootDirection.y) * bulletSpeed;
        Destroy(bullet, 3f);
    }

    IEnumerator Throw()
    {
        animator.SetTrigger("throw");
        yield return new WaitForSeconds(0.3f);
        animator.SetTrigger("idle");
    }

    void ResetBullets()
    {
        currentBullets = maxBullets;
        bulletUI.SetMaxBullets(maxBullets);
    }
}
