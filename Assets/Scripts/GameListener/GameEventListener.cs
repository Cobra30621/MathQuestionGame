using System.Collections.Generic;
using Characters;
using Combat;
using Combat.Card;
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
        #region 加成計算
        
        /// <summary>
        /// 傷害加成的計算順序
        /// </summary>
        public CalculateOrder DamageCalculateOrder = CalculateOrder.None;

        /// <summary>
        /// 獲得格黨加成的計算順序
        /// </summary>
        public CalculateOrder BlockCalculateOrder = CalculateOrder.None;

        /// <summary>
        /// 卡片初始魔力加成的計算順序
        /// </summary>
        public CalculateOrder CardManaCalculateOrder = CalculateOrder.None;
        
        
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
        /// 回合開始獲得魔力加成
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        public virtual int AtGainTurnStartMana(int rawValue)
        {
            return rawValue;
        }

        /// <summary>
        /// 取得最大魔力
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        public virtual int GainMaxMana(int rawValue)
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
        
        
        
        /// <summary>
        /// 卡片初始魔力加成的計算
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        public virtual float GetCardRawMana(float rawValue)
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
        /// 執行攻擊行為時，觸發的方法
        /// 如果是多段傷害，只會執行一次
        /// </summary>
        /// <param name="info"></param>
        public virtual void OnAttack(DamageInfo info, List<CharacterBase> targets)
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
        /// <param name="info"></param>
        public virtual void OnDead(DamageInfo info)
        {
        }

        #endregion


        #region 其他


        public virtual void OnUseCard(BattleCard card)
        {
            
        }
        

        #endregion
        
    }
}