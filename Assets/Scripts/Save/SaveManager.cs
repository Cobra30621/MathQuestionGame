using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Data;
using DataPersistence;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;


/// <summary>
/// 參考影片：https://www.youtube.com/watch?v=aUi9aijvpgs
/// </summary>
public class SaveManager : SerializedMonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    

    [SerializeField] private List<string> needSaveScenes;

    [ShowInInspector] private GameData _gameData;
    [ShowInInspector] [ReadOnly] private List<IDataPersistence> dataPersistenceObjects;
    
    public static SaveManager Instance { get; private set; }

    private void Awake() 
    {
        if (Instance != null) 
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

    }

    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        Debug.Log($"needSaveScenes.Contains(scene.name) {needSaveScenes.Contains(scene.name)}");
        if (needSaveScenes.Contains(scene.name))
        {
            LoadGame();
        }
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log($"needSaveScenes.Contains(scene.name) {needSaveScenes.Contains(scene.name)}");
        if (needSaveScenes.Contains(scene.name))
        {
            SaveGame();
        }
    }

    
    [Button]
    public void NewGame()
    {
        _gameData = new GameData();
        ES3Handler.Clear();
    }

    public void LoadGame()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        Debug.Log("Load Game");
        // load any saved data from a file using the data handler
        // this._gameData = dataHandler.Load();
        this._gameData = ES3Handler.Load();

        // start a new game if the data is null and we're configured to initialize data for debugging purposes
        if (this._gameData == null && initializeDataIfNull) 
        {
            this._gameData = new GameData();
        }

        // if no data can be loaded, don't continue
        if (this._gameData == null) 
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        Debug.Log($"Save Game");
        // if we don't have any data to save, log a warning here
        if (this._gameData == null) 
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.SaveData(_gameData);
        }

        // save that data to a file using the data handler
        ES3Handler.Save(_gameData);
        // dataHandler.Save(_gameData);
    }

    private void OnApplicationQuit() 
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() 
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

}
