using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image heartPrefab;
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    private List<Image> hearts = new List<Image>();
    
    public void SetMaxHearts(int maxHearts)
    {
        foreach (Image heart in hearts)
        {
            Destroy(heart.gameObject);
        }
        hearts.Clear();

        for (int i = 0; i < maxHearts; i++)
        {
            Image newHeart = Instantiate(heartPrefab, transform); // Create a new heart
            newHeart.sprite = fullHeartSprite; // Set the sprite to the full heart sprite
            newHeart.color = Color.red;
            hearts.Add(newHeart); // Add the heart to the list
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeartSprite; // Set the sprite to the full heart sprite
                hearts[i].color = Color.red;
            }
            else
            {
                hearts[i].sprite = emptyHeartSprite; // Set the sprite to the empty heart sprite
                hearts[i].color = Color.white;
            }
        }
    }
}
