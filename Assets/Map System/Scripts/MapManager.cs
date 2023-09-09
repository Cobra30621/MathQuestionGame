using System.Linq;
using Data;
using UnityEngine;
using Newtonsoft.Json;
using NueGames.Encounter;
using NueGames.Managers;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine.Events;

namespace Map
{
    public class MapManager : Singleton<MapManager>
    {
        [ReadOnly]
        [SerializeField] private MapConfig[] maps;
        private int CurrentMapIndex => GameManager.Instance.CurrentMapIndex;
        
        public Map CurrentMap;

        public UnityEvent<Map> showMap;

        public bool needInitializedMap;
        

        public void Initialized(MapConfig[] maps)
        {
            this.maps = maps;
            GameManager.Instance.ResetCurrentMapIndex();
            needInitializedMap = true;
        }

        public void InitializedMap()
        {
            Debug.Log("InitializedMap()");
            GenerateNewMap();
            needInitializedMap = false;
        }
        
        [Button]
        public void GenerateNewMap()
        {
            if(CurrentMapIndex < 0 || CurrentMapIndex >= maps.Length) {
                Debug.LogError($"{new System.ArgumentOutOfRangeException(nameof(CurrentMapIndex))}");
                return;
            }
            
            var mapConfig = maps[CurrentMapIndex];
            var map = MapGenerator.GetMap(mapConfig);
            CurrentMap = map;
            Debug.Log(map.ToJson());

            EncounterManager.Instance.GenerateNewMapEncounter(mapConfig.encounterStage);
            showMap.Invoke(map);
        }

       
        
        
        [Button]
        public bool IsLastRoom()
        {
            var lastRoom = CurrentMap.path.Any(p => p.Equals(CurrentMap.GetBossNode().point));
            Debug.Log($"lastRoom{lastRoom}");
            return lastRoom;
        }

        [Button]
        public bool IsLastMap()
        {
            var lastMap = CurrentMapIndex == maps.Length - 1;
            Debug.Log($"lastMap{lastMap}");
            return lastMap;
        }
    }
}
