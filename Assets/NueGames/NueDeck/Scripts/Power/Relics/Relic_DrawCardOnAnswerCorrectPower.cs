using NueGames.NueDeck.Scripts.Action;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Power.Relics
{
    public class Relic_DrawCardOnAnswerCorrectPower : PowerBase
    {
        public override PowerType PowerType => PowerType.Relic_DrawCardOnAnswerCorrect;

        protected override void OnAnswerCorrect()
        {
            Counter++;
            NeedCounter = 3;

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