using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BulletUI : MonoBehaviour
{
    public Image bulletPrefab;
    public Sprite fullBulletSprite;
    private List<Image> bullets = new List<Image>();

    public void SetMaxBullets(int maxBullets)
    {
        foreach (Image bullet in bullets)
        {
            Destroy(bullet.gameObject);
        }
        bullets.Clear(); // Clear the list

        for (int i = 0; i < maxBullets; i++)
        {
            Image newBullet = Instantiate(bulletPrefab, transform); // Create a new bullet
            newBullet.sprite = fullBulletSprite; // Set the sprite to the full bullet sprite
            bullets.Add(newBullet); // Add the bullet to the list
        }
    }

    public void UpdateBullets(int currentBullets)
    {
        for(int i = bullets.Count - 1; i >= 0; i--)
        {
            if(i >= currentBullets)
            {
                Destroy(bullets[i].gameObject);
                bullets.RemoveAt(i);
            }
            else
            {
                bullets[i].sprite = fullBulletSprite; // Set the sprite to the full bullet sprite
            }
            
        }
    }
}
