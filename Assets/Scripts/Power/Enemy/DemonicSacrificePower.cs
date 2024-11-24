using System.Collections.Generic;
using Action.Enemy;
using Combat;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Managers;
using UnityEngine;

namespace NueGames.Power
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

        public override void SubscribeAllEvent()
        {
            CombatManager.OnTurnStart += OnTurnStart;
        }
        
        public override void UnSubscribeAllEvent()
        {
            CombatManager.OnTurnStart -= OnTurnStart;
        }


        protected override void OnTurnStart(TurnInfo info)
        {
            Debug.Log("OnTurnStart");
            if (IsCharacterTurn(info))
            {
                if (Amount == 1)
                {
                    DemonicSacrifice();
                }
                else
                {
                    Debug.Log("stackPower");
                    StackPower(-1);
                }
            }
        }

        private void DemonicSacrifice()
        {
            CharacterBase enhanceEnemy = null;
            if (CombatManager.Instance.GetEnemyById(enhanceEnemyId, out enhanceEnemy))
            {
                var actions = new List<GameActionBase>();

                // 提升力量
                var strengthAction = new ApplyPowerAction(
                    AddStrengthAmount, PowerName.Strength,
                    new List<CharacterBase>() { enhanceEnemy }, GetActionSource());
                actions.Add(strengthAction);
                
                // 治癒
                int health = Owner.GetHealth();
                var healAction = new HealAction(health,
                    new List<CharacterBase>() { enhanceEnemy }, GetActionSource());
                actions.Add(healAction);
                
                // 自己死亡
                var deathAction = new SetDeathAction(
                    new List<CharacterBase>() { Owner }, GetActionSource());
                actions.Add(deathAction);
                
                
                GameActionExecutor.AddAction(actions, 2f);
                
            }
        }
    }
}