using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    public class DrawCardAction : GameActionBase
    {
        public DrawCardAction()
        {
            
        }

        public override void SetValue(CardActionParameters cardActionParameters)
        {
            CardActionData data = cardActionParameters.CardActionData;
            Value = data.ActionValue;
            Duration = cardActionParameters.CardActionData.ActionDelay;
        }
        
        public override void DoAction()
        {
            if (CollectionManager != null)
                CollectionManager.DrawCards(Mathf.RoundToInt(Value));
            else
                Debug.LogError("There is no CollectionManager");
            
            // PlayFx();
            // PlayAudio();
        }
    }
}