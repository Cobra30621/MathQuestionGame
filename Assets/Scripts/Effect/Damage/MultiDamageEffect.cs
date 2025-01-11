using System.Collections;
using System.Collections.Generic;
using Characters;
using Effect.Parameters;
using UnityEngine;

namespace Effect.Damage
{
    public class MultiDamageEffect : EffectBase
    {
        /// <summary>
        /// 傷害值
        /// </summary>
        private int _damage;
        
        /// <summary>
        /// 傷害次數
        /// </summary>
        private int _times;
        

        public MultiDamageEffect(int damage, int times, 
            List<CharacterBase> targets, ActionSource actionSource)
        {
            _damage = damage;
            _times = times;
            TargetList = targets;
            ActionSource = actionSource;
        }

        /// <summary>
        /// 讀表用的建構值
        /// </summary>
        /// <param name="skillInfo"></param>
        public MultiDamageEffect(SkillInfo skillInfo)
        {
            _damage = skillInfo.EffectParameterList[0];
            _times = skillInfo.EffectParameterList[1];
        }

        protected override void DoMainAction()
        {
            EffectExecutor.DoCoroutine(DamageCoroutine());
        }

        private IEnumerator DamageCoroutine()
        {
            for (int i = 0; i < _times; i++)
            {
                var damageInfo = new DamageInfo(_damage, ActionSource);
                var damageAction = new DamageEffect(damageInfo, TargetList);
         
                damageAction.DoAction();
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        
        
        public override (int, int) GetDamageBasicInfo()
        {
            return (_damage, _times);
        }
    }
}