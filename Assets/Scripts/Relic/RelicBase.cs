using GameListener;
using NueGames.Action;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;

namespace NueGames.Relic
{
    /// <summary>
    /// 遺物的基底 class
    /// </summary>
    public abstract class RelicBase : GameEventListener
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

        
        #region SetUp

        protected RelicBase()
        {
            SubscribeAllEvent();
        }


        #endregion

        #region 事件

        /// <summary>
        /// 當刪除遺物
        /// </summary>
        public virtual void OnRelicRemove()
        {
            UnSubscribeAllEvent();
        }
        

        #endregion
        
        #region 工具
        
        
        /// <summary>
        /// 取得 DamageInfo
        /// </summary>
        /// <param name="damageValue"></param>
        /// <param name="fixDamage"></param>
        /// <returns></returns>
        protected DamageInfo GetDamageInfo(int damageValue, bool fixDamage)
        {
            DamageInfo damageInfo = new DamageInfo()
            {
                BaseValue = damageValue,
                Target = CombatManager.CurrentMainAlly,
                FixDamage = fixDamage,
                ActionSource = ActionSource.Relic,
                SourceRelic = RelicType
            };

            return damageInfo;
        }

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
        DrawCardOnAnswerCorrect = 102,
        StrengthGenerator = 103,
        Burning = 104
    }
}