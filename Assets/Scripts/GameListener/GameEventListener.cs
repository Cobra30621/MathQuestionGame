using Action.Parameters;
using Combat;
using NueGames.Combat;
using NueGames.Parameters;
using Question;

namespace GameListener
{
    /// <summary>
    /// 各種遊戲事件的監聽者
    /// 給能力(Power)、遺物(Relic)、藥水等功能使用
    /// </summary>
    public class GameEventListener
    {
        /// <summary>
        /// 答題管理器
        /// </summary>
        protected QuestionManager QuestionManager => QuestionManager.Instance;


        #region 戰鬥計算順序 (傷害、格擋)

        public CalculateOrder DamageCalculateOrder = CalculateOrder.None;

        public CalculateOrder BlockCalculateOrder = CalculateOrder.None;
        

        #endregion

        #region 訂閱事件

        /// <summary>
        /// 訂閱所有事件
        /// </summary>
        public virtual void SubscribeAllEvent() { }

        /// <summary>
        /// 取消訂閱所以事件
        /// </summary>
        public virtual void UnSubscribeAllEvent(){ }
        

        #endregion
        
        #region 戰鬥計算
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
        public virtual float ModifyBlock(float blockAmount) {
            return blockAmount;
        }

        #endregion
        
        #region 戰鬥流程觸發
        /// <summary>
        /// 遊戲回合開始時，觸發的方法
        /// </summary>
        protected virtual void OnRoundStart(RoundInfo info)
        {
            
        }
        
        /// <summary>
        /// 遊戲回合結束時，觸發的方法
        /// </summary>
        protected virtual void OnRoundEnd(RoundInfo info)
        {
            
        }
        
        /// <summary>
        /// 玩家/敵人 回合開始時觸發
        /// </summary>
        /// <param name="isAlly"></param>
        protected virtual void OnTurnStart(TurnInfo info) 
        {
            
        }
        
        /// <summary>
        /// 玩家/敵人 回合結束時觸發
        /// </summary>
        protected virtual void OnTurnEnd(TurnInfo info)
        {
            
        }

        protected virtual void OnBattleStart()
        {
            
        }
        protected virtual void OnBattleWin(int roundNumber)
        {
            
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

        #endregion
        
        #region 戰鬥事件觸發
        
        /// <summary>
        /// 受到攻擊時，觸發的方法
        /// </summary>
        /// <param name="info"></param>
        protected virtual void OnAttacked(DamageInfo info){}

        protected virtual void OnDead(DamageInfo damageInfo){}
        #endregion
        
        #region 答題流程
        /// <summary>
        /// 開始問答模式時，觸發的方法
        /// </summary>
        protected virtual void OnQuestioningModeStart(){}
        /// <summary>
        /// 回答問題時，觸發的方法
        /// </summary>
        protected virtual void OnAnswer(){}
        /// <summary>
        /// 答對問題時，觸發的方法
        /// </summary>
        protected virtual void OnAnswerCorrect(){}
        /// <summary>
        /// 答錯問題時，觸發的方法
        /// </summary>
        protected virtual void OnAnswerWrong(){}
        /// <summary>
        /// 結束問答模式時，觸發的方法
        /// </summary>
        /// <param name="correctCount"></param>
        protected virtual void OnQuestioningModeEnd(int correctCount){}
        
        #endregion

        
    }
}