﻿using NueGames.Action;
using NueGames.Managers;

namespace NueGames.Relic.Common
{
    public class DrawCardOnAnswerCorrectRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.DrawCardOnAnswerCorrect;

        public int drawAmonut = 1;

        public DrawCardOnAnswerCorrectRelic()
        {
            UseCounter = true;
            NeedCounter = 3;
        }

        public override void SubscribeAllEvent()
        {
            QuestionManager.OnAnswerCorrect += OnAnswerCorrect;
        }

        public override void UnSubscribeAllEvent()
        {
            QuestionManager.OnAnswerCorrect -= OnAnswerCorrect;
        }


        protected override void OnAnswerCorrect()
        {
            Counter++;
            
            if (Counter >= NeedCounter)
            {
                GameActionExecutor.AddAction(new DrawCardAction(
                    drawAmonut, GetActionSource()));

                Counter = 0;
            }
            
            OnCounterChange.Invoke(Counter);
        }
    }
}