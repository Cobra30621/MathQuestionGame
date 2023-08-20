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
public class SaveManager : Singleton<SaveManager>
{
    
    [SerializeField] private List<string> needSaveDataScenes;
    [SerializeField] private List<string> needLoadDataScenes;

    [ShowInInspector] private GameData _gameData;
    [ShowInInspector] [ReadOnly] private List<IDataPersistence> dataPersistenceObjects;
    

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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        // Debug.Log($"Load scene {scene.name}, {needSaveDataScenes.Contains(scene.name)}");
        if (needLoadDataScenes.Contains(scene.name))
        {
            LoadGame();
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        // Debug.Log($"Unload scene {scene.name}, {needSaveDataScenes.Contains(scene.name)}");
        if (needSaveDataScenes.Contains(scene.name))
        {
            SaveGame();
        }
    }

    
    [Button]
    public void ClearGameData()
    {
        _gameData = new GameData();
        ES3Handler.Clear();
    }

    public void LoadGame()
    {
        // load any saved data from a file using the data handler
        // this._gameData = dataHandler.Load();
        this._gameData = ES3Handler.Load();

        // start a new game if the data is null and we're configured to initialize data for debugging purposes
        if (this._gameData == null ) 
        {
            this._gameData = new GameData();
        }
        
        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
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
