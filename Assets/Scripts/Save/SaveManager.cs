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
    [ShowInInspector] private GameData _gameData;
    [ShowInInspector] [ReadOnly] private List<IDataPersistence> dataPersistenceObjects;


    protected override void DoAtAwake()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        base.DoAtAwake();
    }
    
    [Button]
    public void ClearGameData()
    {
        _gameData = new GameData();
        ES3Handler.Clear();
    }

    public void LoadGame()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        // load any saved Skill from a file using the Skill handler
        // this._gameData = dataHandler.Load();
        this._gameData = ES3Handler.Load();

        // start a new game if the Skill is null and we're configured to initialize Skill for debugging purposes
        if (this._gameData == null ) 
        {
            this._gameData = new GameData();
        }
        
        // push the loaded Skill to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        // if we don't have any gameData to save, log a warning here
        if (this._gameData == null) 
        {
            Debug.LogWarning("No gameData was found. A New Game needs to be started before gameData can be saved.");
            return;
        }

        // pass the Skill to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        { 
            // Debug.Log($"dataPersistenceObj {dataPersistenceObj}");
            dataPersistenceObj.SaveData(_gameData);
        }

        // save that Skill to a file using the Skill handler
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
