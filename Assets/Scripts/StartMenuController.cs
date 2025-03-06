using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject howToPlayMenu;
    public GameObject difficultyMenu;

    public void OnStartClick()
    {
        gameObject.SetActive(false);
        difficultyMenu.SetActive(true);
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();        
    }

    // Settings Button
    public void OnSettingsClick()
    {
        gameObject.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void HowToPlayClick()
    {
        gameObject.SetActive(false);
        howToPlayMenu.SetActive(true);
    }
}
