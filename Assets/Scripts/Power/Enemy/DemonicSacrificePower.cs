using System.Collections.Generic;
using Characters;
using Combat;
using Effect;
using Effect.Common;
using Effect.Enemy;
using Effect.Power;

namespace Power.Enemy
{
    /// <summary>
    /// 惡魔獻祭
    /// </summary>
    public class DemonicSacrificePower : PowerBase
    {
        public override PowerName PowerName => PowerName.DemonicSacrifice;

        public string enhanceEnemyId;

        /// <summary>
        /// 幾回合後獻祭
        /// </summary>
        public int Turn = 2;
        
        /// <summary>
        /// 獻祭增加的力量
        /// </summary>
        public int AddStrengthAmount = 5;


        public override void Init()
        {
            enhanceEnemyId = $"{Amount}";
            SetPowerAmount(Turn);
        }



        public override void OnTurnStart(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                if (Amount == 1)
                {
                    DemonicSacrifice();
                }
                else
                {
                    StackPower(-1);
                }
            }
        }

        private void DemonicSacrifice()
        {
            CharacterBase enhanceEnemy = null;
            if (CombatManager.Instance.GetEnemyById(enhanceEnemyId, out enhanceEnemy))
            {
                var actions = new List<EffectBase>();

                // 提升力量
                var strengthAction = new ApplyPowerEffect(
                    AddStrengthAmount, PowerName.Strength,
                    new List<CharacterBase>() { enhanceEnemy }, GetEffectSource());
                actions.Add(strengthAction);
                
                // 治癒
                int health = Owner.GetHealth();
                var healAction = new HealEffect(health,
                    new List<CharacterBase>() { enhanceEnemy }, GetEffectSource());
                actions.Add(healAction);
                
                // 自己死亡
                var deathAction = new SetDeathEffect(
                    new List<CharacterBase>() { Owner }, GetEffectSource());
                actions.Add(deathAction);
                
                
                EffectExecutor.AddEffect(actions, 2f);
                
            }
            else
            {
                StackPower(-1);
            }
        }
    }
}