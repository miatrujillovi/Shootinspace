using UnityEngine;
using UnityEngine.UI;

public class ShowMiniMap : MonoBehaviour
{
    private CreateMiniMap crearMiniMapa;
    private RawImage imagenMiniMapa;

    void Start()
    {
        crearMiniMapa = GetComponent<CreateMiniMap>();
        imagenMiniMapa = crearMiniMapa.imagenMiniMapa;
    }

    void Update()
    {
        if (imagenMiniMapa == null) return;

        if (Input.GetKey(KeyCode.Tab))
        {
            imagenMiniMapa.enabled = true;
        }
        else
        {
            imagenMiniMapa.enabled = false;
        }
    }
}
