using System.Collections.Generic;
using Characters;
using Combat;
using Effect;
using Effect.Damage;
using Effect.Parameters;
using Effect.Power;
using UnityEngine;

namespace Power.Buff
{


    /// <summary>
    /// 反彈能力
    /// </summary>
    public class ThornsPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Thorns;



        // 怪物攻擊時，造成傷害後反彈傷害
        public override void OnBeAttacked(DamageInfo info)
        {
            var source = info.EffectSource.SourceCharacter;
            // 怪物攻擊時，造成傷害後反彈傷害
            Debug.Log(source);
            if (source != null)
            {
                // 造成與層數相等的傷害
                var damageInfo = new DamageInfo(Amount, GetEffectSource(), fixDamage: true);
                EffectExecutor.ExecuteImmediately(new DamageEffect(damageInfo, new List<CharacterBase>() {info.EffectSource.SourceCharacter}));
         
                // 反彈後減層數 1 
                EffectExecutor.ExecuteImmediately(
                    new ApplyPowerEffect(-1, PowerName, 
                        new List<CharacterBase>(){Owner}, GetEffectSource()));
      
            }
        }

    }
}