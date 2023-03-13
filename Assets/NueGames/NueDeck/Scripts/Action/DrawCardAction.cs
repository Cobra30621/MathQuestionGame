using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    public class DrawCardAction : GameActionBase
    {
        public override void SetValue(CardActionParameters parameters)
        {
            CardActionData data = parameters.CardActionData;
            Duration = parameters.CardActionData.ActionDelay;
            
            SetValue(data.ActionValue);
        }
        
        public void SetValue(int drawCard)
        {
            Value = drawCard;

            hasSetValue = true;
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