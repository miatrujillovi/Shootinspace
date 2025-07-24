using UnityEngine;

public class FuelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (LevelManager.Instance.hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            if (LevelManager.Instance.isFuelUnlocked)
            {
                LevelManager.Instance.hasTriggered = true;
                LevelManager.Instance.FuelRecovered();

                //gameObject.SetActive(false);
            }
        }
    }
}
