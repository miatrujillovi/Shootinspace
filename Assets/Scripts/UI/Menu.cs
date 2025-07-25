using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject gameplayScreen;
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
        gameplayScreen.SetActive(false);
        pauseMenuUI.SetActive(true);
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        AudioManager.Instance.IsInMenu = true;
    }

    public void Reanudar()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        gameplayScreen.SetActive(true);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        AudioManager.Instance.IsInMenu = false;
    }
}
