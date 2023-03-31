using NueGames.Action;
using NueGames.Enums;
using NueGames.Managers;
using UnityEngine;

namespace NueGames.Power.Relics
{
    /// <summary>
    /// 遺物能力，先用能力系統實作
    /// 答對 3 題，抽一張卡
    /// </summary>
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