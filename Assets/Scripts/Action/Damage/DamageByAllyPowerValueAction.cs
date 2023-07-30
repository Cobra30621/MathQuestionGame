using System.Collections.Generic;
using Action.Parameters;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Power;

namespace NueGames.Action
{
    /// <summary>
    /// 根據玩家的能力層數，給予傷害
    /// </summary>
    public class DamageByAllyPowerValueAction : GameActionBase
    {
        /// <summary>
        /// 目標的能力
        /// </summary>
        private PowerName _targetPower;
        /// <summary>
        /// 傷害資訊
        /// </summary>
        private readonly DamageInfo _damageInfo;
        /// <summary>
        /// 加成數值
        /// </summary>
        private float _multiplierAmount;
        
        /// <summary>
        /// 設定數值
        /// </summary>
       
        /// <param name="targetList"></param>
        /// <param name="targetPower"></param>
        /// <param name="source"></param>
        /// <param name="multiplierAmount">給予能力 x 倍的傷害</param>
        /// <param name="fixDamage"></param>
        /// <param name="canPierceArmor"></param>
        public DamageByAllyPowerValueAction(List<CharacterBase> targetList, PowerName targetPower, 
            ActionSource source, int multiplierAmount = 1, bool fixDamage  = false, bool canPierceArmor  = false)
        {
            
            TargetList = targetList;
            _targetPower = targetPower;
            _multiplierAmount = multiplierAmount;
            
            _damageInfo = new DamageInfo()
            {
                ActionSource = source,
                FixDamage = fixDamage,
                CanPierceArmor = canPierceArmor
            };
        }
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            float damageValue = CombatManager.CurrentMainAlly.GetPowerValue(_targetPower) * _multiplierAmount;
            _damageInfo.damageValue = damageValue;
            
            GameActionExecutor.AddToBottom(new DamageAction(
                _damageInfo, TargetList));
        }
    }
}