using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirtyPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirtyPath, string dataFileName)
    {
        this.dataDirtyPath = dataDirtyPath;
        this.dataFileName = dataFileName;
    }

    public GameData LoadFromFile()
    {
        string fullPath = Path.Combine(dataDirtyPath, dataFileName);
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
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void SaveToFile(GameData data)
    {
        string fullPath = Path.Combine(dataDirtyPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);
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
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public void RemoveFile()
    {
        string fullPath = Path.Combine(dataDirtyPath, dataFileName);
        if (File.Exists(fullPath))
        {
            try
            {
                File.Delete(fullPath);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to remove data file: " + fullPath + "\n" + e);
            }
        }
    }
}
