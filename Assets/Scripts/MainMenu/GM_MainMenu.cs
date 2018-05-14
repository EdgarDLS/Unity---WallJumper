using UnityEngine;
using UnityEngine.SceneManagement;

public class GM_MainMenu : MonoBehaviour
{
    public GameObject optionsMenu;

    private void Start()
    {
        optionsMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void LoadOptions()
    {
        optionsMenu.SetActive(true);   
    }

    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
