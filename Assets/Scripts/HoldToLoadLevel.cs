using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HoldToLoadLevel : MonoBehaviour
{
    public float holdDuration = 1f; // Duration in seconds to hold the button
    public Image fillCircle; // Reference to the fill circle image

    private float holdTimer = 0f; // Timer to keep track of how long the button has been held
    private bool isHolding = false; // Flag to keep track of whether the button is being held

    public static event Action OnHoldComplete;
    
    
    void Update()
    {
        if(isHolding && GameController.isLevelComplete)
        {
            holdTimer += Time.deltaTime;
            fillCircle.fillAmount = holdTimer / holdDuration;
            
            if(holdTimer >= holdDuration)
            {
                //LoadLevel();
                OnHoldComplete?.Invoke();
                ResetHold();
            }
        }
    }

    // Run hold animation when enter door
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isHolding = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ResetHold();
        }
    }

    /*public void OnHold(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            isHolding = true;
        }
        else if(context.canceled)
        {
            ResetHold();
        }
    }*/

    private void ResetHold()
    {
        isHolding = false;
        holdTimer = 0f;
        fillCircle.fillAmount = 0f;
    }
}
