using Effect.Parameters;
using UnityEngine;

namespace Effect.Damage
{
    /// <summary>
    /// 根據某種類型的參數，給予傷害
    /// </summary>
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

        public override void Play()
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
            var damageInfo = new DamageInfo(damage, EffectSource);
            var damageAction = new DamageEffect(damageInfo,  TargetList);
            damageAction.Play();
        }
    }
}