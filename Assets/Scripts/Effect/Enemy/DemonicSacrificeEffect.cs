using Power;
using UnityEngine;

namespace Effect.Enemy
{
    /// <summary>
    /// 惡魔獻祭: 自殺後將血量給  [1] 怪物 id ，並給她[2]狀態[3]層
    /// </summary>
    public class DemonicSacrificeEffect : EffectBase
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
        
        
        public DemonicSacrificeEffect(SkillInfo skillInfo)
        {
            targetEnemyId = $"{skillInfo.EffectParameterList[0]}";
            _applyValue = skillInfo.EffectParameterList[2];
            _targetPower =  PowerHelper.GetPowerName(skillInfo.EffectParameterList[1]);
        }


        public override void Play()
        {
            Characters.Enemy.Enemy targetEnemy;
            var find = CombatManager.characterHandler.GetEnemyWithId(targetEnemyId, out targetEnemy);
            if (find)
            {
                targetEnemy.ApplyPower(_targetPower, _applyValue, EffectSource);
                
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