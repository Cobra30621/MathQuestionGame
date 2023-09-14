using System.Linq;
using NueGames.Encounter;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace Map
{
    public class MapManager : Singleton<MapManager>
    {
        [ReadOnly]
        [SerializeField] private MapConfig[] _maps;

        private int CurrentMapIndex;

        public Map CurrentMap;

        public UnityEvent<Map> showMapEvent;

        public bool needInitializedMap;


        // 初始化地圖管理器，設定地圖配置數組
        public void Initialized(MapConfig[] maps)
        {
            _maps = maps;
            CurrentMapIndex = 0;
            needInitializedMap = true;
        }

        // 初始化地圖
        public void InitializedMap()
        {
            GenerateNewMap();
            needInitializedMap = false;
        }

        // 生成新的地圖
        [Button]
        public void GenerateNewMap()
        {
            // 檢查當前地圖索引是否有效
            if (CurrentMapIndex < 0 || CurrentMapIndex >= _maps.Length)
            {
                Debug.LogError($"Invalid CurrentMapIndex: {CurrentMapIndex}");
                return;
            }

            // 根據當前地圖配置生成新地圖
            var mapConfig = _maps[CurrentMapIndex];
            var map = MapGenerator.GetMap(mapConfig);
            CurrentMap = map;
            Debug.Log(map.ToJson());

            // 生成新地圖遭遇
            EncounterManager.Instance.GenerateNewMapEncounter(mapConfig.encounterStage);
            showMapEvent.Invoke(map);
        }

        // 生成下一張地圖
        public void GenerateNextMap()
        {
            CurrentMapIndex++;
            GenerateNewMap();
        }

        // 檢查是否為最後一個房間
        [Button]
        public bool IsLastRoom()
        {
            var isLastRoom = CurrentMap.path.Any(p => p.Equals(CurrentMap.GetBossNode().point));
            return isLastRoom;
        }

        // 檢查是否為最後一個地圖
        [Button]
        public bool IsLastMap()
        {
            var isLastMap = CurrentMapIndex == _maps.Length - 1;
            return isLastMap;
        }
    }
}