using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using DG.Tweening;
using System.Collections;

public class ScenesManager : MonoBehaviour
{
    [SerializeField] private GameObject spaceShip;
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private Transform camTransform;
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject estela;
 
    public void StartGame()
    {
        mainMenuScreen.SetActive(false);
        estela.SetActive(true);
        StartCoroutine("StartCinematic");
    }

    private IEnumerator StartCinematic()
    {
        camTransform.DOMoveZ(-230.9f, 3f).SetEase(Ease.InOutSine);

        camTransform.DOMove(new Vector3(-140f, 11.1f, -48.8f), 3f).SetEase(Ease.InSine);
        camTransform.DORotate(new Vector3(0f, 66.265f, 0f), 3f).SetEase(Ease.InSine);

        spaceShip.transform.DOMove(spaceShip.transform.position + new Vector3(450, 0, 0), 3f).SetEase(Ease.InQuad);
        estela.transform.DOMove(estela.transform.position + new Vector3(400, 0, 0), 3f).SetEase(Ease.InQuad);
        yield return camTransform.DOMove(new Vector3(105.3f, 11.1f, -110.3f), 3f).SetEase(Ease.InQuint).WaitForCompletion();

        SceneManager.LoadScene("Level");
        yield return new WaitForSeconds(0.1f);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
