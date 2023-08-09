using System;
using System.Collections.Generic;
using Map;
using Newtonsoft.Json;
using NueGames.Data.Encounter;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace NueGames.Encounter
{
    public class EncounterManager : MonoBehaviour
    {
        public MapManager mapManager;

        public MapEncounter mapEncounter;

        public EncounterGenerator encounterGenerator;
        public SceneChanger sceneChanger;
        
        #region 單例模式

        public static EncounterManager Instance;
        
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            } 
            else
            {
                Instance = this;
            }
        }
        

        #endregion
        
        public void GenerateNewMapEncounter(EncounterStage stage)
        {
            mapEncounter = new MapEncounter();
            mapEncounter.GeneratorStageData(stage);
            
            Debug.Log(mapEncounter.ToJson());
        }


        public void LoadEncounter()
        {
            var mapJson = PlayerPrefs.GetString("MapEncounter");
            var mapEncounter = JsonConvert.DeserializeObject<MapEncounter>(mapJson);

            this.mapEncounter = mapEncounter;
        }

        public void SaveEncounter()
        {
            var json = mapEncounter.ToJson();
            PlayerPrefs.SetString("MapEncounter", json);
            PlayerPrefs.Save();
        }


        public void EnterNode(NodeType nodeType)
        {
            switch (nodeType)
            {
                case NodeType.MinorEnemy:
                    EnterCombatRoom(mapEncounter.GetEnemyEncounter());
                    break;
                case NodeType.EliteEnemy:
                    EnterCombatRoom(mapEncounter.GetEliteEncounter());
                    break;
                case NodeType.CampFire:
                    EnterCampFire();
                    break;
                case NodeType.Treasure:
                    EnterRewardRoom(RewardType.Card);
                    break;
                case NodeType.Store:
                    break;
                case NodeType.Boss:
                    EnterCombatRoom(mapEncounter.GetBossEncounter());
                    break;
                case NodeType.Mystery:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void EnterCampFire()
        {
            UIManager.Instance.CampFireCanvas.OpenCanvas();
        }

        private void EnterRewardRoom(RewardType rewardType)
        {
            UIManager.Instance.RewardCanvas.ShowReward(new List<RewardType>()
            {
                rewardType
            });
        }
        
        private void EnterCombatRoom(EnemyEncounter encounter)
        {
            GameManager.Instance.SetEnemyEncounter(encounter);
            SaveEncounter();
            
            // 進入戰鬥場景
            sceneChanger.OpenCombatScene();
        }


        public EnemyEncounter GetEnemyEncounter(EnemyEncounterName encounterName)
        {
            return encounterGenerator.GetEnemyEncounter(encounterName);
        }
    }
}