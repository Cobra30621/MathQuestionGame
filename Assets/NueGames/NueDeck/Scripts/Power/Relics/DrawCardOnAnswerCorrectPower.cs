using NueGames.NueDeck.Scripts.Action;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Power.Relics
{
    public class DrawCardOnAnswerCorrectPower : PowerBase
    {
        public override PowerType PowerType => PowerType.DrawCardOnAnswerCorrect;

        protected override void OnAnswerCorrect()
        {
            NeedCounter = 3;
            Counter++;

            if (Counter >= NeedCounter)
            {
                DrawCardAction action = new DrawCardAction();
                action.SetValue(Value);
                GameActionExecutor.Instance.AddToTop(action);

                Counter = 0;
            }
            
            Debug.Log($"{PowerType} counter: {Counter} / {NeedCounter}");
        }
    }
}