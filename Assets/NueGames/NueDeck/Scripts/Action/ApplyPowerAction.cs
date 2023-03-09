using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    public class ApplyPowerAction : GameActionBase
    {
        private PowerType powerType; 

        public override void SetValue(CardActionParameter cardActionParameter)
        {
            CardActionData data = cardActionParameter.CardActionData;
            TargetCharacter = cardActionParameter.TargetCharacter;
            Duration = cardActionParameter.CardActionData.ActionDelay;
            powerType = data.PowerType;
            Value = data.ActionValue;
        }
        
        public override void DoAction()
        {
            if (!TargetCharacter) return;
            
            TargetCharacter.CharacterStats.ApplyStatus(powerType,Mathf.RoundToInt(Value));
            
            if (FxManager != null) 
                FxManager.PlayFx(TargetCharacter.transform, FxType.Str);

            if (AudioManager != null) 
                AudioManager.PlayOneShot(AudioActionType.Poison);
        }
    }
}