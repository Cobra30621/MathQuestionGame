using Action.Parameters;
using UnityEngine;

namespace Action.Damage
{
    
    public class DamageByCountAction : GameActionBase
    {
        private int count;
        private int type;
        private int baseDamage;
        private int damage;
        public DamageByCountAction(SkillInfo skillInfo)
        {
            SkillInfo = skillInfo;
            type = skillInfo.EffectParameterList[0];
            baseDamage = skillInfo.EffectParameterList[1];
        }
        protected override void DoMainAction()
        {
            switch(type)
            {
                case 1:
                    // 清空魔力？？？
                    // CombatManager.SetMana(0);
                    count = CombatManager.EnemyCount;
                    break;
                case 2:
                    count = CollectionManager.UsedCardCount;
                    break;
                default:
                    Debug.LogError("Invalid type");
                    break;
            }
            Debug.Log("count: " + count);
            damage = count * baseDamage;
            var damageInfo = new DamageInfo(damage, ActionSource);
            var damageAction = new DamageAction(damageInfo, TargetList);
            damageAction.DoAction();
        }
    }
}