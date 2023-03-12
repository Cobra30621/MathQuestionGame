using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;

namespace NueGames.NueDeck.Scripts.Card
{
    public class CardActionParameters
    {
        public readonly int Value;
        public readonly CharacterBase TargetCharacter;
        public readonly CharacterBase SelfCharacter;
        public readonly CardActionData CardActionData;
        public readonly CardData CardData;
        
        public CardActionParameters(int value,CharacterBase target, CharacterBase self,CardActionData cardActionData, CardData cardData)
        {
            Value = value;
            TargetCharacter = target;
            SelfCharacter = self;
            CardActionData = cardActionData;
            CardData = cardData;
        }
    }
}