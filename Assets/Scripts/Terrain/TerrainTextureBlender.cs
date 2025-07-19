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
        int width = terrainData.alphamapWidth;
        int height = terrainData.alphamapHeight;
        int layers = terrainData.alphamapLayers;

        float[,,] alphaMaps = terrainData.GetAlphamaps(0, 0, width, height);
        bool updated = false;

        int rowStep = 10; // Change this to adjust performance. Smaller = smoother.
        float blendSpeed = 0.3f;

        for (int y = 0; y < height; y += rowStep)
        {
            for (int x = 0; x < width; x++)
            {
                for (int dy = 0; dy < rowStep && y + dy < height; dy++)
                {
                    float fromValue = alphaMaps[x, y + dy, textureFrom];
                    float toValue = alphaMaps[x, y + dy, textureTo];

                    float delta = blendSpeed * Time.deltaTime;
                    if (fromValue > 0f)
                    {
                        float newFrom = Mathf.Max(fromValue - delta, 0f);
                        float newTo = toValue + (fromValue - newFrom);

                        alphaMaps[x, y + dy, textureFrom] = newFrom;
                        alphaMaps[x, y + dy, textureTo] = newTo;

                        updated = true;
                    }
                }
            }

            // Apply changes and yield to spread load across frames
            if (updated)
            {
                terrainData.SetAlphamaps(0, 0, alphaMaps);
            }
            yield return null; // wait one frame before next chunk
        }
    }

}
