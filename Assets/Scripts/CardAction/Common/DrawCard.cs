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
            GameActionExecutor.AddToBottom(new DrawCardAction(
                drawCount, GetActionSource()));
        }
    }
}