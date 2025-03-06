using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public GameObject player;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void FreezeTime(float duration)
        {
            Time.timeScale = 0.1f;
            var playerInput = player.GetComponent<PlayerInput>();
            playerInput.enabled = false;
            StartCoroutine(UnfreezeTime(duration));
        }
        
        private IEnumerator UnfreezeTime(float duration)
        {
            yield return new WaitForSeconds(duration);
            var playerInput = player.GetComponent<PlayerInput>();
            playerInput.enabled = true;
            Time.timeScale = 1.0f;
        }
    }
}