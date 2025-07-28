using System.IO;
using UnityEngine;

public static class SettingsManager
{
    private static string filePath => Path.Combine(Application.persistentDataPath, "settings.json");

    public static void Save(GameSettings settings)
    {
        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(filePath, json);
    }

    public static GameSettings Load()
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                GameSettings loaded = JsonUtility.FromJson<GameSettings>(json);

                if (loaded != null)
                {
                    bool modified = false;

                    if (loaded.musicVolume == 0f) { loaded.musicVolume = 1f; modified = true; }
                    if (loaded.sfxVolume == 0f) { loaded.sfxVolume = 1f; modified = true; }
                    if (loaded.mouseSensitivity == 0f) { loaded.mouseSensitivity = 500f; modified = true; }

                    if (modified)
                    {
                        Debug.Log("Archivo incompleto, se actualizaron campos por defecto.");
                        Save(loaded);
                    }

                    Debug.Log("Configuración cargada desde archivo.");
                    return loaded;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al cargar settings: {e}");
        }

        GameSettings defaultSettings = new GameSettings();
        Save(defaultSettings);
        return defaultSettings;
    }



    public static void Delete()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
