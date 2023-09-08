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
        public abstract int DamageValueForIntention { get; }
        public abstract bool IsDamageAction { get; }


        protected EnemyBase _enemy;
        protected EnemyAbility Ability;
        protected EnemySkill Skill;
        protected List<CharacterBase> TargetList;

        /// <summary>
        /// Set the values for the action.
        /// </summary>
        public void SetValue(EnemyBase enemy, EnemySkill enemySkill, List<CharacterBase> targetList)
        {
            _enemy = enemy;
            Ability = enemy.GetAbility();
            Skill = enemySkill;
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
                new FxInfo(Skill.FxGo, Skill.FxSpawnPosition)
                , TargetList));

            if (Skill.UseDefaultAttackFeedback)
            {
                _enemy.PlayDefaultAttackFeedback();
            }

            if (Skill.UseCustomFeedback)
            {
                _enemy.PlayFeedback(Skill.CustomFeedbackKey);
            }
        }

        #endregion

        /// <summary>
        /// Get the source of the action.
        /// </summary>
        public ActionSource GetActionSource()
        {
            return new ActionSource()
            {
                SourceType = SourceType.Enemy,
                SourceCharacter = _enemy
            };
        }
    }
}
