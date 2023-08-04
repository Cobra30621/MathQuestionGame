using System;
using System.Collections.Generic;
using Managers;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Data.Encounter;
using NueGames.Relic;
using UnityEngine;

namespace NueGames.Data.Settings
{
    [Serializable]
    public class PersistentGameplayData
    {
        private readonly GameplayData _gameplayData;
        
        [SerializeField] private int currentGold;
        [SerializeField] private int drawCount;
        [SerializeField] private int maxMana;
        [SerializeField] private int currentMana;
        [SerializeField] private bool canUseCards;
        [SerializeField] private bool canSelectCards;
        [SerializeField] private AllyBase mainAlly;
        [SerializeField] private int currentStageId;
        [SerializeField] private int currentEncounterId;
        [SerializeField] private bool isFinalEncounter;
        [SerializeField] private List<CardData> currentCardsList;
        [SerializeField] private List<AllyHealthData> allyHealthDataDataList;
        /// <summary>
        /// 玩家持有的遺物
        /// </summary>
        [SerializeField] private List<RelicClip> currentRelicList;
        /// <summary>
        /// 現在戰鬥敵人
        /// </summary>
        [SerializeField] private EnemyEncounter currentEnemyEncounter;

        public PersistentGameplayData(GameplayData gameplayData)
        {
            _gameplayData = gameplayData;

            InitData();
        }
        
        public void SetAllyHealthData(string id,int newCurrentHealth, int newMaxHealth)
        {
            var data = allyHealthDataDataList.Find(x => x.CharacterId == id);
            var newData = new AllyHealthData();
            newData.CharacterId = id;
            newData.CurrentHealth = newCurrentHealth;
            newData.MaxHealth = newMaxHealth;
            if (data != null)
            {
                allyHealthDataDataList.Remove(data);
                allyHealthDataDataList.Add(newData);
            }
            else
            {
                allyHealthDataDataList.Add(newData);
            }
        } 
        private void InitData()
        {
            DrawCount = _gameplayData.DrawCount;
            MaxMana = _gameplayData.MaxMana;
            CurrentMana = MaxMana;
            CanUseCards = true;
            CanSelectCards = true;
            MainAlly = _gameplayData.InitialAlly;
            CurrentEncounterId = 0;
            CurrentStageId = 0;
            CurrentGold = 0;
            CurrentCardsList = new List<CardData>();
            IsFinalEncounter = false;
            allyHealthDataDataList = new List<AllyHealthData>();
            currentRelicList = new List<RelicClip>();
        }

        #region Encapsulation

        public int DrawCount
        {
            get => drawCount;
            set => drawCount = value;
        }

        public int MaxMana
        {
            get => maxMana;
            set => maxMana = value;
        }

        public int CurrentMana
        {
            get => currentMana;
            set => currentMana = value;
        }

        public bool CanUseCards
        {
            get => canUseCards;
            set => canUseCards = value;
        }

        public bool CanSelectCards
        {
            get => canSelectCards;
            set => canSelectCards = value;
        }


        public AllyBase MainAlly
        {
            get => mainAlly;
            set => mainAlly = value;
        }

        public int CurrentStageId
        {
            get => currentStageId;
            set => currentStageId = value;
        }

        public int CurrentEncounterId
        {
            get => currentEncounterId;
            set => currentEncounterId = value;
        }

        public bool IsFinalEncounter
        {
            get => isFinalEncounter;
            set => isFinalEncounter = value;
        }

        public List<CardData> CurrentCardsList
        {
            get => currentCardsList;
            set => currentCardsList = value;
        }

        public List<AllyHealthData> AllyHealthDataList
        {
            get => allyHealthDataDataList;
            set => allyHealthDataDataList = value;
        }
        public int CurrentGold
        {
            get => currentGold;
            set => currentGold = value;
        }

        public List<RelicClip> CurrentRelicList
        {
            get => currentRelicList;
            set => currentRelicList = value;
        }

        public EnemyEncounter CurrentEnemyEncounter
        {
            get => currentEnemyEncounter;
            set => currentEnemyEncounter = value;
        }
        
        #endregion
    }
}