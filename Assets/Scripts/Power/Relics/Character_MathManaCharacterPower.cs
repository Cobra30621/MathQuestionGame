
using NueGames.Action;
using NueGames.Enums;
using NueGames.Managers;
using UnityEngine;

namespace NueGames.Power.Relics
{
    public class Character_MathManaCharacterPower : PowerBase
    {
        public override PowerType PowerType => PowerType.Character_MathManaCharacter;

        protected override void OnAnswerCorrect()
        {
            ApplyPowerAction action = new ApplyPowerAction();
            action.SetValue(PowerType.MathMana, 1, CombatManager.Instance.CurrentMainAlly);
            GameActionExecutor.Instance.AddToTop(action);
            
        }
    }
}