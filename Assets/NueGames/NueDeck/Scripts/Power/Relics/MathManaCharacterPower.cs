
using NueGames.NueDeck.Scripts.Action;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Power.Relics
{
    public class MathManaCharacterPower : PowerBase
    {
        public override PowerType PowerType => PowerType.MathManaCharacter;

        protected override void OnAnswerCorrect()
        {
            NeedCounter = 1;
            Counter++;

            ApplyPowerAction action = new ApplyPowerAction();
            action.SetValue(PowerType.MathMana, 1, CombatManager.Instance.CurrentMainAlly);
            GameActionExecutor.Instance.AddToTop(action);
            
            Debug.Log($"{PowerType} counter: {Counter} / {NeedCounter}");
        }
    }
}