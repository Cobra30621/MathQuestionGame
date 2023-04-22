using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;


namespace NueGames.Action
{
    /// <summary>
    /// 清除能力
    /// </summary>
    public class ClearPowerAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.ClearPower;
        private PowerType powerType;

        public ClearPowerAction()
        {
            FxType = FxType.Buff;
            AudioActionType = AudioActionType.Power;
        }

        
        public override void SetValue(ActionParameters parameters)
        {
            ActionData data = parameters.ActionData;
            powerType = data.PowerType;
            Target = parameters.TargetCharacter;
            Duration = parameters.ActionData.ActionDelay;
            
            HasSetValue = true;
        }
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void DoAction()
        {
            CheckHasSetValue();
            if (IsTargetNull()) return;
            
            Target.CharacterStats.ClearPower(powerType);
            
            PlayFx();
            PlayAudio();
        }
    }
}