using System.Collections.Generic;
using Action.Parameters;
using Characters;

namespace Action.Damage
{
    /// <summary>
    /// 給予傷害
    /// </summary>
    public class DamageAction : GameActionBase
    {
        private bool haveSetDamageInfo = false;
        
        private int _damage;
        private DamageInfo _damageInfo;
        
        /// <summary>
        /// 內部 Call
        /// </summary>
        /// <param name="damageInfo"></param>
        public DamageAction(DamageInfo damageInfo, List<CharacterBase> targets)
        {
            _damageInfo = damageInfo;
            TargetList = targets;
            haveSetDamageInfo = true;
        }

        /// <summary>
        /// 讀表
        /// </summary>
        /// <param name="skillInfo"></param>
        public DamageAction(SkillInfo skillInfo)
        {
            SkillInfo = skillInfo;
            _damage = skillInfo.EffectParameterList[0];
            haveSetDamageInfo = false;
            
        }

        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            if (!haveSetDamageInfo)
            {
                _damageInfo = new DamageInfo(_damage, ActionSource);
            }
            
            foreach (var target in TargetList)
            {
                _damageInfo.SetTarget(target);
                
                PlaySpawnTextFx($"{_damageInfo.GetAfterBlockDamage()}", target.TextSpawnRoot);
                target.BeAttacked(_damageInfo);
            }
        }

        public override (int, int) GetDamageBasicInfo()
        {
            return (_damage, 1);
        }
    }
}