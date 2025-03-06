using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyMenu : MonoBehaviour
{
    public GameObject mainMenu;

    public void OnBackClick()
    {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OnEasyClick()
    {
        PlayerPrefs.SetInt("Difficulty", 0);
        SceneManager.LoadScene("EasyGame");
    }

    public void OnHardClick()
    {
        PlayerPrefs.SetInt("Difficulty", 1);
        SceneManager.LoadScene("HardGame");
    }
}
