using System.Collections.Generic;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Managers;
using NueGames.Power;

namespace Question.QuestionAction
{
    public class KnightAbility : QuestionActionBase
    {
        public int BlockValue;
        
        public override void DoCorrectAction()
        {
            GameActionExecutor.AddToBottom(new ApplyPowerAction(
                BlockValue, PowerName.Block, 
                new List<CharacterBase>(){CombatManager.Instance.MainAlly}, GetActionSource()));
        }

        public override void DoWrongAction()
        {
            
        }
    }
}