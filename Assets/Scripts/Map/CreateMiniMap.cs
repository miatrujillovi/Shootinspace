using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreateMiniMap : MonoBehaviour
{
    public RawImage imagenMiniMapa;
    [SerializeField] private Camera camaraMiniMapa;
    [SerializeField] private int resolucion = 256;

    void Start()
    {
        StartCoroutine(CapturarMinimapa());
    }

    private IEnumerator CapturarMinimapa()
    {
        yield return new WaitForEndOfFrame();

        RenderTexture rt = new RenderTexture(resolucion, resolucion, 24);
        camaraMiniMapa.targetTexture = rt;

        camaraMiniMapa.Render();

        RenderTexture.active = rt;
        Texture2D imagenCapturada = new Texture2D(resolucion, resolucion, TextureFormat.RGB24, false);
        imagenCapturada.ReadPixels(new Rect(0, 0, resolucion, resolucion), 0, 0);
        imagenCapturada.Apply();

        if (imagenMiniMapa != null)
        {
            imagenMiniMapa.texture = imagenCapturada;
        }

        camaraMiniMapa.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        camaraMiniMapa.gameObject.SetActive(false);
    }
}
