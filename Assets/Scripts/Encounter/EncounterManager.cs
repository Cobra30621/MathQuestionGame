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
using UnityEngine.Serialization;
using Utils;

namespace Encounter
{
    /// <summary>
    /// 管理地圖遭遇
    /// </summary>
    public class EncounterManager : MonoBehaviour, IDataPersistence
    {
        /// <summary>
        /// 目前的敵人遭遇名稱
        /// </summary>
        public EncounterName currentEnemyEncounter;
        
        public MapEncounter mapEncounter;

        public SceneChanger sceneChanger;
        
        public EncounterStage EncounterStage;

        public static EncounterManager Instance  => GameManager.Instance != null ? GameManager.Instance.EncounterManager : null;

        
        public void GenerateNewMapEncounter(EncounterStage stage)
        {
            SetEncounterStage(stage);
            
            mapEncounter = new MapEncounter();
            mapEncounter.GeneratorStageData(EncounterStage);
        }


        #region Enter Room
        [Button]
        public void EnterNode(NodeType nodeType)
        {
            switch (nodeType)
            {
                case NodeType.MinorEnemy:
                    EnterCombatRoom(mapEncounter.GetEnemyEncounter(EncounterStage));
                    break;
                case NodeType.EliteEnemy:
                    EnterCombatRoom(mapEncounter.GetEliteEncounter(EncounterStage));
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
                    EnterCombatRoom(mapEncounter.GetBossEncounter(EncounterStage));
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

        public void OnCompletedCombatRoom()
        {
            var currentNodeType = MapManager.Instance.GetCurrentNodeType();
            
            mapEncounter.CompleteRoom(currentNodeType, currentEnemyEncounter);
            
            OnRoomCompleted();
        }
        
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

        /// <summary>
        /// 設定本層地圖的敵人遭遇可能清單
        /// </summary>
        /// <param name="stage"></param>
        public void SetEncounterStage(EncounterStage stage)
        {
            EncounterStage = stage;
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