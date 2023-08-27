using NueGames.Action;
using NueGames.Managers;
using NueGames.Power;

namespace EnemyAbility.EnemyAction
{
    /// <summary>
    /// Represents an enemy action that grants strength to targets.
    /// </summary>
    public class GainStrength : EnemyActionBase
    {
        /// <summary>
        /// The value of strength to be gained.
        /// </summary>
        public int value;

        /// <summary>
        /// Executes the main action of gaining strength.
        /// </summary>
        protected override void DoMainAction()
        {
            GameActionExecutor.AddToBottom(new ApplyPowerAction(
                value, PowerName.Strength, TargetList, GetActionSource()));
        }
    }
}