using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    private string fileName; //= "gameData";

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than 1 DataPersistenceManager in the scene.");
        }
        Instance = this;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameEndedActivity += OnGameEnded;
    }

    private void Start()
    {
        fileName = MenuManager.Instance.gameMode + "ModeData";

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();    
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void OnHomeButtonClicked()
    {
        SaveGame();
    }

    public void OnGameEnded()
    {
        dataHandler.RemoveFile();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.LoadFromFile();

        if (this.gameData == null)
        {
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        dataHandler.SaveToFile(gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
