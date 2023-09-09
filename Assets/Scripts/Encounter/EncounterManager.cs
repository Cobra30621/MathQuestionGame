using System;
using System.Collections.Generic;
using Data;
using DataPersistence;
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
    public class EncounterManager : SingletonDestroyOnLoad<EncounterManager> ,IDataPersistence
    {
        [SerializeField] private ScriptableObjectFileHandler fileHandler;
        
        public MapEncounter mapEncounter;

        public SceneChanger sceneChanger;

        private bool needGenerateNewMapEncounter;
        private EncounterStage _encounterStage;
        
        public void GenerateNewMapEncounter(EncounterStage stage)
        {
            needGenerateNewMapEncounter = true;
            _encounterStage = stage;
            
        }

        #region Enter Room
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
            
            SaveManager.Instance.SaveGame();
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
            
            // 進入戰鬥場景
            sceneChanger.OpenCombatScene();
        }

        #endregion

        #region Data to guid

        public EnemyEncounter GetEnemyEncounter(string guid)
        {
            return fileHandler.GuidToData<EnemyEncounter>(guid);
        }

        public List<string> EnemyEncounterToGuid(List<EnemyEncounter> dataList)
        {
            return fileHandler.DataToGuid(dataList);
        }

        public string GetGuid(EnemyEncounter enemyEncounter)
        {
            return fileHandler.DataToGuid<EnemyEncounter>(enemyEncounter);
        }
        

        #endregion

        #region Save and Load

        public void LoadData(GameData data)
        {
            if (needGenerateNewMapEncounter)
            {
                mapEncounter = new MapEncounter();
                mapEncounter.GeneratorStageData(_encounterStage);
                needGenerateNewMapEncounter = false;
                return;
            }
            
            mapEncounter = data.MapEncounter;
        }

        public void SaveData(GameData data)
        {
            data.MapEncounter = mapEncounter;
        }

        #endregion
    }
}