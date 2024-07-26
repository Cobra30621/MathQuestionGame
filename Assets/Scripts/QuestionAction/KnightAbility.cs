using System.Collections.Generic;
using Action.Parameters;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Managers;
using NueGames.Power;

namespace Question.QuestionAction
{
    public class KnightAbility : QuestionActionBase
    {
        public FxInfo CorrectFx;
        public int BlockValue;
        
        public override void DoCorrectAction()
        {
            GameActionExecutor.AddAction(new ApplyPowerAction(
                BlockValue, PowerName.Block, 
                new List<CharacterBase>(){CombatManager.Instance.MainAlly}, GetActionSource()));
            
            // GameActionExecutor.AddAction(new FXAction(CorrectFx, 
            // new List<CharacterBase>(){CombatManager.Instance.MainAlly}));
        }

        public override void DoWrongAction()
        {
            
        }
    }
}