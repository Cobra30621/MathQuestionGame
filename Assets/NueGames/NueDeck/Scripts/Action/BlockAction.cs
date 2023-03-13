using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    public class BlockAction : GameActionBase
    {
        public BlockAction()
        {
            FxType = FxType.Block;
            AudioActionType = AudioActionType.Block;
        }
        
        public override void SetValue(CardActionParameters cardActionParameters)
        {
            CardActionData data = cardActionParameters.CardActionData;
            Target = cardActionParameters.TargetCharacter;
            Duration = cardActionParameters.CardActionData.ActionDelay;
            Value = data.ActionValue;
        }
        
        public override void DoAction()
        {
            if (!Target) return;
            Target.CharacterStats.ApplyPower(PowerType.Block,Mathf.RoundToInt(Value));
            
            PlayFx();
            PlayAudio();
        }
    }
}