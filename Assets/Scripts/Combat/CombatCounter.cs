using Characters;
using Combat.Card;
using Effect.Parameters;

namespace Combat
{
    public class CombatCounter
    {
        /// <summary>
        /// 本回合中，玩家使用過的卡片
        /// </summary>
        public int UseCardCountInCurrentTurn;

        /// <summary>
        /// 本回合中，玩家對敵人造成的傷害
        /// </summary>
        public int HurtEnemyHealthInCurrentTurn;



        public bool IsFirstUseCardInCurrentTurn()
        {
            return UseCardCountInCurrentTurn == 0;
        }

        /// <summary>
        /// 敵人收到攻擊
        /// </summary>
        /// <param name="damage"></param>
        public void CharacterTakeDamage(CharacterBase character, DamageInfo damageInfo)
        {
            if (character.IsCharacterType(CharacterType.Enemy))
            {
                HurtEnemyHealthInCurrentTurn += damageInfo.GetAfterBlockDamage();
            }
            
            
        }
        
        
        #region Event

        public void Init()
        {
            CombatManager.OnTurnStart += OnTurnStart;
            BattleCard.OnCardExecuteCompleted += OnUseCard;
        }

        public void OnBattleEnd()
        {
            CombatManager.OnTurnStart -= OnTurnStart;
            BattleCard.OnCardExecuteCompleted -= OnUseCard;
        }


        private void OnTurnStart(TurnInfo turnInfo)
        {
            UseCardCountInCurrentTurn = 0;
            HurtEnemyHealthInCurrentTurn = 0;
        }

        private void OnUseCard(BattleCard card)
        {
            UseCardCountInCurrentTurn++;
        }
        

        #endregion

        public bool IsNoEnemyHurtInCurrentTurn()
        {
            return HurtEnemyHealthInCurrentTurn == 0;
        }
    }
}