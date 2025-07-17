using System.Collections;
using UnityEngine;

public class TerrainTextureBlender : MonoBehaviour
{
    private float blendSpeed = 0.3f;
    private float[,,] alphaMaps;
    private int width;
    private int height;
    private int layers;

    public IEnumerator BlendTexturesRoutine(int textureFrom, int textureTo, Terrain terrain)
    {
        TerrainData terrainData = terrain.terrainData;
        width = terrainData.alphamapWidth;
        height = terrainData.alphamapHeight;
        layers = terrainData.alphamapLayers;

        alphaMaps = terrainData.GetAlphamaps(0, 0, width, height);

        bool blending = true;

        while (blending)
        {
            blending = false;

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

                        blending = true;
                    }
                }
            }

            terrain.terrainData.SetAlphamaps(0, 0, alphaMaps);
            yield return null;
        }
    }
}
