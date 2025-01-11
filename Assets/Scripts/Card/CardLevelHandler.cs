using System;
using System.Collections.Generic;
using System.Linq;
using Card.Data;
using Save;
using Save.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Card
{
    /// <summary>
    /// Handles card level and gain status persistence.
    /// </summary>
    public class CardLevelHandler : SerializedMonoBehaviour, IPermanentDataPersistence
    {
        // TEST
        [ShowInInspector] public Dictionary<string, CardSaveInfo> cardSaveInfos { get; private set; }
        public DeckData SaveCard;

        private bool haveInitDict = false;

        /// <summary>
        /// Initializes the cardSaveInfos dictionary with card IDs from SaveCard.
        /// </summary>
        [Button("Initialize Dictionary")]
        public void InitDictionary()
        {
            cardSaveInfos = new Dictionary<string, CardSaveInfo>();
            foreach (var cardData in SaveCard.CardList)
            {
                var cardId = cardData.CardId;
                cardSaveInfos[cardId] = new CardSaveInfo { Level = 0, HasGained = false };
            }

            haveInitDict = true;
        }

        /// <summary>
        /// Upgrades the card level by 1 for the given card ID.
        /// </summary>
        public void UpgradeCard(string id)
        {
            CheckHaveInitDict();
            bool contain = cardSaveInfos.TryGetValue(id, out var cardSaveLevel);

            Debug.Log($"Upgrade card {id} to {cardSaveLevel.Level + 1}");
            if (contain)
            {
                cardSaveLevel.Level++;
            }

            SaveManager.Instance.SavePermanentGame();
        }

        /// <summary>
        /// Marks the card as gained for the given card ID.
        /// </summary>
        public void OnGainCard(string id)
        {
            CheckHaveInitDict();
            cardSaveInfos.TryGetValue(id, out var cardSaveLevel);

            if (cardSaveLevel != null && !cardSaveLevel.HasGained)
            {
                cardSaveLevel.HasGained = true;
                SaveManager.Instance.SavePermanentGame();
            }
        }

        /// <summary>
        /// Retrieves the save info for the given card ID.
        /// </summary>
        public CardSaveInfo GetSaveInfo(string id)
        {
            CheckHaveInitDict();
            return cardSaveInfos.TryGetValue(id, out var cardSaveLevel) ? cardSaveLevel : new CardSaveInfo();
        }

        private void CheckHaveInitDict()
        {
            if(haveInitDict)return;
            
            Debug.LogError($"{nameof(CardLevelHandler)} have not init dictionary");
        }

        /// <summary>
        /// Sets the save info for all cards.
        /// </summary>
        public void SetSaveInfo(Dictionary<string, CardSaveInfo> saveInfos)
        {
            cardSaveInfos = saveInfos;
            haveInitDict = true;
        }

        /// <summary>
        /// Loads the card save info from the given PermanentGameData.
        /// </summary>
        public void LoadData(PermanentGameData data)
        {
            InitDictionary();

            var loadCardLevels = data.cardLevels;
            if (loadCardLevels != null)
            {
                // SaveDeck content takes precedence
                foreach (var cardId in cardSaveInfos.Keys.ToList())
                {
                    if (loadCardLevels.ContainsKey(cardId))
                    {
                        cardSaveInfos[cardId] = loadCardLevels[cardId];
                    }
                }
            }
        }

        /// <summary>
        /// Saves the card save info to the given PermanentGameData.
        /// </summary>
        public void SaveData(PermanentGameData data)
        {
            data.cardLevels = cardSaveInfos;
        }
    }

    
}