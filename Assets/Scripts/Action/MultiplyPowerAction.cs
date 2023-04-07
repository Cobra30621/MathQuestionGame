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
        private PowerType powerType;

        public MultiplyPowerAction()
        {
            FxType = FxType.Buff;
            AudioActionType = AudioActionType.Power;
        }

        

        public override void SetValue(ActionParameters parameters)
        {
            ActionData data = parameters.ActionData;
            Duration = parameters.ActionData.ActionDelay;
            
            SetValue(data.PowerType, data.ActionValue, parameters.TargetCharacter);
        }

        public void SetValue(PowerType applyPower, int powerValue, CharacterBase target)
        {
            powerType = applyPower;
            Amount = powerValue;
            Target = target;

            HasSetValue = true;
        }
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void DoAction()
        {
            CheckHasSetValue();
            if (IsTargetNull()) return;
            
            Target.CharacterStats.MultiplyPower(powerType,Mathf.RoundToInt(Amount));
            
            PlayFx();
            PlayAudio();
        }
    }
}