using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    public abstract class GameActionBase
    {
        public GameActionType ActionType { get;}
        public CharacterBase TargetCharacter;
        public CharacterBase SelfCharacter;
        public int Value;
        public float Duration = 0;
        
        public CardActionData CardActionData;

        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;


        public virtual void SetValue(CardActionParameter cardActionParameter){}

        public abstract void DoAction();
    }
}