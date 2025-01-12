using System.Collections;
using System.Collections.Generic;
using Characters;
using Effect.Parameters;
using UnityEngine;

namespace Effect.Damage
{
    /// <summary>
    /// 造成傷害的效果
    /// </summary>
    public class DamageEffect : EffectBase
    {
        /// <summary>
        /// 傷害設定參數
        /// </summary>
        private DamageInfo _damageInfo;
        
        /// <summary>
        /// 造成傷害次數
        /// </summary>
        private int _times;

        private int damage;

        #region 建構值

        /// <summary>
        /// 內部系統使用
        /// </summary>
        /// <param name="damageInfo"></param>
        /// <param name="targets"></param>
        /// <param name="times"></param>
        public DamageEffect(DamageInfo damageInfo, 
            List<CharacterBase> targets, int times = 1)
        {
            _times = times;
            TargetList = targets;
            _damageInfo = damageInfo;
        }

        /// <summary>
        /// 讀表用的建構值
        /// </summary>
        /// <param name="skillInfo"></param>
        public DamageEffect(SkillInfo skillInfo)
        {
            damage = skillInfo.EffectParameterList[0];
            _times = skillInfo.EffectParameterList[1];
        }

        
        public override void SetBasicValue(List<CharacterBase> targets, EffectSource effectSource)
        {
            base.SetBasicValue(targets, effectSource);
            // 創建傷害資訊 (由於需要等有 EffectSource 後，才能創建)
            _damageInfo = new DamageInfo(damage, EffectSource);
        }

        #endregion
        
        
        #region 主要效果執行

        public override void Play()
        {
            EffectExecutor.DoCoroutine(DamageCoroutine());
        }

        private IEnumerator DamageCoroutine()
        {
            // 造成 _time 次傷害
            for (int i = 0; i < _times; i++)
            {
                DamageOneTime(TargetList);
                yield return new WaitForSeconds(0.1f);
            }
        }

        /// <summary>
        /// 對所有目標造成一次傷害
        /// </summary>
        /// <param name="targets"></param>
        private void DamageOneTime(List<CharacterBase> targets)
        {
            foreach (var target in targets)
            {
                _damageInfo.SetTarget(target);
            
                PlaySpawnTextFx($"{_damageInfo.GetAfterBlockDamage()}", target.TextSpawnRoot);
                target.BeAttacked(_damageInfo);
            }
        }

        #endregion
        
        /// <summary>
        /// 取得傷害基礎值，針對傷害類型的效果。
        /// 回傳一個包含兩個整數的 tuple，分別為傷害基礎值和傷害次數。
        /// </summary>
        /// <returns>
        /// 包含兩個整數的 tuple，分別為傷害基礎值和傷害次數。
        /// </returns>
        public override (int, int) GetDamageBasicInfo()
        {
            return ((int)_damageInfo.DamageValue, _times);
        }
    }
}