using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    public class EarnManaAction : GameActionBase
    {
        public EarnManaAction()
        {
            FxType = FxType.Buff;
            AudioActionType = AudioActionType.Power;
        }

        public override void SetValue(CardActionParameters cardActionParameters)
        {
            CardActionData data = cardActionParameters.CardActionData;
            Value = data.ActionValue;
            Duration = cardActionParameters.CardActionData.ActionDelay;
        }
        
        public override void DoAction()
        {
            if (CombatManager != null)
                CombatManager.IncreaseMana(Mathf.RoundToInt(Value));
            else
                Debug.LogError("There is no CombatManager");

            PlayFx();
            PlayAudio();
        }
    }
}