using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Data.Collection
{
    [CreateAssetMenu(fileName = "Deck Data", menuName = "NueDeck/Collection/Deck", order = 1)]
    public class DeckData : ScriptableObject
    {
        [SerializeField] private string deckId;
        [SerializeField] private string deckName;
        
        [ValueDropdown("GetAssets", IsUniqueList = true)]
        [InlineEditor]
        [SerializeField] private List<CardData> cardList;
        public List<CardData> CardList => cardList;

        public string DeckId => deckId;

        public string DeckName => deckName;

        private string cardDataPath = "";

#if UNITY_EDITOR // Editor-related code must be excluded from builds
        private IEnumerable GetAssets()
        {
            return AssetGetter.GetAssets(AssetGetter.DataName.Card);
        }
#endif
    }
}