using UnityEngine;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float floatSpeed = 1f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        transform.LookAt(Camera.main.transform);
    }

    public void SetDamage(float amount)
    {
        damageText.text = amount.ToString();
    }
}
