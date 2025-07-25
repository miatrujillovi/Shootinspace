using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using DG.Tweening;
using System.Collections;

public class ScenesManager : MonoBehaviour
{
    [Header("References for Play Button")]
    [SerializeField] private GameObject spaceShip;
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private Transform camTransform;
    [SerializeField] private RectTransform mainMenuScreen;
    [SerializeField] private GameObject estela;
    [Space]
    [Header("References for Options & Credits")]
    [SerializeField] private RectTransform creditsPanel;
    [SerializeField] private RectTransform optionsPanel;

    private Vector3 targetPositionMainMenu = new Vector3(-200f, 0f, 0f);

    //Panels internal logic
    private bool panelActive = false;
    private Vector3 initialPositionPanels = new Vector3(40f, 0.1452f, 0f);
    private Vector3 targetPositionPanels = new Vector3(12.45f, 0.1452f, 0f);

    public void StartGame()
    {
        creditsPanel.gameObject.SetActive(false);
        optionsPanel.gameObject.SetActive(false);
        mainMenuScreen.DOLocalMove(targetPositionMainMenu, 1f).SetEase(Ease.InOutSine).OnComplete(() => {
            mainMenuScreen.gameObject.SetActive(false);
        });
        estela.SetActive(true);
        StartCoroutine("StartCinematic");
    }

    private IEnumerator StartCinematic()
    {
        yield return camTransform.DOMoveZ(-230.9f, 1.5f).SetEase(Ease.InOutSine).WaitForCompletion();

        camTransform.DOMove(new Vector3(-140f, 11.1f, -48.8f), 3f).SetEase(Ease.InSine);
        camTransform.DORotate(new Vector3(0f, 66.265f, 0f), 3f).SetEase(Ease.InSine);

        spaceShip.transform.DOMove(spaceShip.transform.position + new Vector3(450, 0, 0), 3f).SetEase(Ease.InQuad);
        estela.transform.DOMove(estela.transform.position + new Vector3(400, 0, 0), 3f).SetEase(Ease.InQuad);
        yield return camTransform.DOMove(new Vector3(105.3f, 11.1f, -110.3f), 3f).SetEase(Ease.InQuint).WaitForCompletion();

        SceneManager.LoadScene("Level");
        yield return new WaitForSeconds(0.1f);
    }

    public void CreditsButton()
    {
        if (!panelActive)
        {
            creditsPanel.DOLocalMove(targetPositionPanels, 1f).SetEase(Ease.InOutSine);
            panelActive = true;
        }
        else if (panelActive) 
        {
            creditsPanel.DOLocalMove(initialPositionPanels, 1f).SetEase(Ease.InOutSine);
            panelActive = false;
        }
    }

    public void OptionsButton()
    {
        if (!panelActive)
        {
            optionsPanel.DOLocalMove(targetPositionPanels, 1f).SetEase(Ease.InOutSine);
            panelActive = true;
        }
        else if (panelActive)
        {
            optionsPanel.DOLocalMove(initialPositionPanels, 1f).SetEase(Ease.InOutSine);
            panelActive = false;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
