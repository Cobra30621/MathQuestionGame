
using NueGames.NueDeck.Scripts.Action;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Power.Relics
{
    public class MathManaCharacterPower : PowerBase
    {
        public override PowerType PowerType => PowerType.Relic_MathManaCharacter;

        protected override void OnAnswerCorrect()
        {
            ApplyPowerAction action = new ApplyPowerAction();
            action.SetValue(PowerType.MathMana, 1, CombatManager.Instance.CurrentMainAlly);
            GameActionExecutor.Instance.AddToTop(action);
            
        }
    }
}