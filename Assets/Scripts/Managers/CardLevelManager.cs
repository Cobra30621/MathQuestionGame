using System;
using System.Collections.Generic;
using Card.Data;
using Cinemachine;
using Data;
using NueGames.Data.Collection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class CardLevelManager : Singleton<CardLevelManager>,IDataPersistence
    {
        // TEST
        [SerializeField] public String cardName;
        [SerializeField] public int level;
        // TEST
        public Dictionary<string, int> cardLevels;
        public DeckData SaveCard;
        [Button("初始化字典")]
        public void InitDictionary()
        {
            cardLevels = new Dictionary<string, int>();
            foreach (var cardData in SaveCard.CardList)
            {
                var cardId = cardData.CardId;
                cardLevels[cardId] = 1;
            }
        }
        
        // TEST
        [Button("AddCardLevel")]
        public void AddCardLevel()
        {
            cardLevels[cardName] = level;
        }
        
        public int GetCardLevel(string id)
        {
            return cardLevels.ContainsKey(id) ? cardLevels[id] : 1;
        }
        public void LoadData(GameData data)
        {
            cardLevels = data.cardLevels;

        }

        public void SaveData(GameData data)
        {
            data.cardLevels = cardLevels;
        }
        
        
    }
}