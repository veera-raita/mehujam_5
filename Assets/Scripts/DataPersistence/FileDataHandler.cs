using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string _dataDirPath, string _dataFileName)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
        if (dataFileName == "")
        {
            dataFileName = "default";
        }
    }

    public void DeleteData()
    {
        //using path.combine to account for differing file systems
        string fullPath = Path.Combine(dataDirPath, "saves", dataFileName);
        File.Delete(fullPath);
    }

    public GameData Load()
    {
        //using path.combine to account for differing file systems
        string fullPath = Path.Combine(dataDirPath, "saves", dataFileName);
        Debug.Log(message: $"Loading data from {fullPath}");
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError(message: $"Error occured while trying to load data from file: {fullPath}\n{e}");
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        //using path.combine to account for differing file systems
        string fullPath = Path.Combine(dataDirPath, "saves", dataFileName);

        try
        {
            //creating directory for saves if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            //serialize GameData object into a JSON file
            string dataToStore = JsonUtility.ToJson(data, true);
            //write data to file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(message: $"Error occured while trying to save data to file: {fullPath}'\n{e}");
        }
    }
}