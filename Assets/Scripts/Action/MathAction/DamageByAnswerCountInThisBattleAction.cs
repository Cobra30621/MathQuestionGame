using NueGames.Card;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;
using Question;
using UnityEngine;

namespace NueGames.Action.MathAction
{
    public class DamageByAnswerCountInThisBattleAction : ByQuestioningActionBase
    {
        public override GameActionType ActionType => GameActionType.DamageByAnswerCountInThisBattle;
        private DamageInfo damageInfo;

        public override void SetValue(ActionParameters parameters)
        {
            ActionData data = parameters.ActionData;
            Duration = parameters.ActionData.ActionDelay;
            
            baseValue = data.ActionValue;
            _answerOutcomeType = data.AnswerOutcomeType;

            damageInfo = new DamageInfo(baseValue,  parameters.SelfCharacter);
            Target = parameters.TargetCharacter;

            HasSetValue = true;
        }

        public override void DoAction()
        {
            CheckHasSetValue();
            answerCount = QuestionManager.Instance.GetAnswerCountInThisCombat(_answerOutcomeType);
            damageInfo.Value  = GetAddedValue();
            
            DamageAction gameActionBase = new DamageAction();
            gameActionBase.SetValue(damageInfo, Target);
            GameActionExecutor.AddToBottom(gameActionBase);
        }
    }
}