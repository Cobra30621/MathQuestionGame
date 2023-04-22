using NueGames.Action;
using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace Action
{
    /// <summary>
    /// 根據數學瑪娜數量，給予狀態
    /// </summary>
    public class ApplyPowerByMathManaValueAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.ApplyPowerByMathManaValue;
        private PowerType powerType;
        protected int baseValue;
        protected int additionValue;
        
        public override void SetValue(ActionParameters parameters)
        {
            ActionData data = parameters.ActionData;
            powerType = data.PowerType; 
            baseValue = data.ActionValue;
            additionValue = data.AdditionValue;
            Target = parameters.TargetCharacter;

            HasSetValue = true;
        }

        public override void DoAction()
        {
            CheckHasSetValue();
            if (IsTargetNull()) return;
            
            int mathMana = CombatManager.CurrentMainAlly.GetPowerValue(PowerType.MathMana);
            Amount = GetAddedValue(mathMana);
            Target.CharacterStats.ApplyPower(powerType,Mathf.RoundToInt(Amount));
        }
        
        protected int GetAddedValue(int mathMana)
        {
            // Debug.Log($"{base.ToString()}, {nameof(baseValue)}: {baseValue}, " +
            //           $"{nameof(additionValue)}: {additionValue}, {nameof(answerCount)}: {answerCount}, {nameof(_answerOutcomeType)}: {_answerOutcomeType}" +
            //           $"baseValue + answerCount * additionValue, {baseValue + answerCount * additionValue}");
            
            return baseValue + mathMana * additionValue;
        }
    }
}