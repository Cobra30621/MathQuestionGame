using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;
using Question;

namespace NueGames.Action.MathAction
{
    /// <summary>
    /// 根據答對數，造成傷害
    /// </summary>
    public class DamageByQuestioningAction : ByQuestioningActionBase
    {
        public override GameActionType ActionType => GameActionType.DamageByQuestioning;
        private DamageInfo damageInfo;
        
        public override void SetValue(ActionParameters parameters)
        {
            ActionData data = parameters.ActionData;
            Duration = parameters.ActionData.ActionDelay;

            SetValue(new DamageInfo(data.ActionValue, parameters.SelfCharacter),
                parameters.TargetCharacter);
        }
        
        public void SetValue(DamageInfo info, CharacterBase target)
        {
            damageInfo = info;
            Target = target;
            baseValue = info.Value;

            HasSetValue = true;
        }
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void DoAction()
        {
            CheckHasSetValue();
            answerCount = QuestionManager.Instance.CorrectAnswerCount;
            damageInfo.Value  = GetAddedValue();
            
            DamageAction gameActionBase = new DamageAction();
            gameActionBase.SetValue(damageInfo, Target);
            GameActionExecutor.AddToBottom(gameActionBase);
        }

    }
}