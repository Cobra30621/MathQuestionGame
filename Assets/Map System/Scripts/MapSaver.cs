using Data;
using Newtonsoft.Json;
using NueGames.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// 負責儲存地圖資料
    /// </summary>
    public class MapSaver : SerializedMonoBehaviour, IDataPersistence
    {
        private MapManager _manager => MapManager.Instance;
        
        public void LoadData(GameData data)
        {
            // 如果需要初始化（開新的遊戲），便建立新的地圖
            if (_manager.needInitializedMap)
            {
                _manager.InitializedMap();
                return;
            }
            
            _manager.CurrentMap = JsonConvert.DeserializeObject<Map>(data.MapJson, 
                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            // 為本地圖的最後房間，換下一張地圖後，產生新地圖
            if (_manager.IsLastRoom())
            {
                _manager.GenerateNextMap();
            }
            else
            {
                // 請 UI 顯示地圖資料
                _manager.showMapEvent.Invoke(_manager.CurrentMap);
            }
        }
        
        public void SaveData(GameData data)
        {
            Debug.Log($"CurrentMap {_manager.CurrentMap.path.Count}");
            var json = JsonConvert.SerializeObject(_manager.CurrentMap, Formatting.Indented,
                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            
            data.MapJson = json;
        }

    }
}