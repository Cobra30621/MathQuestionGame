using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    public class DrawCardAction : GameActionBase
    {
        public DrawCardAction() { }
        public DrawCardAction(int amount)
        {
            Value = amount;
        }

        public override void SetValue(CardActionParameter cardActionParameter)
        {
            CardActionData data = cardActionParameter.CardActionData;
            Value = data.ActionValue;
            Duration = cardActionParameter.CardActionData.ActionDelay;
        }
        
        public override void DoAction()
        {
            if (CollectionManager != null)
                CollectionManager.DrawCards(Mathf.RoundToInt(Value));
            else
                Debug.LogError("There is no CollectionManager");
            
            // if (FxManager != null)
            //     FxManager.PlayFx( FxType.);

            if (AudioManager != null) 
                AudioManager.PlayOneShot(AudioActionType.Draw);
        }
    }
}