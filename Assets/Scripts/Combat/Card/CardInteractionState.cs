using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat.Card
{
    [Serializable]
    public class CardInteractionState
    {
        public HandState HandState = HandState.Hover;
        /// <summary>目前與滑鼠距離最近、會被選中的卡牌索引。若為 -1 則表示沒有選中任何卡。 </summary>
        public int ClosestMouseIndex  = -1;

        public int SelectedIndex = -1;
 

        /// <summary>目前正在拖曳中的卡牌索引。若為 -1 則表示沒有拖曳任何卡。 </summary>
        public int DraggedInsideHandIndex  = -1;

        /// <summary>當卡片拖出手牌後，手上裝的卡牌物件引用。 </summary>
        public BattleCard HeldOutHandCard = null;

        /// <summary>當卡牌被拖著時，手牌位置與滑鼠世界座標的偏移量 (Vector3.z 用來控制渲染順序)。</summary>
        public Vector3 HeldCardOffset;

        /// <summary>卡牌在拖曳時的傾斜角度 (用來做傾斜效果)。</summary>
        public Vector2 HeldCardTilt { get; set; }

        /// <summary>用於計算卡牌傾斜的中介力。</summary>
        public Vector2 Force { get; set; }

        /// <summary>滑鼠在世界座標系的即時座標。</summary>
        public Vector3 MouseWorldPos { get; set; }

        /// <summary>上一幀滑鼠在螢幕上的座標 (用於計算滑鼠移動向量)。</summary>
        public Vector2 PrevMousePos;

        /// <summary>滑鼠在螢幕的位移向量 (用於卡牌傾斜計算)。</summary>
        public Vector2 MousePosDelta { get; set; }

        /// <summary>滑鼠是否在手牌範圍內 (手牌 UI 區域中的判斷結果)。</summary>
        public bool MouseInsideHand;

        /// <summary>若當前拖曳卡牌需要選擇敵人，這裡存放被拖出手牌時的卡牌索引 (待還給手牌時用)。</summary>
        public int TempDraggedInHandCardIndex  = -1;

        /// <summary>當正在拖曳一張需要指定單體敵人的卡，則此值為 true，否則 false。</summary>
        public bool IsUsingSelectingEffectCard;

        /// <summary>是否目前處於 Highlight 效果 (選擇單一敵人的狀態)。</summary>
        public bool EnemyIsBeingSelected;
    }


    public enum HandState
    {
        Hover, // 閒置
        Selected, // 有手牌被選擇
        DraggedInsideHand, // 在手牌區被拖移
        DraggedOutsideHand, // 在手牌需外被拖移
    }
}