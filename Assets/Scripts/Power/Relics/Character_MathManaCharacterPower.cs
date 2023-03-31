
using NueGames.Action;
using NueGames.Enums;
using NueGames.Managers;
using UnityEngine;

namespace NueGames.Power.Relics
{
    /// <summary>
    /// 角色能力，先用能力系統實作
    /// 答對時，獲得一點數學瑪娜
    /// </summary>
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