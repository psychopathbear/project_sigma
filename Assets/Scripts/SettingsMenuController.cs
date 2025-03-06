using UnityEngine;

public class SettingsMenuController : MonoBehaviour
{
    public GameObject mainMenu;

    public void OnBackClick()
    {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
}
