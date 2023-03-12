using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    public class IncreaseMaxHealthAction : GameActionBase
    {
        public IncreaseMaxHealthAction()
        {
            FxType = FxType.Buff;
            AudioActionType = AudioActionType.Power;
        }
        
        public override void SetValue(CardActionParameters cardActionParameters)
        {
            CardActionData data = cardActionParameters.CardActionData;
            Value = data.ActionValue;
            Target = cardActionParameters.TargetCharacter;
            Duration = cardActionParameters.CardActionData.ActionDelay;
        }
        
        public override void DoAction()
        {
            if (!Target) return;
            
            Target.CharacterStats.IncreaseMaxHealth(Mathf.RoundToInt(Value));

            if (FxManager != null)
            {
                FxManager.PlayFx(Target.transform,FxType.Attack);
                FxManager.SpawnFloatingText(Target.TextSpawnRoot,Value.ToString());
            }
            PlayAudio();
        }
    }
}