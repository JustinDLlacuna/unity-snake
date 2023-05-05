using System.IO;
using UnityEngine;

public class SaveLoader
{
    public static void SaveData<T>(T data, string fileName) where T : new()
    {
        string path = Application.persistentDataPath + "/" + fileName;
        string saveDataJson = JsonUtility.ToJson(data);
        File.WriteAllText(path, saveDataJson);
    }

    public static T LoadData<T>(string fileName) where T : new()
    {
        T saveData = new T();
          
        string path = Application.persistentDataPath + "/" + fileName;
        
        if (File.Exists(path))
        {
            string saveDataJson = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(saveDataJson, saveData);
        }
       
        return saveData;
    }
}
