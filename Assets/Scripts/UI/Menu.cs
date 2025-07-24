using UnityEngine;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject pauseMenuUI;
    private bool isPaused = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Pausar()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        isPaused = true;

        AudioManager.Instance.IsInMenu = true;
    }

    public void Reanudar()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        isPaused = false;

        AudioManager.Instance.IsInMenu = false;
    }
}
