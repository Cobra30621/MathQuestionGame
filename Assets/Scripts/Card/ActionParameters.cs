using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Managers;

namespace NueGames.Card
{
    public class ActionParameters
    {
        public readonly int Value;
        public readonly CharacterBase TargetCharacter;
        public readonly CharacterBase SelfCharacter;
        public readonly ActionData ActionData;
        public readonly CardData CardData;
        
        public ActionParameters(int value,CharacterBase target, CharacterBase self,ActionData actionData, CardData cardData)
        {
            Value = value;
            TargetCharacter = target;
            SelfCharacter = self;
            ActionData = actionData;
            CardData = cardData;
        }
    }
}