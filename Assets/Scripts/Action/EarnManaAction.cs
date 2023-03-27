using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    public class EarnManaAction : GameActionBase
    {
        public EarnManaAction()
        {
            FxType = FxType.Buff;
            AudioActionType = AudioActionType.Power;
        }

        public override void SetValue(CardActionParameters parameters)
        {
            CardActionData data = parameters.CardActionData;
            Duration = parameters.CardActionData.ActionDelay;
            
            SetValue(Amount);
        }

        public void SetValue(int earnMana)
        {
            Amount = earnMana;
            
            HasSetValue = true;
        }
        
        public override void DoAction()
        {
            CheckHasSetValue();
            
            if (CombatManager != null)
                CombatManager.IncreaseMana(Mathf.RoundToInt(Amount));
            else
                Debug.LogError("There is no CombatManager");

            PlayFx();
            PlayAudio();
        }
    }
}