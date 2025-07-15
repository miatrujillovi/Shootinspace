using UnityEngine;

public class FuelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (LevelManager.Instance.isFuelUnlocked)
            {
                LevelManager.Instance.FuelRecovered();
            }
        }
    }
}
