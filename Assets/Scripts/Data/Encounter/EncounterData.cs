using System;
using System.Collections.Generic;
using System.Linq;
using NueGames.Characters;
using NueGames.Data.Characters;
using NueGames.Enums;
using NueGames.NueExtentions;
using UnityEngine;

namespace NueGames.Data.Encounter
{
    [CreateAssetMenu(fileName = "Encounter Data", menuName = "Map/EncounterData", order = 4)]
    public class EncounterData : ScriptableObject
    {
        [Header("Settings")] 
        [SerializeField] private bool encounterRandomlyAtStage;
        [SerializeField] private List<EnemyEncounterStage> enemyEncounterList;

        public bool EncounterRandomlyAtStage => encounterRandomlyAtStage;
        public List<EnemyEncounterStage> EnemyEncounterList => enemyEncounterList;

        public EnemyEncounter GetEnemyEncounter(int stageId = 0,int encounterId =0,bool isFinal = false)
        {
            // var selectedStage = EnemyEncounterList.First(x => x.StageId == stageId);
            // if (isFinal) return selectedStage.BossEncounterList.RandomItem().Encounter;
            //
            // return EncounterRandomlyAtStage
            //     ? selectedStage.EnemyEncounterList.RandomItem().Encounter
            //     : selectedStage.EnemyEncounterList[encounterId].Encounter ?? 
            //       selectedStage.EnemyEncounterList.RandomItem().Encounter;

            // TODO 刪除
            return null;
        }
        
    }


    /// <summary>
    /// 一層地圖的資料
    /// </summary>
    [Serializable]
    public class EnemyEncounterStage
    {
        [SerializeField] private string name;
        [SerializeField] private int stageId;
        [SerializeField] private List<EnemyEncounterClip> enemyEncounterList;
        [SerializeField] private List<EnemyEncounterClip> bossEncounterList;
        
        
        public string Name => name;
        public int StageId => stageId;

        /// <summary>
        /// 較弱的一般敵人(出現在地圖前半)
        /// </summary>
        public List<EnemyEncounterClip> EnemyEncounterList => enemyEncounterList;
        /// <summary>
        /// Boss 戰
        /// </summary>
        public List<EnemyEncounterClip> BossEncounterList => bossEncounterList;
    }


}