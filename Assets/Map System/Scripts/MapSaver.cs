using Data;
using Newtonsoft.Json;
using NueGames.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Map
{
    public class MapSaver : SerializedMonoBehaviour, IDataPersistence
    {
        private MapManager _manager => MapManager.Instance;
        
        
        public void LoadData(GameData data)
        {
            if (_manager.needInitializedMap)
            {
                _manager.InitializedMap();
                return;
            }
            
            _manager.CurrentMap = JsonConvert.DeserializeObject<Map>(data.MapJson, 
                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            if (_manager.IsLastRoom())
            {
                // payer has already reached the boss, generate a new map
                GameManager.Instance.AddCurrentMapIndex();
                _manager.GenerateNewMap();
            }
            else
            {
                // player has not reached the boss yet, load the current map
                _manager.showMap.Invoke(_manager.CurrentMap);
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