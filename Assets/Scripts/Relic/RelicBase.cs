using NueGames.Combat;
using NueGames.Managers;

namespace NueGames.Relic
{
    /// <summary>
    /// 遺物的基底 class
    /// </summary>
    public abstract class RelicBase
    {
        /// <summary>
        /// 哪一個遺物
        /// </summary>
        public abstract RelicType RelicType { get; }
        /// <summary>
        /// 遺物使用計數器
        /// </summary>
        public bool UseCounter;
        /// <summary>
        /// 計數器，用來計算如回合數、答對題數、使用卡片張數等等
        /// </summary>
        public int Counter;
        /// <summary>
        /// 發動事件，所需的計數
        /// </summary>
        public int NeedCounter;

        /// <summary>
        /// 計數器數值發生變動
        /// </summary>
        public System.Action<int> OnCounterChange;
        /// <summary>
        /// 事件管理器
        /// </summary>
        protected EventManager EventManager => EventManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        
        #region SetUp

        protected RelicBase()
        {
            SubscribeAllEvent();
        }

        /// <summary>
        /// 訂閱所有事件
        /// </summary>
        protected void SubscribeAllEvent()
        {
            if (EventManager != null)
            {
                EventManager.onAttacked += OnAttacked;
                EventManager.OnQuestioningModeEnd += OnQuestioningModeEnd;
                EventManager.OnAnswer += OnAnswer;
                EventManager.OnAnswerCorrect += OnAnswerCorrect;
                EventManager.OnAnswerWrong += OnAnswerWrong;
            }

            if (CombatManager != null)
            {
                CombatManager.OnAllyTurnStarted += OnTurnStarted;
            }
        }

        /// <summary>
        /// 取消訂閱所以事件
        /// </summary>
        protected void UnSubscribeAllEvent()
        {
            if (EventManager != null)
            {
                EventManager.onAttacked -= OnAttacked;
                EventManager.OnQuestioningModeEnd -= OnQuestioningModeEnd;
                EventManager.OnAnswer -= OnAnswer;
                EventManager.OnAnswerCorrect -= OnAnswerCorrect;
                EventManager.OnAnswerWrong -= OnAnswerWrong;
            }
            
            if (CombatManager != null)
            {
                CombatManager.OnAllyTurnStarted -= OnTurnStarted;
            }
        }
        
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

        /// <summary>
        /// 賦予格檔時，對格檔的加乘(最後觸發)
        /// </summary>
        /// <param name="blockAmount"></param>
        /// <returns></returns>
        public virtual float ModifyBlockLast(float blockAmount) {
            return blockAmount;
        }

        #endregion


        #region 戰鬥流程觸發
        /// <summary>
        /// 回合開始時，觸發的方法
        /// </summary>
        public virtual void OnTurnStarted()
        {
            
        }
        

        #endregion
        
        
        #region 事件觸發的方法
        

        /// <summary>
        /// 受到攻擊時，觸發的方法
        /// </summary>
        /// <param name="info"></param>
        /// <param name="damageAmount"></param>
        protected virtual void OnAttacked(DamageInfo info, int damageAmount){}
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

        public override string ToString()
        {
            return $"{nameof(RelicType)}: {RelicType}, {nameof(NeedCounter)}: {NeedCounter}";
        }
    }

    public enum RelicType
    {
        // Test 101 ~
        ManaGenerator = 101, // 每回產生瑪那
        DrawCardOnAnswerCorrect = 102
    }
}