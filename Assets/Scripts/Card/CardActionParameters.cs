using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Managers;

namespace NueGames.Card
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