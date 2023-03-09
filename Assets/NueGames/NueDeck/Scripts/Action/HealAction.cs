using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    public class HealAction : GameActionBase
    {
        public HealAction()
        {
            FxType = FxType.Heal;
            AudioActionType = AudioActionType.Heal;
        }
        
        public override void SetValue(CardActionParameter cardActionParameter)
        {
            CardActionData data = cardActionParameter.CardActionData;
            Value = data.ActionValue;
            TargetCharacter = cardActionParameter.TargetCharacter;
            Duration = cardActionParameter.CardActionData.ActionDelay;
        }
        
        public override void DoAction()
        {
            if (!TargetCharacter) return;
            
            TargetCharacter.CharacterStats.Heal(Mathf.RoundToInt(Value));

            PlayFx();
            PlayAudio();
        }
    }
}