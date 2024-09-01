using System;
using System.Collections.Generic;
using System.Linq;
using Card.Data;
using Data;
using NueGames.Data.Collection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Card
{
    public class CardLevelHandler : SerializedMonoBehaviour, IDataPersistence
    {
        // TEST
        public Dictionary<string, CardSaveLevel> cardLevels;
        public DeckData SaveCard;

        
        [Button("Initialize Dictionary")]
        public void InitDictionary()
        {
            cardLevels = new Dictionary<string, CardSaveLevel>();
            foreach (var cardData in SaveCard.CardList)
            {
                var cardId = cardData.CardId;
                cardLevels[cardId] = new CardSaveLevel { Level = 0, HasGained = false };
            }
        }

        public void UpgradeCard(string id)
        {
            bool contain = cardLevels.TryGetValue(id, out var cardSaveLevel);

            Debug.Log($"Upgrade card {id} to {cardSaveLevel.Level + 1}");
            if (contain)
            {
                cardSaveLevel.Level++;
            }

            SaveManager.Instance.SaveGame();
        }

        public void OnGainCard(string id)
        {
            bool contain = cardLevels.TryGetValue(id, out var cardSaveLevel);

            Debug.Log($"Gain card {id}");
            if (contain)
            {
                cardSaveLevel.HasGained = true;
            }
            
            SaveManager.Instance.SaveGame();
        }

        public CardSaveLevel GetCardLevel(string id)
        {
            return cardLevels.TryGetValue(id, out var cardSaveLevel) ? cardSaveLevel : new CardSaveLevel();
        }


        public void LoadData(GameData data)
        {
            InitDictionary();

            var loadCardLevels = data.cardLevels;
            if (loadCardLevels != null)
            {
                // SaveDeck content takes precedence
                foreach (var cardId in cardLevels.Keys.ToList())
                {
                    if (loadCardLevels.ContainsKey(cardId))
                    {
                        cardLevels[cardId] = loadCardLevels[cardId];
                    }
                }
            }
        }

        public void SaveData(GameData data)
        {
            data.cardLevels = cardLevels;
        }
    }
    
    [Serializable]
    public class CardSaveLevel
    {
        [LabelText("等級")]
        public int Level;
        [LabelText("已獲取過")]
        public bool HasGained;
    }

}