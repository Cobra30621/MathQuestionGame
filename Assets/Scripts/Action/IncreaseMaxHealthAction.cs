using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
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
            Amount = increaseValue;
            Target = target;

            HasSetValue = true;
        }
        
        public override void DoAction()
        {
            CheckHasSetValue();
            if (IsTargetNull()) return;
            
            Target.CharacterStats.IncreaseMaxHealth(Mathf.RoundToInt(Amount));

            if (FxManager != null)
            {
                FxManager.PlayFx(Target.transform,FxType.Attack);
                FxManager.SpawnFloatingText(Target.TextSpawnRoot,Amount.ToString());
            }
            PlayAudio();
        }
    }
}