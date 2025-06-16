using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;
    public static SaveManager Instance => instance;

    private const string SAVE_FILE_NAME = "gamesave.dat";
    private BinaryFormatter formatter;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            formatter = new BinaryFormatter();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        try
        {
            SaveData saveData = new SaveData();
            
            var saveableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();
            foreach (var saveable in saveableObjects)
            {
                saveable.Save(saveData);
            }

            using (FileStream stream = new FileStream(GetSavePath(), FileMode.Create))
            {
                formatter.Serialize(stream, saveData);
            }

            Debug.Log("Game saved successfully!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving game: {e.Message}");
        }
    }

    public void LoadGame()
    {
        Debug.Log("Game load is called");
        try
        {
            if (!File.Exists(GetSavePath()))
            {
                Debug.LogWarning("No save file found!");
                return;
            }

            SaveData saveData;
            using (FileStream stream = new FileStream(GetSavePath(), FileMode.Open))
            {
                saveData = (SaveData)formatter.Deserialize(stream);
            }

            var saveableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();
            foreach (var saveable in saveableObjects)
            {
                saveable.Load(saveData);
            }

            Debug.Log("Game loaded successfully!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading game: {e.Message}");
        }
    }

    public bool HasSaveFile()
    {
        return File.Exists(GetSavePath());
    }

    public DateTime GetLastSaveTime()
    {
        if (HasSaveFile())
        {
            try
            {
                using (FileStream stream = new FileStream(GetSavePath(), FileMode.Open))
                {
                    SaveData saveData = (SaveData)formatter.Deserialize(stream);
                    return saveData.saveDateTime;
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        return DateTime.MinValue;
    }

    private string GetSavePath()
    {
        Debug.Log(Application.persistentDataPath);
        return Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
    }
} 