using System.Linq;
using Encounter;
using Managers;
using Map_System.Scripts.MapData;
using Newtonsoft.Json;
using Save;
using Save.Data;
using UnityEngine;
using Sirenix.OdinInspector;
using Stage;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Map
{
    public class MapManager : SerializedMonoBehaviour, IDataPersistence
    {
        public StageName stageName;
        
        [Required]
        public StageDataOverview stageDataOverview;
        
        public StageData stageData;
        
        private int _currentMapIndex;

        public Map CurrentMap;

        public UnityEvent<Map> showMapEvent;

        public bool needInitializedMap;

        public MapNode selectedNode;

        public bool Locked;

        public static MapManager Instance => GameManager.Instance.MapManager;

        [SerializeField] private Canvas canvas;
        
        
        private void Awake()
        {
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (scene.name == "1- Map")
                {
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
        public void Initialized(StageName stageName)
        {
            this.stageName = stageName;
            stageData = stageDataOverview.FindUniqueId(this.stageName.Id);
            _currentMapIndex = 0;
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
            if (_currentMapIndex < 0 || _currentMapIndex >= stageData.maps.Count)
            {
                Debug.LogError($"Invalid CurrentMapIndex: {_currentMapIndex}");
                return;
            }

            // 根據當前地圖配置生成新地圖
            var mapConfig = stageData.maps[_currentMapIndex];
            var map = MapGenerator.GetMap(mapConfig);
            CurrentMap = map;

            // 生成新地圖遭遇
            EncounterManager.Instance.GenerateNewMapEncounter(mapConfig.encounterStage);
            showMapEvent.Invoke(map);
        }

        // 生成下一張地圖
        public void GenerateNextMap()
        {
            _currentMapIndex++;
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
            var isLastMap = _currentMapIndex == stageData.maps.Count() - 1;
            return isLastMap;
        }
        
        public void LoadData(GameData data)
        {
            CurrentMap = JsonConvert.DeserializeObject<Map>(data.MapJson, 
                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            _currentMapIndex = data.CurrentMapIndex;

            stageName = new StageName();
            stageName.SetId(data.StageName);
            stageData = stageDataOverview.FindUniqueId(stageName.Id);
        }
        
        public void SaveData(GameData data)
        {
            var json = JsonConvert.SerializeObject(CurrentMap, Formatting.Indented,
                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            
            data.MapJson = json;
            data.CurrentMapIndex = _currentMapIndex;
            data.StageName = stageName.Id;
        }
    }
}