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
}
