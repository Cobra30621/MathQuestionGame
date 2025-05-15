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
    /// 遊戲中各種效果（如：傷害、回血、抽牌等）的抽象基底類別。
    /// 可供卡牌、遺物、道具、敵人等系統繼承並實作自定義行為。
    /// </summary>
    public abstract class EffectBase
    {
        #region 基本參數與資源存取

        /// <summary>
        /// 效果的作用目標角色列表（可多個）
        /// </summary>
        public List<CharacterBase> TargetList { get; protected set; }

        /// <summary>
        /// 效果的來源，例如：哪一張卡、哪一個角色、哪個事件觸發了這個效果
        /// </summary>
        public EffectSource EffectSource { get; protected set; }

        /// <summary>
        /// 存取全域戰鬥管理器（方便查詢戰鬥狀態或操控流程）
        /// </summary>
        protected CombatManager CombatManager => CombatManager.Instance;

        /// <summary>
        /// 存取全域卡片/道具集合管理器
        /// </summary>
        protected CollectionManager CollectionManager => CollectionManager.Instance;

        #endregion

        #region 基本設定與執行接口

        /// <summary>
        /// 設定這個效果的基本資訊：目標列表與來源。
        /// 通常由上層系統呼叫
        /// </summary>
        /// <param name="targets">目標角色</param>
        /// <param name="effectSource">效果的觸發來源</param>
        public virtual void SetBasicValue(List<CharacterBase> targets, EffectSource effectSource)
        {
            TargetList = targets;
            EffectSource = effectSource;
        }

        /// <summary>
        /// 抽象函數：實際執行效果（例如造成傷害、抽牌、恢復等）
        /// 所有子類別都必須實作這個方法。
        /// </summary>
        public abstract void Play();

        #endregion

        #region 常用工具與擴充函數

        /// <summary>
        /// 播放文字浮動特效，例如顯示 "-5" 代表受到傷害。
        /// </summary>
        /// <param name="text">要顯示的文字</param>
        /// <param name="spawnRoot">特效產生的位置（通常是角色身上）</param>
        protected void PlaySpawnTextFx(string text, Transform spawnRoot)
        {
            if (spawnRoot == null) return;

            FxManager.Instance.SpawnFloatingText(spawnRoot, text);
        }

        /// <summary>
        /// 取得這個效果的預設傷害資訊（供需要的子類別覆寫）。
        /// 通常用於傷害類效果，例如 DamageEffect。
        /// </summary>
        /// <returns>
        /// Tuple：第一項是每次攻擊的傷害值，第二項是攻擊次數。
        /// 預設值為 (-1, 1)，表示「無效傷害」。
        /// </returns>
        public virtual (int damagePerHit, int hitCount) GetDamageBasicInfo()
        {
            return (-1, 1);
        }

        #endregion
    }
}
