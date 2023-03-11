using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    public class EarnMathManaAction : GameActionBase
    {
        public EarnMathManaAction()
        {
            FxType = FxType.Buff;
            AudioActionType = AudioActionType.Power;
        }

        public override void SetValue(CardActionParameter cardActionParameter)
        {
            CardActionData data = cardActionParameter.CardActionData;
            Value = data.ActionValue;
            Duration = cardActionParameter.CardActionData.ActionDelay;
        }
        
        public override void DoAction()
        {
            if (CombatManager != null)
                CombatManager.IncreaseMathMana(Mathf.RoundToInt(Value));
            else
                Debug.LogError("There is no CombatManager");

            PlayFx();
            PlayAudio();
        }
    }
}