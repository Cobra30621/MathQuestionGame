using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    public class ApplyPowerAction : GameActionBase
    {
        private PowerType powerType;

        public ApplyPowerAction()
        {
            FxType = FxType.Buff;
            AudioActionType = AudioActionType.Power;
        }
        
        public override void SetValue(CardActionParameters parameters)
        {
            CardActionData data = parameters.CardActionData;
            Duration = parameters.CardActionData.ActionDelay;
            
            SetValue(data.PowerType, data.ActionValue, parameters.TargetCharacter);
        }

        public void SetValue(PowerType applyPower, int powerValue, CharacterBase target)
        {
            powerType = applyPower;
            Amount = powerValue;
            Target = target;

            HasSetValue = true;
        }
        
        public override void DoAction()
        {
            CheckHasSetValue();
            if (IsTargetNull()) return;
            
            Target.CharacterStats.ApplyPower(powerType,Mathf.RoundToInt(Amount));
            
            PlayFx();
            PlayAudio();
        }
    }
}