using System.Collections.Generic;
using Action.Parameters;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Managers;
using NueGames.Parameters;

namespace EnemyAbility.EnemyAction
{
    /// <summary>
    /// An abstract base class for enemy actions.
    /// </summary>
    public abstract class EnemyActionBase
    {
        protected EnemyBase _enemy;
        protected EnemyAbility _ability;
        protected EnemySkill _skill;
        protected List<CharacterBase> TargetList;

        /// <summary>
        /// Set the values for the action.
        /// </summary>
        public void SetValue(EnemyBase enemy, EnemySkill enemySkill, List<CharacterBase> targetList)
        {
            _enemy = enemy;
            _ability = enemy.GetAbility();
            _skill = enemySkill;
            TargetList = targetList;
        }

        /// <summary>
        /// Execute the game action.
        /// </summary>
        public void DoAction()
        {
            // Execute effect playback
            DoFXAction();
            // Execute the main game logic
            DoMainAction();
        }

        /// <summary>
        /// Execute the main game logic.
        /// </summary>
        protected abstract void DoMainAction();

        #region Play FX

        /// <summary>
        /// Execute the effects to be played.
        /// </summary>
        protected void DoFXAction()
        {
            GameActionExecutor.AddToBottom(new FXAction(
                new FxInfo(_skill.FxName, _skill.FxSpawnPosition)
                , TargetList));

            if (_skill.UseDefaultAttackFeedback)
            {
                _enemy.PlayDefaultAttackFeedback();
            }

            if (_skill.UseCustomFeedback)
            {
                _enemy.PlayFeedback(_skill.CustomFeedbackKey);
            }
        }

        #endregion

        /// <summary>
        /// Get the source of the action.
        /// </summary>
        protected ActionSource GetActionSource()
        {
            return new ActionSource()
            {
                SourceType = SourceType.Enemy,
                SourceCharacter = _enemy
            };
        }
    }
}
