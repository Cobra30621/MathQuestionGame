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
        
        public void SetValue(int value, CharacterBase target, PowerType powerType)
        {
            BaseValue = value;
            Target = target;
            PowerType = powerType;
        }
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            if (IsTargetNull()) return;
            
            Target.CharacterStats.ApplyPower(PowerType, AdditionValue);
            
            PlayFx(FxType.Buff, Target.transform);
        }
    }
}