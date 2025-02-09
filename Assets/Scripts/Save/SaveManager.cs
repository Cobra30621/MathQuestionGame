using System.Collections.Generic;
using System.Linq;
using Log;
using Managers;
using Newtonsoft.Json;
using Save.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using VersionControl;

namespace Save
{
    /// <summary>
    /// 參考影片：https://www.youtube.com/watch?v=aUi9aijvpgs
    /// </summary>
    public class SaveManager : SerializedMonoBehaviour
    {
        public static SaveManager Instance => GameManager.Instance.SaveManager;
    
    
        [LabelText("單局存檔資訊")]
        [ShowInInspector] private GameData _gameData;
        [LabelText("永久存檔資訊")]
        [ShowInInspector] private PermanentGameData _permanentGameData;
        [ShowInInspector] [ReadOnly] private List<IDataPersistence> dataPersistenceObjects;
        [ShowInInspector] [ReadOnly] private List<IPermanentDataPersistence> permanentObjects;
    
        void Awake()
        {
            dataPersistenceObjects = FindAllDataPersistenceObjects();

        }
    
        [Button("清除所有資料")]
        public void ClearAllData()
        {
            _gameData = new GameData();
            _permanentGameData = new PermanentGameData();
            ES3Handler.ClearAllData();
            
            EventLogger.Instance.LogEvent(LogEventType.Save, "清除 - 所有資料");
        }
    

        #region 單局遊戲

        /// <summary>
        /// 是否有正在遊玩中的單局遊戲
        /// </summary>
        /// <returns></returns>
        public bool HasOngoingGame()
        {
            var hasOngoingGame = ES3Handler.LoadHasOngoingGame();
            
            return hasOngoingGame;
        }

        public void SetOngoingGame()
        {
            ES3Handler.SetHasOngoingGame();
        }
        
        
        [Button("讀檔")]
        public void LoadSingleGame()
        {
            dataPersistenceObjects = FindAllDataPersistenceObjects();
            _gameData = ES3Handler.LoadSingleGame();

            // 檢查是否有存檔資料
            if (this._gameData == null ) 
            {
                this._gameData = new GameData();
            }
            else
            {
                // 如果存檔資料版本不相容，轉換
                _gameData = SaveVersionHandler.ConvertGameDataIfNeeded(
                    _permanentGameData.saveVersion, _gameData);
            }
        
            // push the loaded Skill to all other scripts that need it
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
            {
                dataPersistenceObj.LoadData(_gameData);
            }
            
            EventLogger.Instance.LogEvent(LogEventType.Save, "讀取 - 單局遊戲資料", 
                $"{JsonConvert.SerializeObject(_gameData, Formatting.Indented)}");
        }
        [Button("存檔")]
        public void SaveSingleGame()
        {
            dataPersistenceObjects = FindAllDataPersistenceObjects();
            if (this._gameData == null)
            {
                _gameData = new GameData();
            }

            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
            { 
                dataPersistenceObj.SaveData(_gameData);
            }

            ES3Handler.SaveSingleGame(_gameData);
            
            EventLogger.Instance.LogEvent(LogEventType.Save, "儲存 - 單局遊戲資料", 
                $"{JsonConvert.SerializeObject(_gameData, Formatting.Indented)}");
        }

  
        private List<IDataPersistence> FindAllDataPersistenceObjects() 
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
                .OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }
    
        [Button("清除單局遊戲資料")]
        public void ClearSingleGameData()
        {
            _gameData = new GameData();
            ES3Handler.ClearSingleGameData();
            EventLogger.Instance.LogEvent(LogEventType.Save, "清除 - 單局遊戲資料");
        }

        #endregion

        #region 永久資料
        public bool IsFirstEnterGame()
        {
            return ES3Handler.IsFirstEnterGame();
        }

        public void SetHaveEnteredGame()
        {
            EventLogger.Instance.LogEvent(LogEventType.Main, "紀錄 - 已經進入過遊戲");
            ES3Handler.SetHaveEnteredGame();
        }
    
    
        [Button("讀取永久存檔")]
        public void LoadPermanentGame()
        {
            permanentObjects = FindAllPermanentDataPersistenceObjects();
            _permanentGameData = ES3Handler.LoadPermanent();
            
            // 檢查是否有存檔資料
            if (_permanentGameData == null) 
            {
                _permanentGameData = new PermanentGameData();
            }
            else
            {
                // 如果存檔資料版本不相容，轉換
                _permanentGameData = SaveVersionHandler.ConvertPermanentDataIfNeeded(
                    _permanentGameData.saveVersion, _permanentGameData);
            }
            
            // push the loaded PermanentGameData to all other scripts that need it
            foreach (IPermanentDataPersistence permanentObj in permanentObjects) 
            {
                permanentObj.LoadData(_permanentGameData);
            }
            
             EventLogger.Instance.LogEvent(LogEventType.Save, "讀取 - 永久存檔", 
                            $"{JsonConvert.SerializeObject(_permanentGameData, Formatting.Indented)}");
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
            
            EventLogger.Instance.LogEvent(LogEventType.Save, "儲存 - 永久存檔", 
                            $"{JsonConvert.SerializeObject(_permanentGameData, Formatting.Indented)}");

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
    }
}
