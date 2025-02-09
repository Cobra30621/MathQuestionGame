using System;
using System.Collections.Generic;
using Encounter.Data;
using Managers;
using Map;
using Reward;
using Reward.Data;
using Save;
using Save.Data;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using Utils;

namespace Encounter
{
    /// <summary>
    /// 管理地圖遭遇
    /// </summary>
    public class EncounterManager : MonoBehaviour,IDataPersistence
    {
        /// <summary>
        /// 目前的敵人遭遇名稱
        /// </summary>
        public EncounterName currentEnemyEncounter;
        
        public MapEncounter mapEncounter;

        public SceneChanger sceneChanger;

        private EncounterStage _encounterStage;

        public static EncounterManager Instance => GameManager.Instance.EncounterManager;

        
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
                    EnterRewardRoom(new List<RewardData>()
                    {
                        new()
                        {
                            RewardType = RewardType.Card,
                            ItemGainType =  ItemGainType.Character
                        }
                    });
                    break;
                case NodeType.Store:
                    break;
                case NodeType.Boss:
                    EnterCombatRoom(mapEncounter.GetBossEncounter());
                    break;
                case NodeType.Event:
                    EnterEventRoom();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [Button]
        private void EnterCampFire()
        {
            UIManager.Instance.CampFireCanvas.OpenCanvas();
        }

        [Button]
        private void EnterRewardRoom(List<RewardData> rewards)
        {
            UIManager.Instance.RewardCanvas.ShowReward(rewards, NodeType.Treasure);
        }
        
        [Button]
        private void EnterCombatRoom(EncounterName encounter)
        {
            SetEnemyEncounter(encounter);
            
            // 進入戰鬥場景
            StartCoroutine(sceneChanger.OpenCombatScene());
        }

        [Button]
        private void EnterEventRoom()
        {
            UIManager.Instance.EventCanvas.OpenCanvas();
        }
        
        #endregion

        public void OnRoomCompleted()
        {      
            MapManager.Instance.OnRoomCompleted();
            SaveManager.Instance.SaveSingleGame();
        }
        
        /// <summary>
        /// 設定敵人遭遇
        /// </summary>
        /// <param name="encounter"></param>
        public void SetEnemyEncounter(EncounterName encounter)
        {
            currentEnemyEncounter  = encounter;
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