using UnityEngine;
using Newtonsoft.Json;

public static class SaveSystem
{
    public static void SaveFile<T>(T data, string keyName)
    {
        string json = JsonConvert.SerializeObject(data);
        PlayerPrefs.SetString(keyName, json);
    }

    public static T LoadJson<T>(string keyName)
    {
        string json = PlayerPrefs.GetString(keyName);
        if (!string.IsNullOrWhiteSpace(json))
            return JsonConvert.DeserializeObject<T>(json);

        return default;
    }

    public static bool HasKey(string keyName)
    {
        return PlayerPrefs.HasKey(keyName);
    }

    public static void DeleteSavedFile(string keyName)
    {
        PlayerPrefs.DeleteKey(keyName);
    }
}
