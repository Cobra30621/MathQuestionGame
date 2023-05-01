using NueGames.Card;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;
using Question;
using UnityEngine;

namespace NueGames.Action.MathAction
{
    public class DamageByAnswerCountInThisBattleAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.DamageByAnswerCountInThisBattle;
        
        public override void DoAction()
        {
            CheckHasSetValue();
            multiplierAmount = QuestionManager.Instance.GetAnswerCountInThisCombat(answerOutcomeType);
     
            DoDamageAction();
        }
    }
}