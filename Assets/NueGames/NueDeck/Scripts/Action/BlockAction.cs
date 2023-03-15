using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Action
{
    public class BlockAction : GameActionBase
    {
        public BlockAction()
        {
            FxType = FxType.Block;
            AudioActionType = AudioActionType.Block;
        }
        
        public override void SetValue(CardActionParameters parameters)
        {
            CardActionData data = parameters.CardActionData;
            Duration = parameters.CardActionData.ActionDelay;
            
            SetValue(data.ActionValue, parameters.TargetCharacter);
        }

        public void SetValue(int powerValue, CharacterBase target)
        {
            Amount = powerValue;
            Target = target;

            HasSetValue = true;
        }
        
        public override void DoAction()
        {
            CheckHasSetValue();
            if (IsTargetNull()) return;
            
            Target.CharacterStats.ApplyPower(PowerType.Block,Mathf.RoundToInt(Amount));
            
            PlayFx();
            PlayAudio();
        }
    }
}