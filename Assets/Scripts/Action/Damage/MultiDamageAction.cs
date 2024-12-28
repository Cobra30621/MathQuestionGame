using System.Collections;
using System.Collections.Generic;
using Action.Parameters;
using Card;
using NueGames.Characters;
using NueGames.Managers;
using UnityEngine;

namespace NueGames.Action
{
    public class MultiDamageAction : GameActionBase
    {
        /// <summary>
        /// 傷害值
        /// </summary>
        private int _damage;
        
        /// <summary>
        /// 傷害次數
        /// </summary>
        private int _times;
        

        public MultiDamageAction(int damage, int times, 
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
        public MultiDamageAction(SkillInfo skillInfo)
        {
            _damage = skillInfo.EffectParameterList[0];
            _times = skillInfo.EffectParameterList[1];
        }

        protected override void DoMainAction()
        {
            GameActionExecutor.DoCoroutine(DamageCoroutine());
        }

        private IEnumerator DamageCoroutine()
        {
            for (int i = 0; i < _times; i++)
            {
                var damageInfo = new DamageInfo(_damage, ActionSource);
                var damageAction = new DamageAction(damageInfo, TargetList);
         
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