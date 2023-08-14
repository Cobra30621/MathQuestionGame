using System.Linq;
using Data;
using UnityEngine;
using Newtonsoft.Json;
using NueGames.Encounter;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Map
{
    public class MapManager : SerializedMonoBehaviour, IDataPersistence
    {
        public MapConfig config;
        public MapView view;
        public EncounterManager encounterManager;

        public Map CurrentMap;

        public void GenerateNewMap()
        {
            var map = MapGenerator.GetMap(config);
            CurrentMap = map;
            Debug.Log(map.ToJson());

            encounterManager.GenerateNewMapEncounter(config.encounterStage);
            view.ShowMap(map);
        }

        // public void SaveMap()
        // {
        //     if (CurrentMap == null) return;
        //
        //     var json = JsonConvert.SerializeObject(CurrentMap, Formatting.Indented,
        //         new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
        //     PlayerPrefs.SetString("Map", json);
        //     PlayerPrefs.Save();
        //     
        //     encounterManager.SaveEncounter();
        // }


        public void LoadData(GameData data)
        {
            CurrentMap = data.Map;

            Debug.Log($"Load CurrentMap {CurrentMap}");
            if (CurrentMap == null)
            {
                GenerateNewMap();
            }
            
            if (CurrentMap.path.Any(p => p.Equals(CurrentMap.GetBossNode().point)))
            {
                // payer has already reached the boss, generate a new map
                GenerateNewMap();
            }
            else
            {
                // player has not reached the boss yet, load the current map
                view.ShowMap(CurrentMap);
            }
        }

        public void SaveData(GameData data)
        {
            Debug.Log($"Save Map {CurrentMap}");
            data.Map = CurrentMap;
        }
    }
}
