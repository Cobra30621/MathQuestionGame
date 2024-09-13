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
    [LabelText("單局存檔資訊")]
    [ShowInInspector] private GameData _gameData;
    [LabelText("永久存檔資訊")]
    [ShowInInspector] private PermanentGameData _permanentGameData;
    [ShowInInspector] [ReadOnly] private List<IDataPersistence> dataPersistenceObjects;
    [ShowInInspector] [ReadOnly] private List<IPermanentDataPersistence> permanentObjects;
    
    
    [LabelText("在 Awake 時，就進行讀檔")]
    [SerializeField] private bool loadOnAwake;

    protected override void DoAtAwake()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        base.DoAtAwake();

        if (loadOnAwake)
        {
            LoadSingleGame();
        }
    }
    
    

    #region Single Game

    [Button("讀檔")]
    public void LoadSingleGame()
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
    [Button("存檔")]
    public void SaveSingleGame()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        // if we don't have any gameData to save, log a warning here
        if (this._gameData == null)
        {
            _gameData = new GameData();
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

  
    private List<IDataPersistence> FindAllDataPersistenceObjects() 
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    
    [Button("清除單局遊戲資料")]
    public void ClearGameData()
    {
        _gameData = new GameData();
        ES3Handler.ClearGameData();
    }

    #endregion

    #region PermanentGame

    
    [Button("清除所有資料")]
    public void ClearAllData()
    {
        _gameData = new GameData();
        _permanentGameData = new PermanentGameData();
        ES3Handler.ClearAllData();
    }
    
    
    public bool IsFirstEnterGame()
    {
        return ES3Handler.IsFirstEnterGame();
    }

    public void SetHaveEnterGame()
    {
        ES3Handler.SetHaveEnterGame();
    }
    
    
    [Button("讀取永久存檔")]
    public void LoadPermanentGame()
    {
        permanentObjects = FindAllPermanentDataPersistenceObjects();
        // load any saved PermanentGameData from a file using the PermanentGameData handler
        this._permanentGameData = ES3Handler.LoadPermanent();

        // start a new game if the PermanentGameData is null and we're configured to initialize it for debugging purposes
        if (this._permanentGameData == null) 
        {
            this._permanentGameData = new PermanentGameData();
        }
    
        // push the loaded PermanentGameData to all other scripts that need it
        foreach (IPermanentDataPersistence permanentObj in permanentObjects) 
        {
            permanentObj.LoadData(_permanentGameData);
        }
    }

    [Button("儲存永久存檔")]
    public void SavePermanentGame()
    {
        permanentObjects = FindAllPermanentDataPersistenceObjects();
        // if we don't have any PermanentGameData to save, log a warning here
        if (this._permanentGameData == null)
        {
            _permanentGameData = new PermanentGameData();
        }

        // pass the PermanentGameData to other scripts so they can update it
        foreach (IPermanentDataPersistence permanentObj in permanentObjects) 
        { 
            permanentObj.SaveData(_permanentGameData);
        }

        // save that PermanentGameData to a file using the PermanentGameData handler
        ES3Handler.SavePermanent(_permanentGameData);
    }

    private List<IPermanentDataPersistence> FindAllPermanentDataPersistenceObjects() 
    {
        IEnumerable<IPermanentDataPersistence> permanentObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IPermanentDataPersistence>();

        return new List<IPermanentDataPersistence>(permanentObjects);
    }
    

    #endregion
    private void OnApplicationQuit() 
    {
        SaveSingleGame();
    }

}
