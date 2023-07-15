using NueGames.Characters;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Parameters;
using NueGames.Relic;
using UnityEngine;

namespace Action.Parameters
{
    public class DamageInfo
    {
        public ActionParameters Parameters;
        
        
        /// <summary>
        /// 傷害來源
        /// </summary>
        public ActionSource ActionSource => Parameters.ActionSource;
        /// <summary>
        /// 傷害對象
        /// </summary>
        public CharacterBase Target;
        /// <summary>
        /// 基礎傷害數值
        /// </summary>
        public int BaseValue => Parameters.ActionData.BaseValue;
        /// <summary>
        /// 固定傷害，不受狀態影響
        /// </summary>
        public bool FixDamage  => Parameters.ActionData.FixDamage;
        /// <summary>
        /// 可以穿甲
        /// </summary>
        public bool CanPierceArmor => Parameters.ActionData.CanPierceArmor;

        #region 選填

        
        /// <summary>
        /// 加成數值
        /// </summary>
        public float MultiplierValue => Parameters.ActionData.MultiplierValue;
        /// <summary>
        /// 加成數量
        /// </summary>
        public float MultiplierAmount;

        

        #endregion


        /// <summary>
        /// 取得加成數值
        /// </summary>
        /// <returns></returns>
        public int GetAddictionValue()
        {
            // Debug.Log( $" GetAddictionValue {BaseValue} + {MultiplierAmount} * {MultiplierValue}"  );
            return Mathf.RoundToInt(BaseValue + MultiplierAmount * MultiplierValue);
        }

        public int GetDamageValue()
        {
            return CombatCalculator.GetDamageValue(this);
        }


        /// <summary>
        /// 傷害是否可以被格檔
        /// </summary>
        /// <returns></returns>
        public bool EnableBlock()
        {
            var target = Target;
            var damageValue = GetDamageValue();
            
            if (CanPierceArmor)
            {
                return false;
            }
            
            if (target.HasPower(PowerType.Block))
            {
                var blockValue = target.GetPowerValue(PowerType.Block);

                return blockValue >= damageValue;
            }

            return false;
        }


        /// <summary>
        /// 本次傷害有讓對方受到傷害
        /// </summary>
        /// <returns></returns>
        public bool HaveDamage()
        {
            int afterBlockDamage = GetAfterBlockDamage();

            return afterBlockDamage != 0;
        }
        
        
        /// <summary>
        /// 取得格檔後剩下的傷害
        /// </summary>
        /// <param name="damageInfo"></param>
        /// <returns></returns>
        public int GetAfterBlockDamage()
        {
            var target = Target;
            var damageValue = GetDamageValue();
            var remainingDamage = damageValue;
            if (!CanPierceArmor)
            {
                if (target.HasPower(PowerType.Block))
                {
                    var blockValue = target.GetPowerValue(PowerType.Block);
                    remainingDamage -= blockValue;

                    if (remainingDamage < 0)
                    {
                        remainingDamage = 0;
                    }
                }
            }

            return remainingDamage;
        }
        
        
    }
}