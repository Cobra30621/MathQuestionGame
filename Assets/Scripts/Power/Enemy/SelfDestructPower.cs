
using System.Collections;
using Combat;
using Effect;
using Effect.Parameters;
using UnityEngine;

namespace Power.Enemy
{
    /// <summary>
    /// 戰鬥中的 Boss 死亡後，自毀
    /// </summary>
    public class SelfDestructPower : PowerBase
    {
        public override PowerName PowerName => PowerName.SelfDestruct;


        public override void Init()
        {
            base.Init();
            CharacterHandler.OnAnyEnemyDead.AddListener(TrySelfDestruct);
        }

        public override void OnDead(DamageInfo info)
        {
            base.OnDead(info);
            CharacterHandler.OnAnyEnemyDead.RemoveListener(TrySelfDestruct);
        }


        public void TrySelfDestruct(Characters.Enemy.Enemy enemy)
        {
            if (enemy.IsBoss)
            {
                EffectExecutor.DoCoroutine(DestructCoroutine());
            }
        }

        private IEnumerator DestructCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            Owner.SetDeath();
        }
    }
}