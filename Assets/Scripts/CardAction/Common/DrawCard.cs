using NueGames.Action;
using NueGames.Managers;
using UnityEngine;

namespace CardAction
{
    public class DrawCard : CardActionBase
    {
        [SerializeField] private int drawCount;
        
        protected override void DoMainAction()
        {
            var drawCardAction = new DrawCardAction();
            drawCardAction.SetValue(drawCount, GetActionSource());
            GameActionExecutor.Instance.AddToBottom(drawCardAction);
        }
    }
}