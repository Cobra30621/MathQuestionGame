using System.Collections;
using System.Collections.Generic;
using NueGames.Data.Collection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Card.Data
{
    [CreateAssetMenu(fileName = "Deck Data", menuName = "Collection/Deck", order = 1)]
    public class DeckData : ScriptableObject
    {
        [SerializeField] private string deckId;
        [SerializeField] private string deckName;
        
        [ValueDropdown("GetAssets", IsUniqueList = false)]
        [InlineEditor]
        [SerializeField] private List<CardData> cardList;
        public List<CardData> CardList => cardList;
        

        public string DeckId => deckId;

        public string DeckName => deckName;

#if UNITY_EDITOR // Editor-related code must be excluded from builds
        private IEnumerable GetAssets()
        {
            return AssetGetter.GetAssets(AssetGetter.DataName.Card);
        }
#endif
    }
}