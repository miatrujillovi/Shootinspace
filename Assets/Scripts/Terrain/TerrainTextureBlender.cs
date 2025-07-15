using UnityEngine;

public class TerrainTextureBlender : MonoBehaviour
{
    [SerializeField] private Terrain terrain;
    [SerializeField][Range(0f, 1f)] private float blendSpeed = 0.3f;
    [SerializeField] private int textureFrom; //Index of the current floor texture
    [SerializeField] private int textureTo; //Index of the texture to blend towards

    private float[,,] alphaMaps;
    private int width;
    private int height;
    private int layers;

    private void Start()
    {
        TerrainData terrainData = terrain.terrainData;
        width = terrainData.alphamapWidth;
        height = terrainData.alphamapHeight;
        layers = terrainData.alphamapLayers;

        alphaMaps = terrainData.GetAlphamaps(0, 0, width, height);
    }

    public void BlendTextures()
    {
        bool updated = false;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float fromValue = alphaMaps[x, y, textureFrom];
                float toValue = alphaMaps[x, y, textureTo];

                float delta = blendSpeed * Time.deltaTime;
                if (fromValue > 0f)
                {
                    float newFrom = Mathf.Max(fromValue - delta, 0f);
                    float newTo = toValue + (fromValue - newFrom);

                    alphaMaps[x, y, textureFrom] = newFrom;
                    alphaMaps[x, y, textureTo] = newTo;

                    updated = true;
                }
            }
        }

        if (updated)
        {
            terrain.terrainData.SetAlphamaps(0, 0, alphaMaps);
        }
    }
}
