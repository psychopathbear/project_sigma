using System.Collections;
using UnityEngine;

public class SecretArea : MonoBehaviour
{
    public float fadeDuration = 1f;
    SpriteRenderer spriteRenderer;
    Color hiddenColour;
    Coroutine currentCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hiddenColour = spriteRenderer.color;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(FadeOut(true));
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(FadeOut(false));
        }
    }

    private IEnumerator FadeOut(bool fadeOut)
    {
        Color startColour = spriteRenderer.color;
        Color endColour = fadeOut ? new Color(hiddenColour.r, hiddenColour.g, hiddenColour.b, 0f) : hiddenColour;
        float timeFading = 0f;

        while(timeFading < fadeDuration)
        {
            timeFading += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(startColour, endColour, timeFading / fadeDuration);
            yield return null;
        }

        spriteRenderer.color = endColour;
    }
}
