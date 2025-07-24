using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
    public void RestartGame()
    {
        Debug.Log("Restart button pressed.");

        if (LevelManager.Instance != null)
        {
            Debug.Log("Calling RestartGame() on GameManager instance: " + LevelManager.Instance.GetInstanceID());
            LevelManager.Instance.RestartGame();
        }
        else
        {
            Debug.LogError("LevelManager.Instance is null. Make sure the LevelManager is in the scene and assigned.");
        }
    }

    public void GoBackMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
