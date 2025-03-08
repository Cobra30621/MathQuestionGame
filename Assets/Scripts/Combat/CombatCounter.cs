using Combat.Card;

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
        
        
        #region Event

        public void Init()
        {
            CombatManager.OnTurnStart += OnTurnStart;
            CollectionManager.OnUseCard += OnUseCard;
        }

        public void OnBattleEnd()
        {
            CombatManager.OnTurnStart -= OnTurnStart;
            CollectionManager.OnUseCard -= OnUseCard;
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
    }
}