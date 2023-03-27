using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Data.Characters;

namespace NueGames.EnemyBehaviour
{
    
    public abstract class EnemyActionBase
    {
        protected EnemyActionBase(){}
        public abstract EnemyActionType ActionType { get;}
        public abstract void DoAction(EnemyActionParameters actionParameters);
        
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        
    }
    
    public class EnemyActionParameters
    {
        public readonly float Value;
        public readonly EnemyActionData ActionData;
        public readonly CharacterBase TargetCharacter;
        public readonly CharacterBase SelfCharacter;

        public EnemyActionParameters(float value,CharacterBase target, CharacterBase self, EnemyActionData data)
        {
            Value = value;
            TargetCharacter = target;
            SelfCharacter = self;
            ActionData = data;
        }
    }
    
    
}