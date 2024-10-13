using Card;
using NueGames.Action;

namespace Action.Enemy
{
    public class SplitEnemyAction : GameActionBase
    {
        private readonly string _spawnEnemyId;

        public SplitEnemyAction(SkillInfo skillInfo)
        {
            _spawnEnemyId = $"{skillInfo.EffectParameterList[0]}";
        }
        
        protected override void DoMainAction()
        {
            var characterHandler = CombatManager.characterHandler;

            var enemy = TargetList[0];
            var hp = enemy.GetHealth();
            enemy.SetDeath();
            
            for (int i = 0; i < 2; i++)
            {
                characterHandler.BuildAndSetEnemyHealth(_spawnEnemyId, hp);
            }
        }
    }
}