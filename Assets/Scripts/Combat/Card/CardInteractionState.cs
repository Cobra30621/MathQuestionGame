using System;
using UnityEngine;

namespace Combat.Card
{
    /// <summary>
    /// 用來記錄目前卡牌互動過程中的狀態，包含滑鼠選擇、拖曳、與目標選取等資料。
    /// 由 HandController 負責初始化與更新。
    /// </summary>
    [Serializable]
    public class CardInteractionState
    {
        /// <summary>
        /// 目前滑鼠最靠近的卡牌索引，用來作為預選目標。
        /// 若為 -1 則表示沒有任何卡片接近滑鼠。
        /// </summary>
        public int ClosestMouseIndex = -1;

        /// <summary>
        /// 當前選中的卡牌索引。
        /// 若為 -1 表示目前沒有選擇任何卡牌。
        /// </summary>
        public int SelectedIndex = -1;

        /// <summary>
        /// 當前拖曳中的卡牌索引（仍在手牌內）。
        /// 若為 -1 表示沒有拖曳中的卡牌。
        /// </summary>
        public int DraggedInsideHandIndex = -1;

        /// <summary>
        /// 已從手牌拖出、目前被玩家「手持」的卡牌物件。
        /// 用來管理卡牌拖曳後的行為與出牌判斷。
        /// </summary>
        public BattleCard HeldOutHandCard = null;

        /// <summary>
        /// 拖曳卡牌時，卡牌與滑鼠的世界座標之間的偏移量。
        /// z 值常被用來調整卡牌在場景中的深度，避免卡片被遮擋。
        /// </summary>
        public Vector3 HeldCardOffset;
        
        /// <summary>
        /// 滑鼠是否目前位於手牌 UI 區域內。
        /// 可用來控制選擇與拖曳行為是否生效。
        /// </summary>
        public bool MouseInsideHand;

        /// <summary>
        /// 當卡牌被拖出手牌且尚未出牌時，
        /// 儲存該卡牌原本在手牌中的索引，以便還原。
        /// </summary>
        public int TempDraggedInHandCardIndex = -1;

        /// <summary>
        /// 若該卡牌為需要選擇單體敵人（如攻擊目標）才能出牌的類型，
        /// 則此值為 true。
        /// </summary>
        public bool IsUsingSelectingEffectCard;

        /// <summary>
        /// 是否目前已鎖定某個敵人為目標並進行高亮顯示。
        /// 主要配合箭頭特效與高亮控制器使用。
        /// </summary>
        public bool EnemyIsBeingSelected;
        
    }
}
