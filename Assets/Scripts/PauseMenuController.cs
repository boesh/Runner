using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///controling ui elements, changing timeScale for pause 
/// </summary>
public class PauseMenuController : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenuPanel;
    [SerializeField]
    GameObject pauseButton;
    [SerializeField]
    GameObject resumeButton;
    [SerializeField]
    GameObject deathText;
    public void Death()
    {
        PauseGame();
        resumeButton.SetActive(false);
        deathText.SetActive(true);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        mainMenuPanel.SetActive(false);
        pauseButton.SetActive(true);
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        mainMenuPanel.SetActive(true);
        pauseButton.SetActive(false);
    }
    public void RestartGame()
    {
        resumeButton.SetActive(true);
        ResumeGame();
        deathText.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
