using NueGames.Card;
using NueGames.Data.Collection;
using UnityEngine;

namespace NueGames.Action
{
    public class DrawCardAction : GameActionBase
    {
        public override void SetValue(ActionParameters parameters)
        {
            ActionData data = parameters.ActionData;
            Duration = parameters.ActionData.ActionDelay;
            
            SetValue(data.ActionValue);
        }
        
        public void SetValue(int drawCard)
        {
            Amount = drawCard;

            HasSetValue = true;
        }
        
        public override void DoAction()
        {
            if (CollectionManager != null)
                CollectionManager.DrawCards(Mathf.RoundToInt(Amount));
            else
                Debug.LogError("There is no CollectionManager");
            
            // PlayFx();
            // PlayAudio();
        }
    }
}