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
        
        public override void SetValue(CardActionParameters parameters)
        {
            CardActionData data = parameters.CardActionData;
            Duration = parameters.CardActionData.ActionDelay;
            
            SetValue(data.ActionValue,parameters.TargetCharacter);
        }

        public void SetValue(int increaseValue, CharacterBase target)
        {
            Value = increaseValue;
            Target = target;

            hasSetValue = true;
        }
        
        public override void DoAction()
        {
            CheckHasSetValue();
            if (IsTargetNull()) return;
            
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