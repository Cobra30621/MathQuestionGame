using Characters;
using Combat;
using Effect.Parameters;
using GameListener;
using Relic.Data;

namespace Relic
{
    /// <summary>
    /// 遺物的基底 class
    /// </summary>
    public abstract class RelicBase : GameEventListener
    {
        /// <summary>
        /// 哪一個遺物
        /// </summary>
        public abstract RelicName RelicName { get; }
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


        public RelicInfo RelicInfo;

        public CharacterBase MainAlly => CombatManager.Instance.MainAlly;
        
        protected CombatManager CombatManager => CombatManager.Instance;

        
        #region 工具
        public bool IsCharacterTurn(TurnInfo info)
        {
            return info.CharacterType == CharacterType.Ally;
        }
        protected EffectSource GetEffectSource()
        {
            return new EffectSource()
            {
                SourceType = SourceType.Relic,
                SourceRelic = RelicName,
                SourceCharacter = MainAlly
            };
        }


        #endregion
        
        
        public override string ToString()
        {
            return $"{nameof(RelicName)}: {RelicName}, {nameof(NeedCounter)}: {NeedCounter}";
        }
    }

    
}