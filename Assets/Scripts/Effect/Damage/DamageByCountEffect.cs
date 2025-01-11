using Effect.Parameters;
using UnityEngine;

namespace Effect.Damage
{
    
    public class DamageByCountEffect : EffectBase
    {
        private int count;
        private int type;
        private int baseDamage;
        private int damage;
        public DamageByCountEffect(SkillInfo skillInfo)
        {
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
            var damageAction = new DamageEffect(damageInfo, TargetList);
            damageAction.DoAction();
        }
    }
}