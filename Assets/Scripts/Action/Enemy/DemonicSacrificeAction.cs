using Card;
using Enemy;
using NueGames.Action;
using NueGames.Power;
using UnityEngine;

namespace Action.Enemy
{
    public class DemonicSacrificeAction : GameActionBase
    {
        private string targetEnemyId;
        
        /// <summary>
        /// 狀態層數
        /// </summary>
        private int _applyValue;
        /// <summary>
        /// 給予狀態
        /// </summary>
        private PowerName _targetPower;
        
        
        public DemonicSacrificeAction(SkillInfo skillInfo)
        {
            SkillInfo = skillInfo;
            targetEnemyId = $"{skillInfo.EffectParameterList[0]}";
            _applyValue = skillInfo.EffectParameterList[2];
            _targetPower =  PowerHelper.GetPowerName(skillInfo.EffectParameterList[1]);
        }
        
        
        protected override void DoMainAction()
        {
            EnemyBase targetEnemy;
            var find = CombatManager.characterHandler.GetEnemyWithId(targetEnemyId, out targetEnemy);
            Debug.Log($"Find {targetEnemy}");
            if (find)
            {
                targetEnemy.ApplyPower(_targetPower, _applyValue);
                
                // 自己自殺
                Debug.Log($"自殺 {TargetList[0]}");
                TargetList[0].SetDeath();
            }
            else
            {
                Debug.LogError($"找不到敵人 {targetEnemyId}");
            }
        }
    }
}