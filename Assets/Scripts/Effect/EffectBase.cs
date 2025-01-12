using System.Collections.Generic;
using Characters;
using Combat;
using Combat.Card;
using Effect.Parameters;
using Feedback;
using UnityEngine;

namespace Effect
{
    /// <summary>
    /// 用來實作遊戲效果（ex: 給予傷害、抽牌、回血）的基底 class
    /// 可以供其他系統（ex: 卡牌、遺物、敵人、道具等等）
    /// </summary>
    public abstract class EffectBase
    {
        #region Parameters

        /// <summary>
        /// 效果的目標對象
        /// </summary>
        public List<CharacterBase> TargetList { get; protected set; }

        /// <summary>
        /// 效果的動作來源
        /// </summary>
        public EffectSource EffectSource { get; protected set; }


        /// <summary>
        /// 戰鬥管理器
        /// </summary>
        protected CombatManager CombatManager => CombatManager.Instance;

        /// <summary>
        /// 卡組管理器
        /// </summary>
        protected CollectionManager CollectionManager => CollectionManager.Instance;

        #endregion

        /// <summary>
        /// 設定基礎數值
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="effectSource"></param>
        public virtual void SetBasicValue(List<CharacterBase> targets, EffectSource effectSource)
        {
            TargetList = targets;
            EffectSource = effectSource;
        }


        /// <summary>
        /// 執行遊戲的效果
        /// </summary>
        public abstract void Play();


        /// <summary>
        /// 生成文字特效(如收到傷害顯示傷害數值)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="spawmRoot"></param>
        protected void PlaySpawnTextFx(string info, Transform spawmRoot)
        {
            if (spawmRoot == null)
                return;

            FxManager.Instance.SpawnFloatingText(spawmRoot, info);
        }

        /// <summary>
        /// 取得傷害基礎值，針對傷害類型的效果。
        /// 回傳一個包含兩個整數的 tuple，分別為傷害基礎值和傷害次數。
        /// </summary>
        /// <returns>
        /// 包含兩個整數的 tuple，分別為傷害基礎值和傷害次數。
        /// 預設值為 (-1, 1)。
        /// </returns>
        public virtual (int, int) GetDamageBasicInfo()
        {
            return (-1, 1);
        }
    }
}