using NueGames.Action;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Combat;
using NueGames.Parameters;
using System.Collections.Generic;
using NueGames.Power;

namespace NueGames.Relic.Common
{
    /// <summary>
    /// 答對一題，獲得一層護盾
    /// </summary>
    public class ManaShieldRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.MathShield;


        public override void SubscribeAllEvent()
        {
            QuestionManager.OnAnswerCorrect += OnAnswerCorrect;
        }

        public override void UnSubscribeAllEvent()
        {
            QuestionManager.OnAnswerCorrect -= OnAnswerCorrect;
        }
        
        // protected override void OnAnswerCorrect(TurnInfo info)
        // {
        //     if (info.CharacterType == GetOwnerCharacterType())
        //     {
        //         ApplyPowerAction action = new ApplyPowerAction();
        //         action.SetPowerActionValue(1, 
        //             PowerName.Block, 
        //             new List<CharacterBase>() {Owner},
        //             GetActionSource()
        //         );
        //         
        //         GameActionExecutor.AddToBottom(action);
        //     }
        // }
        
    }
}