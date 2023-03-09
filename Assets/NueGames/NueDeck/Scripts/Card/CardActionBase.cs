using NueGames.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;

namespace NueGames.NueDeck.Scripts.Card
{
    public class CardActionParameter
    {
        public readonly int Value;
        public readonly CharacterBase TargetCharacter;
        public readonly CharacterBase SelfCharacter;
        public readonly CardActionData CardActionData;
        
        public CardActionParameter(int value,CharacterBase target, CharacterBase self,CardActionData cardData)
        {
            Value = value;
            TargetCharacter = target;
            SelfCharacter = self;
            CardActionData = cardData;
        }
    }
    
    public class CardActionParameters
    {
        public readonly float Value;
        public readonly CharacterBase TargetCharacter;
        public readonly CharacterBase SelfCharacter;
        public readonly CardData CardData;
        public readonly CardBase CardBase;
        public PowerType PowerType;
        
        public CardActionParameters(float value,CharacterBase target, CharacterBase self,CardData cardData, CardBase cardBase, PowerType powerType)
        {
            Value = value;
            TargetCharacter = target;
            SelfCharacter = self;
            CardData = cardData;
            CardBase = cardBase;
            PowerType = powerType;
        }
    }
    public abstract class CardActionBase
    {
        protected CardActionBase(){}
        public abstract GameActionType ActionType { get;}
        public abstract void DoAction(CardActionParameters actionParameters);
        
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        
    }
    
    
   
}