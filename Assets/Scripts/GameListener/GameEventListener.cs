using Combat;
using Effect.Parameters;
using Question;

namespace GameListener
{
    /// <summary>
    /// 各種遊戲事件的監聽者
    /// 給能力(Power)、遺物(Relic)等功能使用
    /// </summary>
    public class GameEventListener
    {
        #region 戰鬥加成計算
        
        public CalculateOrder DamageCalculateOrder = CalculateOrder.None;

        public CalculateOrder BlockCalculateOrder = CalculateOrder.None;


        /// <summary>
        /// 受到傷害時，對傷害的加成
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public virtual float AtDamageReceive(float damage)
        {
            return damage;
        }

        /// <summary>
        /// 給予對方傷害時，對傷害的加成
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public virtual float AtDamageGive(float damage)
        {
            return damage;
        }

        /// <summary>
        /// 賦予格檔時，對格檔的加乘
        /// </summary>
        /// <param name="blockAmount"></param>
        /// <returns></returns>
        public virtual float ModifyBlock(float blockAmount)
        {
            return blockAmount;
        }

        /// <summary>
        /// 回合開始獲得瑪娜加成
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        public virtual int AtGainTurnStartMana(int rawValue)
        {
            return rawValue;
        }

        /// <summary>
        /// 回合開始抽卡數量加成
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        public virtual int AtGainTurnStartDraw(int rawValue)
        {
            return rawValue;
        }

        #endregion

        #region 戰鬥流程

        /// <summary>
        /// 遊戲回合開始時，觸發的方法
        /// </summary>
        public virtual void OnRoundStart(RoundInfo info)
        {
        }

        /// <summary>
        /// 遊戲回合結束時，觸發的方法
        /// </summary>
        public virtual void OnRoundEnd(RoundInfo info)
        {
        }

        /// <summary>
        /// 玩家/敵人 階段開始時觸發
        /// </summary>
        /// <param name="isAlly"></param>
        public virtual void OnTurnStart(TurnInfo info)
        {
        }

        /// <summary>
        /// 玩家/敵人 階段結束時觸發
        /// </summary>
        public virtual void OnTurnEnd(TurnInfo info)
        {
        }

        /// <summary>
        /// 戰鬥開始時觸發
        /// </summary>
        public virtual void OnBattleStart()
        {
        }

        /// <summary>
        /// 戰鬥勝利時觸發
        /// </summary>
        public virtual void OnBattleWin(int roundNumber)
        {
        }

        /// <summary>
        /// 戰鬥失敗時觸發
        /// </summary>
        public virtual void OnBattleLose(int roundNumber)
        {
        }

        #endregion

        #region 戰鬥中角色事件觸發

        /// <summary>
        /// 受到攻擊時，觸發的方法
        /// </summary>
        /// <param name="info"></param>
        public virtual void OnBeAttacked(DamageInfo info)
        {
        }

        /// <summary>
        /// 攻擊人時，觸發的方法
        /// </summary>
        /// <param name="info"></param>
        public virtual void OnAttack(DamageInfo info)
        {
            
        }

        /// <summary>
        /// 血量發生變化時，觸發的方法
        /// </summary>
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        public virtual void OnHealthChanged(int health, int maxHealth)
        {
        }


        /// <summary>
        /// 死亡時觸發
        /// </summary>
        /// <param name="damageInfo"></param>
        public virtual void OnDead(DamageInfo damageInfo)
        {
        }

        #endregion
    }
}