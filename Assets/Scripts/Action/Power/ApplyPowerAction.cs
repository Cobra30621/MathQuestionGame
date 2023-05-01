using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 賦予能力
    /// </summary>
    public class ApplyPowerAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.ApplyPower;

        public ApplyPowerAction()
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
            
            Target.CharacterStats.ApplyPower(powerType, AdditionValue);
            
            PlayFx();
            PlayAudio();
        }
    }
}