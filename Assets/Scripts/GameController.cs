using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int progressAmount;

    public GameObject player;
    public GameObject loadCanvas;
    public List<GameObject> levels;
    private int currentLevelIndex = 0;
    public Slider progressSlider;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public static event Action OnReset;
    private SpriteRenderer spriteRenderer;
    private GameObject[] bullets;
    public GameObject[] collectables;
    public GameObject[] enemies;
    private GameObject[] enemyArrows;
    public int targetAmount;
    public static bool isPaused = false;
    public Animator animator;
    public static bool isLevelComplete = false;
    public GameObject[] nextLevel;
    public GameObject[] resetRequired;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        progressAmount = 0;
        progressSlider.value = 0;
        Gem.OnGemCollected += IncreaseProgressAmount;
        PlayerHealth.OnPlayedDied += GameOverScreen;
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        animator = player.GetComponent<Animator>();
        HoldToLoadLevel.OnHoldComplete += LoadNextLevel;
        Debug.Log("Enemies found: " + enemies.Length);
    }

    void Update()
    {
        bullets = GameObject.FindGameObjectsWithTag("Bullet");
        enemyArrows = GameObject.FindGameObjectsWithTag("Arrow");
    }

    void GameOverScreen()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
            MusicManager.PauseBackgroundMusic();
            Time.timeScale = 0;
            foreach (GameObject reset in resetRequired)
            {
                reset.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("GameOverScreen object is null.");
        }
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (context.started && playerHealth.currentHealth > 0) // If the button is pressed
        {
            pauseScreen.SetActive(true);
            MusicManager.PauseBackgroundMusic();
            // Stop Animation
            animator.speed = 0f;

            Time.timeScale = 0;
            isPaused = true;

            var playerInput = player.GetComponent<PlayerInput>();
            playerInput.enabled = false;
        }
    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        MusicManager.ResumeBackgroundMusic();
        // Resume Animation
        animator.speed = 1f;

        Time.timeScale = 1;
        isPaused = false;

        var playerInput = player.GetComponent<PlayerInput>();
        playerInput.enabled = true;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ResetGame()
    {
        gameOverScreen.SetActive(false);
        MusicManager.PlayBackgroundMusic(true);
        LoadLevel(currentLevelIndex);
        OnReset.Invoke();
        PlayerHealth.isDead = false;
        PlayerHealth.animator.ResetTrigger("die");
        PlayerHealth.animator.Rebind();

        spriteRenderer.color = Color.white;
        Time.timeScale = 1;

        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
        foreach (GameObject arrow in enemyArrows)
        {
            Destroy(arrow);
        }
    }

    void IncreaseProgressAmount(int amount)
    {
        progressAmount += amount;
        progressSlider.value = progressAmount;
        Debug.Log("Progress: " + progressAmount);

        if (progressAmount >= targetAmount)
        {
            nextLevel[currentLevelIndex].SetActive(true);
            isLevelComplete = true;
            Debug.Log("Level Complete!");
        }
    }

    void LoadLevel(int level)
    {
        //Stop current sounds
        SoundEffectManager.Stop();

        levels[currentLevelIndex].gameObject.SetActive(false);
        levels[level].gameObject.SetActive(true);

        ActivateCurrentLevelCollectables(level);
        ActivateCurrentLevelEnemies(level);

        nextLevel[currentLevelIndex].SetActive(false);
        nextLevel[level].SetActive(false);

        player.transform.position = new Vector3(-3.75f, 4.38f, 1f);

        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.isFacingRight = true;
            Vector3 ls = player.transform.localScale;
            if (ls.x < 0)
            {
                ls.x *= -1f;
                player.transform.localScale = ls;
            }
        }

        // Last level make bigger player
        if(levels.Count - 1 == level)
        {
            player.transform.localScale = new Vector3(1.8f, 1.8f, 1f);
            player.GetComponent<PlayerMovement>().moveSpeed = 12f;
            player.GetComponent<PlayerMovement>().dashSpeed = 8f;
            player.GetComponent<PlayerMovement>().dashCooldown = 0.3f;
            player.GetComponent<PlayerMovement>().jumpPower = 12f;
            player.GetComponent<PlayerMovement>().baseGravity = 3f;
            player.GetComponent<PlayerMovement>().fallSpeedMultiplier = 3f;
            player.GetComponent<PlayerInput>().actions.FindActionMap("Player").FindAction("Drop").Disable(); // Disable drop action
        }
        else
        {
            player.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        currentLevelIndex = level;
        progressAmount = 0;
        progressSlider.value = 0;
    }

    void ActivateCurrentLevelCollectables(int levelIndex)
    {
        Transform collectableParent = levels[levelIndex].transform.Find($"Level{levelIndex + 1}Collectable");
        if (collectableParent != null)
        {
            foreach (Transform collectable in collectableParent)
            {
                collectable.gameObject.SetActive(true);
            }
        }
    }

    void ActivateCurrentLevelEnemies(int levelIndex)
    {

        Transform enemiesParent = levels[levelIndex].transform.Find($"Level{levelIndex + 1}Enemies");
        if (enemiesParent != null)
        {

            Transform[] allChildren = enemiesParent.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in allChildren)
            {

                child.gameObject.SetActive(true);

                var patrol = child.GetComponent<EnemyPatrol>();
                if (patrol != null) patrol.enabled = true;

                var meleeEnemy = child.GetComponent<MeleeEnemy>();
                if (meleeEnemy != null) meleeEnemy.enabled = true;

                var enemyHealth = child.GetComponent<EnemyHealth>();
                if (enemyHealth != null) enemyHealth.ResetHealth();
            }
        }
    }

    void LoadNextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1;
        Debug.Log("Loading next level: " + nextLevelIndex);
        LoadLevel(nextLevelIndex);      
    }
}
