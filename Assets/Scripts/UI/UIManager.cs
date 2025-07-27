using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject exileTXT;
    [SerializeField] private GameObject reloadTXT;
    [SerializeField] private GameObject fuelTXT;
    [SerializeField] private GameObject destierroIcon;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); //Destroying duplicated Instance
            return;
        }
    }

    public void ExileEnemy()
    {
        destierroIcon.SetActive(true);
        exileTXT.SetActive(true);
    }

    public void HideExileEnemy()
    {
        destierroIcon.SetActive(false);
        exileTXT.SetActive(false);
    }

    public void NeedReload()
    {
        reloadTXT.SetActive(true);
    }

    public void HideReload()
    {
        reloadTXT.SetActive(false);
    }

    public void CollectFuel()
    {
        fuelTXT.SetActive(true);
    }

    public void HideFuel()
    {
        fuelTXT.SetActive(false);
    }
}
