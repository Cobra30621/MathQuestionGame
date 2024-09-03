using System;
using System.Collections.Generic;
using Data;
using Data.Encounter;
using DataPersistence;
using Map;
using Newtonsoft.Json;
using NueGames.Data.Encounter;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


namespace NueGames.Encounter
{
    public class EncounterManager : Singleton<EncounterManager> ,IDataPersistence
    {
        public MapEncounter mapEncounter;

        public SceneChanger sceneChanger;

        private EncounterStage _encounterStage;

        
        public void GenerateNewMapEncounter(EncounterStage stage)
        {
            _encounterStage = stage;
            
            mapEncounter = new MapEncounter();
            mapEncounter.GeneratorStageData(_encounterStage);
        }

        #region Enter Room
        [Button]
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
            Debug.Log($"EnterNode {nodeType}");
            
            SaveManager.Instance.SaveSingleGame();
        }

        [Button]
        private void EnterCampFire()
        {
            UIManager.Instance.CampFireCanvas.OpenCanvas();
        }

        [Button]
        private void EnterRewardRoom(RewardType rewardType)
        {
            UIManager.Instance.RewardCanvas.ShowReward(new List<RewardType>()
            {
                rewardType
            }, NodeType.Treasure);
        }
        
        [Button]
        private void EnterCombatRoom(EncounterName encounter)
        {
            GameManager.Instance.SetEnemyEncounter(encounter);
            
            // 進入戰鬥場景
            sceneChanger.OpenCombatScene();
        }

        #endregion

        public void OnRoomCompleted()
        {
            MapManager.Instance.OnRoomCompleted();
        }
        

        #region Save and Load

        public void LoadData(GameData data)
        {
            mapEncounter = data.MapEncounter;
        }

        public void SaveData(GameData data)
        {
            data.MapEncounter = mapEncounter;
        }

        #endregion
    }
}