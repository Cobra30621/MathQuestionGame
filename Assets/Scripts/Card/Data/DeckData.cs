using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NSubstitute.Core;
using NueGames.Data.Collection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Card.Data
{
    [CreateAssetMenu(fileName = "Deck Data", menuName = "Collection/Deck", order = 1)]
    public class DeckData : ScriptableObject
    {
        [ValueDropdown("GetAssets", IsUniqueList = false)]
        [InlineEditor]
        [SerializeField] private List<CardData> cardList;
        public List<CardData> CardList => cardList;


        public CardData GetCard(string id)
        {
            var cardData = CardList.First(card => card.CardId == id);

            return cardData;
        }
        
        

#if UNITY_EDITOR // Editor-related code must be excluded from builds
        private IEnumerable GetAssets()
        {
            return AssetGetter.GetAssets(AssetGetter.DataName.Card);
        }
#endif
    }
}