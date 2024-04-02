using System.Collections.Generic;
using Data;
using NueGames.Data.Collection;
using Sirenix.OdinInspector;

namespace Managers
{
    public class CardLevelManager : Singleton<CardLevelManager>,IDataPersistence
    {
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

        public int GetCardLevel(string id)
        {
            return cardLevels.ContainsKey(id) ? cardLevels[id] : 1;
        }
        public void LoadData(GameData data)
        {
            // throw new System.NotImplementedException();
        }

        public void SaveData(GameData data)
        {
            // throw new System.NotImplementedException();
        }
        
        
    }
}