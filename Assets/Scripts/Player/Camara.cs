using DG.Tweening.Core.Easing;
using UnityEngine;

public class Camara : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Camera playerCamera;

    private float rotationSensibility;

    private float cameraVerticalAngle;
    private Vector3 rotationinput = Vector3.zero;

    private void Awake()
    {
        playerCamera = Camera.main;
        rotationSensibility = PlayerPrefs.GetFloat("MouseSensitivity", 100f);
    }

    private void Update()
    {
        if (AudioManager.Instance.IsInMenu == false)
        {
            Look();
        }
    }

    private void Look()
    {
        rotationinput.x = Input.GetAxis("Mouse X") * rotationSensibility * Time.deltaTime;
        rotationinput.y = Input.GetAxis("Mouse Y") * rotationSensibility * Time.deltaTime;

        cameraVerticalAngle = cameraVerticalAngle + rotationinput.y;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -60, 60);

        transform.Rotate(Vector3.up * rotationinput.x);
        playerCamera.transform.localRotation = Quaternion.Euler(-cameraVerticalAngle, 0f, 0f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}