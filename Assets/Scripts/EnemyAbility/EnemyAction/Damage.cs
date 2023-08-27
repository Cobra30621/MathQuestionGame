using NueGames.Action;
using NueGames.Managers;

namespace EnemyAbility.EnemyAction
{
    /// <summary>
    /// Represents an enemy action that inflicts damage to targets.
    /// </summary>
    public class Damage : EnemyActionBase
    {
        /// <summary>
        /// The value of damage to be inflicted.
        /// </summary>
        public int damageValue;

        /// <summary>
        /// Executes the main action of causing damage.
        /// </summary>
        protected override void DoMainAction()
        {
            GameActionExecutor.AddToBottom(new DamageAction(
                damageValue, TargetList, GetActionSource()));
        }
    }
}