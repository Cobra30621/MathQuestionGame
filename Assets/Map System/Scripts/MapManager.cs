using System.Linq;
using Data;
using Map_System.Scripts.MapData;
using Newtonsoft.Json;
using NueGames.Encounter;
using UnityEngine;
using Sirenix.OdinInspector;
using Stage;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Map
{
    public class MapManager : Singleton<MapManager>, IDataPersistence
    {

        public StageData stageData;
        
        private int CurrentMapIndex;

        public Map CurrentMap;

        public UnityEvent<Map> showMapEvent;

        public bool needInitializedMap;

        public MapNode selectedNode;

        public bool Locked;

        protected override void DoAtAwake()
        {
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                Debug.Log("Load Scene");
                if (scene.name == "1- Map")
                {
                    Debug.Log("Show Map");
                    ShowMap();
                }
            };
        }
        private void ShowMap()
        {
            Locked = false;
            // 如果需要初始化（開新的遊戲），便建立新的地圖
            if (needInitializedMap)
            {
                Debug.Log("initialized");
                InitializedMap();
                return;
            }
            
            // 為本地圖的最後房間，換下一張地圖後，產生新地圖
            if (IsLastRoom())
            {
                GenerateNextMap();
            }
            else
            {
                // 請 UI 顯示地圖資料
                showMapEvent.Invoke(CurrentMap);
            }
        }

        public void SelectedNode(MapNode node)
        {
            selectedNode = node;
        }

        public NodeType GetCurrentNodeType()
        {
            return selectedNode.Node.nodeType;
        }
        
        public void OnRoomCompleted()
        {
            CurrentMap.path.Add(selectedNode.Node.point);
            Locked = false;
        }
        


        // 初始化地圖管理器，設定地圖配置數組
        public void Initialized(StageData stage)
        {
            stageData = stage;
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
            if (CurrentMapIndex < 0 || CurrentMapIndex >= stageData.maps.Count)
            {
                Debug.LogError($"Invalid CurrentMapIndex: {CurrentMapIndex}");
                return;
            }

            // 根據當前地圖配置生成新地圖
            var mapConfig = stageData.maps[CurrentMapIndex];
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
            var isLastMap = CurrentMapIndex == stageData.maps.Count() - 1;
            return isLastMap;
        }
        
        public void LoadData(GameData data)
        {
            CurrentMap = JsonConvert.DeserializeObject<Map>(data.MapJson, 
                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
        }


        
        
        public void SaveData(GameData data)
        {
            Debug.Log($"CurrentMap {CurrentMap.path.Count}");
            var json = JsonConvert.SerializeObject(CurrentMap, Formatting.Indented,
                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            
            data.MapJson = json;
        }
    }
}