using System.Collections.Generic;
using UnityEngine;
using Card.Data;
using Characters;
using Characters.Enemy;
using Kalkatos.DottedArrow;
using Managers;
using Sirenix.OdinInspector;

namespace Combat.Card
{
    /// <summary>
    /// 管理玩家手牌的顯示、選擇、拖曳與出牌邏輯
    /// </summary>
    public class HandController : MonoBehaviour
    {
        #region Inspector Exposed Fields

        [Header("卡牌顯示設定")]  
        [SerializeField] private bool isUprightOnSelect = true;            // 選牌時卡牌是否直立
        [SerializeField] private bool enableTilt = true;                   // 是否在拖曳時使用卡牌傾斜效果

        [Header("手牌佈局設定")]
        [SerializeField] private Vector3 curveStartPoint = new Vector3(2f, -0.2f, 0);  // 貝茲曲線起點 (Local Space)
        [SerializeField] private Vector3 curveEndPoint   = new Vector3(-2f, -0.2f, 0); // 貝茲曲線終點 (Local Space)
        [SerializeField] private Vector2 handAreaOffset = new Vector2(0, 0.3f);        // 手牌區域中心偏移 (Local Space)
        [SerializeField] private Vector2 handAreaSize   = new Vector2(9, 1.7f);         // 手牌區域範圍大小
        [SerializeField] private float selectedCardScale = 1.5f;                        // 選中卡牌縮放倍率
        [SerializeField] private float selectedCardYOffset = 0.3f;                      // 選中卡牌的垂直偏移

        [Header("場景參考")]
        public Transform discardPileTransform;       // 棄牌堆位置
        public Transform exhaustPileTransform;       // 消耗堆位置
        public Transform drawPileTransform;          // 抽牌堆位置
        public LayerMask targetLayerMask;            // 目標角色圖層 (敵人、玩家等)
        public Camera selectionCamera;               // 用於卡牌選擇的攝影機
        public List<BattleCard> handCards = new List<BattleCard>();  // 玩家當前手牌清單

        [Header("選擇特效")]
        [SerializeField] private ArrowController arrowEffectController;  // 卡牌箭頭特效控制器

        #endregion

        #region Private Fields

        private MouseHandler mouseHandler;                    // 處理滑鼠輸入與世界座標
        private CardInteractionState interactionState;        // 保存選牌、拖曳等互動狀態
        private Rect handAreaRect;                            // 手牌區域（Local Space）
        private Vector3 curvePointA, curvePointB, curvePointC; // 貝茲曲線三個世界座標控制點
        private Plane mouseHitPlane;                          // 滑鼠射線對應的平面
        private CharacterHighlightController highlightController = new CharacterHighlightController();  // 角色被選擇時控制
        private Camera mainCamera;                           // 場景主攝影機
        [SerializeField] private Vector3 selectingEffectCardPosition; // 指定目標特效卡牌顯示位置

        private bool reorderInHierarchy = false;             // 是否同步更新 Transform 層級順序
        private bool showDebugGizmos    = true;              // 是否在編輯器顯示偵錯 Gizmos

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Start()
        {
            InitializeHandArea();
            arrowEffectController.SetCamera(selectionCamera);
        }

        private void Update()
        {
            if (!IsDraggingEnabled) return;

            UpdateMouseState();               // 更新滑鼠世界位置與是否在手牌範圍內
            EvaluateSelectionState();         // 根據滑鼠位置與狀態更新選擇/拖曳卡牌邏輯
            LayoutHandCards();               // 根據貝茲曲線排列卡牌位置
            UpdateSelectedOrDraggedCard();   // 處理選中或拖曳中的卡牌位置與動畫
            HandleDragWithinHand();          // 檢查是否拖出卡牌
            HandleDragOutsideHand();         // 拖出後的持續操作與出牌行為
            HighlightSingleEnemyTarget();    // 若需選擇敵人，則進行目標高亮
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!showDebugGizmos) return;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color  = Color.blue;

            // 繪製曲線起終點
            Gizmos.DrawSphere(curveStartPoint, 0.03f);
            Gizmos.DrawSphere(curveEndPoint,   0.03f);

            // 繪製貝茲曲線
            Vector3 prev = curveStartPoint;
            for (int i = 1; i <= 20; i++)
            {
                float t = i / 20f;
                Vector3 next = LayoutCurveUtility.GetCurvePoint(curveStartPoint, Vector3.zero, curveEndPoint, t);
                Gizmos.DrawLine(prev, next);
                prev = next;
            }

            // 手牌區域
            Gizmos.color = interactionState != null && interactionState.MouseInsideHand ? Color.red : Color.blue;
            Gizmos.DrawWireCube(handAreaOffset, handAreaSize);
        }
#endif

        #endregion

        #region Initialization

        [Button]
        private void InitializeHandArea()
        {
            // 1. 計算世界座標下的貝茲曲線控制點
            curvePointA = transform.TransformPoint(curveStartPoint);
            curvePointB = transform.position;
            curvePointC = transform.TransformPoint(curveEndPoint);

            // 2. 設定手牌區域矩形 (Local Space)
            handAreaRect = new Rect(handAreaOffset - handAreaSize / 2, handAreaSize);

            // 3. 建立滑鼠射線對應的平面 (World XY Plane)
            mouseHitPlane = new Plane(-Vector3.forward, transform.position);

            // 4. 初始化互動狀態
            interactionState = new CardInteractionState { };

            // 5. 初始化滑鼠處理
            mouseHandler = new MouseHandler(selectionCamera, mouseHitPlane, enableTilt);
        }

        #endregion

        #region Public API

        /// <summary>是否允許拖曳操作</summary>
        public bool IsDraggingEnabled { get; private set; } = true;

        /// <summary>啟用拖曳</summary>
        public void EnableDragging()  => IsDraggingEnabled = true;
        /// <summary>停用拖曳</summary>
        public void DisableDragging() => IsDraggingEnabled = false;

        #endregion

        #region Mouse State

        /// <summary>更新滑鼠世界座標與手牌區域內判斷</summary>
        private void UpdateMouseState()
        {
            mouseHandler.Update();
            Vector3 worldPos = mouseHandler.MouseWorldPos;
            Vector3 localPos = transform.InverseTransformPoint(worldPos);
            interactionState.MouseInsideHand = handAreaRect.Contains(new Vector2(localPos.x, localPos.y));
        }

        #endregion

        #region Selection Evaluation

        /// <summary>計算最接近滑鼠的卡片索引，並判斷是否開始拖曳</summary>
        private void EvaluateSelectionState()
        {
            // 尋找離滑鼠最近的卡片
            FindClosestMouseIndex();
            
            // 檢查卡牌是否被選擇、拖曳
            int count = handCards.Count;
            for (int i = 0; i < count; i++)
            {
                var card       = handCards[i];
                var cardTrans  = card.transform;
 
                // 判斷滑鼠在手牌範圍內且沒有正在拖曳別張卡
                bool noHeldCard      = interactionState.HeldOutHandCard == null;
                bool draggingInHand  = interactionState.DraggedInsideHandIndex != -1;
                bool isHoverClosest  = noHeldCard && interactionState.ClosestMouseIndex == i && interactionState.MouseInsideHand && !draggingInHand;

                // 選中卡片
                if (isHoverClosest)
                    interactionState.SelectedIndex = i;
                
                // 按下滑鼠開始拖曳
                if (isHoverClosest && mouseHandler.MouseButton)
                {
                    interactionState.DraggedInsideHandIndex = i;
                    interactionState.SelectedIndex           = -1;
                    interactionState.HeldCardOffset          = cardTrans.position - mouseHandler.MouseWorldPos;
                    interactionState.HeldCardOffset.z        = -0.1f;
                }
            }
            
            // 滑鼠離開選擇區塊，取消選擇
            if (!interactionState.MouseInsideHand)
            {
                interactionState.SelectedIndex          = -1;
            }
            
            // 若不允許出牌，重置所有選擇
            if (!CombatManager.Instance.CanSelectCards)
            {
                interactionState.SelectedIndex          = -1;
                interactionState.DraggedInsideHandIndex = -1;
            }

            // 放開滑鼠則清除拖曳狀態
            if (!mouseHandler.MouseButton)
            {
                interactionState.DraggedInsideHandIndex = -1;
                interactionState.HeldCardOffset         = Vector3.zero;
            }
        }

        private void FindClosestMouseIndex()
        {
            int count = handCards.Count;
            float closestDistSq = CalculateClosestCardDistanceSquared();

            for (int i = 0; i < count; i++)
            {
                float t        = (i + 0.5f) / count;
                Vector3 curveP = LayoutCurveUtility.GetCurvePoint(curvePointA, curvePointB, curvePointC, t);
                float dSq      = (curveP - mouseHandler.MouseWorldPos).sqrMagnitude;

                // 找最接近滑鼠的卡
                if (dSq < closestDistSq)
                {
                    closestDistSq = dSq;
                    interactionState.ClosestMouseIndex = i;
                }
            }
        }

        #endregion

        #region Layout & Lerp Hand Cards

        /// <summary>按貝茲曲線佈局手牌，並對非選中卡進行位置與旋轉平滑過渡</summary>
        private void LayoutHandCards()
        {
            int count = handCards.Count;
            for (int i = 0; i < count; i++)
            {
                var card      = handCards[i];
                var cardTrans = card.transform;
                float t       = (i + 0.5f) / count;
                Vector3 targetPos = LayoutCurveUtility.GetCurvePoint(curvePointA, curvePointB, curvePointC, t);

                // 設置卡牌可用材質
                card.SetInactiveMaterialState(!PlayCardJudgment.EnoughResourceToUseCard(card));

                bool noHeld     = interactionState.HeldOutHandCard == null;
                bool draggingIn = noHeld && interactionState.DraggedInsideHandIndex == i;
                bool selected   = noHeld && interactionState.SelectedIndex == i;
                if (draggingIn || selected) continue;

                // 方向與平滑插值
                Vector3 upDir   = LayoutCurveUtility.GetCurveNormal(curvePointA, curvePointB, curvePointC, t);
                Vector3 forward = Vector3.forward;
                Vector3 destPos = targetPos;
                destPos.z = transform.position.z + t * 0.5f;

                // 旋轉到曲線法線方向
                cardTrans.rotation = Quaternion.RotateTowards(
                    cardTrans.rotation,
                    Quaternion.LookRotation(forward, upDir),
                    80f * Time.deltaTime
                );

                // 插值移動
                cardTrans.position = Vector3.MoveTowards(cardTrans.position, destPos, 16f * Time.deltaTime);
                cardTrans.localScale = Vector3.one;
            }
        }

        #endregion

        #region Selected or Dragged Card Handling

        /// <summary>更新正在選中或拖曳的卡片位置與顯示</summary>
        private void UpdateSelectedOrDraggedCard()
        {
            // 如果已經有卡片拿出手牌，跳過
            if (interactionState.HeldOutHandCard != null) return;

            int idx = interactionState.DraggedInsideHandIndex != -1
                ? interactionState.DraggedInsideHandIndex
                : interactionState.SelectedIndex;
            if (idx == -1) return;

            var card      = handCards[idx];
            var cardTrans = card.transform;
            float t       = (idx + 0.5f) / handCards.Count;
            Vector3 basePos = LayoutCurveUtility.GetCurvePoint(curvePointA, curvePointB, curvePointC, t);
            Vector3 upDir   = LayoutCurveUtility.GetCurveNormal(curvePointA, curvePointB, curvePointC, t);
            if (isUprightOnSelect) upDir = Vector3.up;

            Vector3 targetPos = basePos + upDir * 0.3f;
            targetPos.z = transform.position.z;
            targetPos.y += selectedCardYOffset;

            // 縮放
            cardTrans.localScale = selectedCardScale * Vector3.one;
            cardTrans.rotation    = Quaternion.RotateTowards(
                cardTrans.rotation,
                Quaternion.LookRotation(Vector3.forward, upDir),
                80f * Time.deltaTime
            );

            // 拖曳則直接跟隨滑鼠
            if (interactionState.DraggedInsideHandIndex != -1)
            {
                var pos = mouseHandler.MouseWorldPos + interactionState.HeldCardOffset;
                cardTrans.position = Vector3.MoveTowards(cardTrans.position, pos, 16f * Time.deltaTime);
            }
            else
            {
                // 平滑回到偏移位置
                cardTrans.position = Vector3.MoveTowards(cardTrans.position, targetPos, 16f * Time.deltaTime);
            }
        }

        #endregion

        #region Drag Within Hand

        /// <summary>處理手牌內拖曳排序與拖出判斷</summary>
        private void HandleDragWithinHand()
        {
            if (interactionState.DraggedInsideHandIndex != -1 && mouseHandler.MouseButton && !interactionState.MouseInsideHand)
            {
                OnCardDraggedOutsideHand();
            }
        }

        /// <summary>當卡片拖出手牌時執行的核心邏輯</summary>
        private void OnCardDraggedOutsideHand()
        {
            interactionState.HeldOutHandCard        = handCards[interactionState.DraggedInsideHandIndex];
            interactionState.IsUsingSelectingEffectCard = interactionState.HeldOutHandCard.SpecifiedEnemyTarget;
            interactionState.TempDraggedInHandCardIndex = interactionState.DraggedInsideHandIndex;

            if (interactionState.IsUsingSelectingEffectCard)
                arrowEffectController.SetupAndActivate(interactionState.HeldOutHandCard.transform);

            RemoveCardAt(interactionState.DraggedInsideHandIndex);
            interactionState.DraggedInsideHandIndex = -1;

            CardEvents.CardDragged(interactionState.HeldOutHandCard);
        }

        #endregion

        #region Drag Outside Hand

        /// <summary>處理卡片被拖出手牌後的持續跟隨與出牌行為</summary>
        private void HandleDragOutsideHand()
        {
            var held = interactionState.HeldOutHandCard;
            if (held == null) return;

            var cardTrans = held.transform;
            Vector3 pos   = mouseHandler.MouseWorldPos + interactionState.HeldCardOffset;

            // 若需指定單一敵人，固定顯示在特殊位置
            if (interactionState.IsUsingSelectingEffectCard)
            {
                cardTrans.localScale = selectedCardScale * Vector3.one;
                pos.x = selectingEffectCardPosition.x;
                pos.y = selectingEffectCardPosition.y;
                cardTrans.position = Vector3.MoveTowards(cardTrans.position, pos, 12f * Time.deltaTime);
            }
            else
            {
                cardTrans.position = pos;
            }




            // 顯示高亮
            highlightController.OnDraggedCardOutsideHand(held.TargetChoose);

            // 無法選卡或滑鼠回到手牌區，則還原
            if (!CombatManager.Instance.CanSelectCards || interactionState.MouseInsideHand)
            {
                ReturnCardToHand();
                return;
            }

            // 放開滑鼠執行出牌
            TryPlayCard();
        }

        /// <summary>將拖出手牌卡牌還原回手牌</summary>
        private void ReturnCardToHand()
        {
            if (interactionState.IsUsingSelectingEffectCard)
            {
                highlightController.DeactivateAllHighlights();
                arrowEffectController.Deactivate();
            }
            interactionState.DraggedInsideHandIndex = interactionState.TempDraggedInHandCardIndex;
            interactionState.TempDraggedInHandCardIndex = -1;

            InsertCard(interactionState.HeldOutHandCard, interactionState.DraggedInsideHandIndex);
            interactionState.HeldOutHandCard = null;
        }

        #endregion

        #region Play & Return Card Logic

        /// <summary>嘗試執行出牌，資源與目標檢查</summary>
        private void TryPlayCard()
        {
            if (!Input.GetMouseButtonUp(0)) return;

            highlightController.DeactivateAllHighlights();
            arrowEffectController.Deactivate();

            bool returnToHand = true;
            var held = interactionState.HeldOutHandCard;
            var hitCharacter = RaycastCharacterUnderMouse(mouseHandler.MousePos);

            if (CardPlayValidator.CanUseCard(held, hitCharacter))
            {
                held.Use(new List<CharacterBase> { hitCharacter });
                highlightController.DeactivateAllHighlights();
                returnToHand = false;
                CardEvents.CardPlayed(held);
            }

            if (returnToHand)
            {
                InsertCard(held, interactionState.ClosestMouseIndex);
                CardEvents.CardReturnedToHand(held);
            }
            interactionState.HeldOutHandCard = null;
        }

        #endregion

        #region Single Enemy Highlight

        /// <summary>若卡牌需要選擇單一敵人，進行高亮顯示</summary>
        private void HighlightSingleEnemyTarget()
        {
            var held = interactionState.HeldOutHandCard;
            if (held == null || !held.SpecifiedEnemyTarget) return;

            Enemy target = null;
            Ray ray;
            RaycastHit hit;
            ray = mainCamera.ScreenPointToRay(mouseHandler.MousePos);
            if (Physics.Raycast(ray, out hit, 1000f, targetLayerMask))
                target = hit.collider.GetComponent<Enemy>();

            if (target != null && !interactionState.EnemyIsBeingSelected)
            {
                highlightController.ActivateEnemyHighlight(target);
                arrowEffectController.OnEnterEnemy();
                interactionState.EnemyIsBeingSelected = true;
                held.UpdateCardDisplay();
            }
            else if (target == null && interactionState.EnemyIsBeingSelected)
            {
                highlightController.DeactivateEnemyHighlights();
                arrowEffectController.OnLeaveEnemy();
                interactionState.EnemyIsBeingSelected = false;
                held.UpdateCardDisplay();
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>計算最接近滑鼠的卡片距離平方，供選牌判斷</summary>
        private float CalculateClosestCardDistanceSquared()
        {
            int idx = interactionState.ClosestMouseIndex;
            if (idx >= 0 && idx < handCards.Count)
            {
                float t = (idx + 0.5f) / handCards.Count;
                Vector3 p = LayoutCurveUtility.GetCurvePoint(curvePointA, curvePointB, curvePointC, t);
                return (p - mouseHandler.MouseWorldPos).sqrMagnitude;
            }
            return float.MaxValue;
        }

        /// <summary>射線檢測滑鼠下的角色，若命中回傳 CharacterBase，否則 null</summary>
        private CharacterBase RaycastCharacterUnderMouse(Vector2 mousePos)
        {
            Ray ray;
            RaycastHit hit;
            ray = mainCamera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out hit, 1000f, targetLayerMask))
                return hit.collider.GetComponent<CharacterBase>();
            return null;
        }

        #endregion

        #region Card Management

        /// <summary>移動手牌列表中卡片位置並更新 UI 層級</summary>
        public void MoveCardTo(int fromIndex, int toIndex)
        {
            if (fromIndex == toIndex) return;
            var card = handCards[fromIndex];
            handCards.RemoveAt(fromIndex);
            handCards.Insert(toIndex, card);
            if (reorderInHierarchy)
                card.transform.SetSiblingIndex(toIndex);
        }

        /// <summary>將卡片插入手牌，可指定位置，否則置於末端</summary>
        public void InsertCard(BattleCard card, int index = -1)
        {
            if (index < 0) index = handCards.Count;
            handCards.Insert(index, card);
            if (reorderInHierarchy)
            {
                card.transform.SetParent(transform);
                card.transform.SetSiblingIndex(index);
            }
        }

        /// <summary>根據索引從手牌移除卡片</summary>
        public void RemoveCardAt(int index)
        {
            var card = handCards[index];
            if (reorderInHierarchy)
            {
                card.transform.SetParent(transform.parent);
                card.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            }
            handCards.RemoveAt(index);
        }

        /// <summary>依據 CardData 尋找對應卡片並執行棄置</summary>
        public void DiscardCard(CardData data)
        {
            var card = handCards.Find(c => c.CardData == data);
            if (card != null) card.Discard();
        }

        #endregion
    }
}
