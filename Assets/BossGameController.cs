using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossGameController : MonoBehaviour
{
    public GameObject player;
    public GameObject loadCanvas;
    public List<GameObject> levels;
    private int currentLevelIndex = 0;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    private SpriteRenderer spriteRenderer;
    private GameObject[] bullets;
    public GameObject[] enemies;
    private GameObject[] enemyArrows;
    public static bool isPaused = false;
    public Animator animator;
    public static bool isLevelComplete = false;
    public GameObject[] nextLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        if(PlayerPrefs.GetInt("Difficulty") == 0)
        {
            SceneManager.LoadScene("BossEasy");
        }
        else if(PlayerPrefs.GetInt("Difficulty") == 1)
        {
            SceneManager.LoadScene("BossHard");
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

        currentLevelIndex = level;
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
        if(currentLevelIndex == levels.Count - 1) 
        {
            if(PlayerPrefs.GetInt("Difficulty") == 0)
            {
                // load easy boss scene
                SceneManager.LoadScene("BossEasy");
            }
            else if(PlayerPrefs.GetInt("Difficulty") == 1)
            {
                // load hard boss scene
                SceneManager.LoadScene("BossHard");
            }
        }
        else
        {
            Debug.Log("Loading next level: " + nextLevelIndex);
            LoadLevel(nextLevelIndex);
        }
        
    }
}
