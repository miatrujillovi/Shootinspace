using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
    public void RestartGame()
    {
        Time.timeScale = 1f;
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.RestartGame();
        }
        else
        {
            Debug.LogError("LevelManager.Instance is null. Make sure the LevelManager is in the scene and assigned.");
        }
    }

    public void GoBackMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu");
    }
}
