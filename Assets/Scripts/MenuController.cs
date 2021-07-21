
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main menu controller have methods for work with main menu buttons
/// </summary>
public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
