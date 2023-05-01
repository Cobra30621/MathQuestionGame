using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;


namespace NueGames.Action
{
    public class MultiplyPowerAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.MultiplyPower;

        public MultiplyPowerAction()
        {
            FxType = FxType.Buff;
            AudioActionType = AudioActionType.Power;
        }
        
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void DoAction()
        {
            CheckHasSetValue();
            if (IsTargetNull()) return;
            
            Target.CharacterStats.MultiplyPower(powerType, AdditionValue);
            
            PlayFx();
            PlayAudio();
        }
    }
}