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
    public class HandController : MonoBehaviour
    {
        #region Inspector Exposed Fields

        [Header("Card Settings")]
        [SerializeField] private bool cardUprightWhenSelected = true;
        [SerializeField] private bool cardTilt = true;

        [Header("Hand Settings")]
        [SerializeField] [Range(0, 5)] private float selectionSpacing = 1;
        [SerializeField] private Vector3 curveStart = new Vector3(2f, -0.2f, 0);
        [SerializeField] private Vector3 curveEnd = new Vector3(-2f, -0.2f, 0);
        [SerializeField] private Vector2 handOffset = new Vector2(0, 0.3f);
        [SerializeField] private Vector2 handSize = new Vector2(9, 1.7f);
        [SerializeField] private float selectedCardSize = 1.5f;
        [SerializeField] private float selectedCardOffsetY;

        [Header("References")]
        public Transform discardTransform;
        public Transform exhaustTransform;
        public Transform drawTransform;
        public LayerMask selectableLayer;
        public LayerMask targetLayer;
        public Camera cam = null;
        public List<BattleCard> hand; // 手牌列表 (目前在手上的卡牌物件引用)

        [Header("Selecting Effect")]
        [SerializeField] private ArrowController arrowController;

        #endregion

        #region Private Fields

        [ShowInInspector]
        private MouseHandler _mouseHandler;
        [ShowInInspector]
        private CardInteractionState _state;
        private Rect _handBounds;         // 記錄手牌 UI 區域 (用來判斷滑鼠是否在手牌範圍內)
        private Vector3 _a, _b, _c;       // 用於排布手牌貝茲曲線的三個控制點
        private Plane _plane;             // 用於射線判斷滑鼠在世界 XY 平面下的座標
        private CharacterHighlightController _highlightController = new CharacterHighlightController();
        private Camera _mainCam;
        [SerializeField] private Vector3 selectingEffectCardPos;

        private bool updateHierarchyOrder = false;  // 是否要同步調整 Transform 的 sibling index
        private bool showDebugGizmos = true;

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            _mainCam = Camera.main;
        }

        private void Start()
        {
            InitHand();
            arrowController.SetCamera(cam);
        }

        private void Update()
        {
            Debug.Log($"Close: {_state.ClosestMouseIndex}, "+
                      $"MouseInside {_state.MouseInsideHand}\n" +
                      $"Selected {_state.SelectedIndex}, " +
                      $"Drag {_state.DraggedInsideHandIndex}" );
            if (!IsDraggingActive) return;

            UpdateMouseState();

            // 3. 計算「目前與 selectedIndex 卡牌的距離平方」，在 HandleCardsInHand 中用來挑選最接近滑鼠的卡
            GetDistanceToCurrentSelectedCard(out float sqrDistance);

            // 判斷選擇中、拖移中的卡片
            HandleJudgeState( sqrDistance);
            
            // 4. 處理手牌呈現、選卡、拖曳的邏輯
            HandleCardsWithoutSelectedAndDragged();

            // 處理選擇中的卡片的顯示
            HandleSelectedOrDraggedCard();
            
            // 5. 處理若在手牌內移動拖曳 (卡片在手牌內被拖動)
            HandleDraggedCardInsideHand();

            // 6. 處理若卡片已被拖出手牌，檢查放置目標或放回手牌 (卡片在手牌外面)
            HandleDraggedCardOutsideHand();

            // 7. 處理若卡片需要指定單一敵人時，顯示 Highlight 效果
            HandleCardSelectingSingleEnemy();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.blue;

            // 顯示貝茲曲線的兩端點
            Gizmos.DrawSphere(curveStart, 0.03f);
            Gizmos.DrawSphere(curveEnd, 0.03f);

            // 畫出近似曲線
            Vector3 p1 = curveStart;
            for (int i = 0; i < 20; i++)
            {
                float t = (i + 1) / 20f;
                Vector3 p2 = LayoutCurveUtility.GetCurvePoint(curveStart, Vector3.zero, curveEnd, t);
                Gizmos.DrawLine(p1, p2);
                p1 = p2;
            }

            // 若滑鼠在手牌區域內，改為紅色
            if (_state != null && _state.MouseInsideHand)
                Gizmos.color = Color.red;

            Gizmos.DrawWireCube(handOffset, handSize);
        }
#endif

        #endregion

        #region Initialization

        [Button]
        private void InitHand()
        {
            // 1. 計算貝茲曲線的控制點：world 空間下的 A, B, C
            _a = transform.TransformPoint(curveStart);
            _b = transform.position;
            _c = transform.TransformPoint(curveEnd);

            // 2. 設定手牌 Bounds (手牌 UI 區域)
            _handBounds = new Rect((handOffset - handSize / 2), handSize);

            // 3. 設定滑鼠射線所參考的 XY 平面 (Z 軸朝向相機)
            _plane = new Plane(-Vector3.forward, transform.position);

            // 4. 初始化互動狀態 (selected, dragged, etc.)
            _state = new CardInteractionState { PrevMousePos = Input.mousePosition };

            // 5. 初始化滑鼠處理物件
            _mouseHandler = new MouseHandler(cam, _plane, cardTilt);
        }

        #endregion

        #region Public Properties

        /// <summary>是否允許拖曳手牌 (若 false，Update 會直接 return)</summary>
        [ShowInInspector]
        public bool IsDraggingActive { get; private set; } = true;

        public void EnableDragging()  => IsDraggingActive = true;
        public void DisableDragging() => IsDraggingActive = false;

        #endregion

        private void UpdateMouseState()
        {
            // 1. 更新滑鼠狀態 (位置、世界坐標、傾斜量等)
            _mouseHandler.Update();
            Vector3 mouseWorldPos = _mouseHandler.MouseWorldPos;

            // 2. 判斷滑鼠是否在手牌 Bounds 內 (以 HandController 的 local space 計算)
            Vector3 localPoint = transform.InverseTransformPoint(mouseWorldPos);
            _state.MouseInsideHand = _handBounds.Contains(new Vector2(localPoint.x, localPoint.y));
        }
        
        
        #region 處理手牌 (手牌區域內) —— HandleCardsInHand

        private void HandleJudgeState( float sqrDistance)
        {
            // 檢查
            var count = hand.Count;
            for (int i = 0; i < count; i++)
            {
                BattleCard card = hand[i];
                Transform cardTransform = card.transform;
                
                // 檢查離滑鼠最近的那張卡
                float t = (i + 0.5f) / count ;
                Vector3 p = LayoutCurveUtility.GetCurvePoint(_a, _b, _c, t);
                float d = (p - _mouseHandler.MouseWorldPos).sqrMagnitude;
                if (d < sqrDistance)
                {
                    sqrDistance = d;
                    _state.ClosestMouseIndex = i;
                }
                
                // 設定 SelectedCard
                bool noCardHeld = _state.HeldOutHandCard == null;
                bool haveDraggedCard =  _state.DraggedInsideHandIndex != -1;
                bool onSelectedCard =  noCardHeld && (_state.ClosestMouseIndex == i) 
                                                  && _state.MouseInsideHand && !haveDraggedCard;
               
                
                if (onSelectedCard)
                {
                    _state.SelectedIndex = i;
                }
                // 6. 當滑鼠箭頭懸停在此卡牌上並且按下左鍵時，開始拖曳
                if (onSelectedCard && _mouseHandler.MouseButton)
                {
                    _state.DraggedInsideHandIndex = i;
                    _state.SelectedIndex = -1;
                    _state.HeldCardOffset = cardTransform.position - _mouseHandler.MouseWorldPos;
                    // _state.HeldCardOffset.z = -0.1f; // 微調 z 軸讓拖曳卡牌顯示在最前
                }
                
                // 如果現在不能選擇卡牌，重製
                if (!CombatManager.Instance.CanSelectCards)
                {
                    // 若不允許選卡，重設索引
                    _state.SelectedIndex = -1;
                    _state.DraggedInsideHandIndex  = -1;
                }
            }
            
            // 滑鼠點擊
            if (!_mouseHandler.MouseButton)
            {
                _state.DraggedInsideHandIndex = -1;
                _state.HeldCardOffset = Vector3.zero;
            }
        }
        
        

        /// <summary>
        /// 將整個手牌從左到右依照曲線排布，並判斷卡牌是否被選取 / 拖曳。
        /// </summary>
        private void HandleCardsWithoutSelectedAndDragged()
        {
            var count = hand.Count;
            for (int i = 0; i < count; i++)
            {
                BattleCard card = hand[i];
                Transform cardTransform = card.transform;

                // 1. 顯示卡牌是否可用 (如果資源不足就設定成 Inactive Material)
                card.SetInactiveMaterialState(!PlayCardJudgment.EnoughResourceToUseCard(card));

                // 9. 如果目前允許選卡 (CombatManager.CanSelectCards)，挑出最靠近滑鼠的那張卡
                float t = (i + 0.5f) / count ;
                Vector3 p = LayoutCurveUtility.GetCurvePoint(_a, _b, _c, t);
                
                // 2.
                bool noCardHeld = _state.HeldOutHandCard == null;;
                bool onDraggedCard  = noCardHeld && (_state.DraggedInsideHandIndex  == i);
                bool onSelectedCard =  noCardHeld && (_state.SelectedIndex == i);
                
                if (onSelectedCard || onDraggedCard)
                {
                  continue;   
                }
                
                // 3. 計算卡牌應該朝向的「上方」向量，以及預設位置
                Vector3 cardUp = LayoutCurveUtility.GetCurveNormal(_a, _b, _c, t);
                Vector3 cardPos = p;
                Vector3 cardForward = Vector3.forward;

                // 4. 如果滑鼠正在選到或拖曳到該卡：把卡牌抬高、縮放、直立
                cardTransform.localScale = Vector3.one;
                cardPos.z = transform.position.z + t * 0.5f;

                // 5. 逐漸將卡牌轉向到 (forward, up) 的朝向
                cardTransform.rotation = Quaternion.RotateTowards(
                    cardTransform.rotation,
                    Quaternion.LookRotation(cardForward, cardUp),
                    80f * Time.deltaTime
                );
                
                // 8. 否則用 MoveTowards 平滑過渡到預設位置
                Vector3 smoothPos = Vector3.MoveTowards(cardTransform.position, cardPos, 16f * Time.deltaTime);
                cardTransform.position = smoothPos;
            }
        }

        private void HandleSelectedOrDraggedCard()
        {
            int i = -1;
            // 有正拖到外面的卡片
            bool haveHeldOutCard = _state.HeldOutHandCard != null;
            if (haveHeldOutCard)
            {
                return;
            }
            // 有正在手牌中拖動的卡片
            bool haveDraggedCard = _state.DraggedInsideHandIndex != -1;
            if (haveDraggedCard)
            {
                i = _state.DraggedInsideHandIndex;
            }
            else
            {
                i = _state.SelectedIndex;
            }
            // 如果沒有正在拖動、選擇的卡片，不需要處理
            if (i == -1 )
            {
                return;
            }
            BattleCard card = hand[i];
            Transform cardTransform = card.transform;
            // 9. 如果目前允許選卡 (CombatManager.CanSelectCards)，挑出最靠近滑鼠的那張卡
            float t = (i + 0.5f) / hand.Count ;
            Vector3 p = LayoutCurveUtility.GetCurvePoint(_a, _b, _c, t);
            
            
            // 3. 計算卡牌應該朝向的「上方」向量，以及預設位置
            Vector3 cardUp = LayoutCurveUtility.GetCurveNormal(_a, _b, _c, t);
            Vector3 cardPos = p + cardTransform.up * 0.3f;
            Vector3 cardForward = Vector3.forward;

            // 4. 如果滑鼠正在選到或拖曳到該卡：把卡牌抬高、縮放、直立
            if (cardUprightWhenSelected) cardUp = Vector3.up;
            cardTransform.localScale = selectedCardSize * Vector3.one;
            cardPos.z = transform.position.z;
            cardPos.y += selectedCardOffsetY;

            // 5. 逐漸將卡牌轉向到 (forward, up) 的朝向
            cardTransform.rotation = Quaternion.RotateTowards(
                cardTransform.rotation,
                Quaternion.LookRotation(cardForward, cardUp),
                80f * Time.deltaTime
            );


            // 7. 如果此卡正在被拖曳 (onDraggedCard) 而滑鼠左鍵仍然按著，就追蹤滑鼠位置
            if (haveDraggedCard)
            {
                var mouseWorldPos = _mouseHandler.MouseWorldPos;
                cardPos = mouseWorldPos + _state.HeldCardOffset;
                cardTransform.position = cardPos;
            }
            else
            {
                // 8. 否則用 MoveTowards 平滑過渡到預設位置
                Vector3 smoothPos = Vector3.MoveTowards(cardTransform.position, cardPos, 16f * Time.deltaTime);
                cardTransform.position = smoothPos;
            }
        }

        #endregion

        #region 處理手牌內部拖曳 —— HandleDraggedCardInsideHand

        /// <summary>
        /// 如果玩家正在拖曳手牌區域內的卡，此函式處理拖曳過程中卡牌可能向左或向右移動的排序邏輯。<br/>
        /// 若在手牌內按下左鍵並拖出手牌外面，則呼叫 OnCardDraggedOutsideHand() 轉接到下一步處理 (卡牌已被拖出手牌)。
        /// </summary>
        private void HandleDraggedCardInsideHand()
        {
            bool mouseButton = _mouseHandler.MouseButton;
            // 2. 如果當前有某張卡在拖曳狀態，且滑鼠位於手牌範圍之外，就視為「已經拖出手牌」 
            if (_state.DraggedInsideHandIndex != -1 && mouseButton && !_state.MouseInsideHand)
            {
                OnCardDraggedOutsideHand();
            }
        }

        /// <summary>
        /// 當卡片「真正被拖出手牌」的時候呼叫。<br/>
        /// 將該卡保存在 _state.HeldCard，並從手牌列表中移除 (RemoveCardFromHand)，<br/>
        /// 若為需要選擇敵人的卡，則啟動箭頭 Highlight 特效。
        /// </summary>
        private void OnCardDraggedOutsideHand()
        {
            _state.HeldOutHandCard = hand[_state.DraggedInsideHandIndex];
            _state.IsUsingSelectingEffectCard = _state.HeldOutHandCard.SpecifiedEnemyTarget;
            _state.TempDraggedInHandCardIndex = _state.DraggedInsideHandIndex;
            if (_state.IsUsingSelectingEffectCard)
            {
               
                arrowController.SetupAndActivate(_state.HeldOutHandCard.transform);
            }

            RemoveCardFromHand(_state.DraggedInsideHandIndex);
            _state.DraggedInsideHandIndex = -1; 

            // 觸發卡牌被拖出的事件 (給外部訂閱者)
            CardEvents.CardDragged(_state.HeldOutHandCard);
        }

        #endregion

        #region 處理手牌外拖曳 —— HandleDraggedCardOutsideHand

        /// <summary>
        /// 處理「卡片已經被拖出手牌區域」的狀況：<br/>
        /// 1. 若滑鼠尚未放開，則持續跟著滑鼠飛行並傾斜。<br/>
        /// 2. 若這張卡需要指定單一敵人，則持續顯示敵人 Highlight。<br/>
        /// 3. 當滑鼠放開之後，嘗試打出卡片 (PlayCard)；<br/>
        ///    若無法打出，就自動放回手牌 (AddCardToHand)。
        /// </summary>
        private void HandleDraggedCardOutsideHand()
        {
            if (_state.HeldOutHandCard != null)
            {
                Transform cardTransform = _state.HeldOutHandCard.transform;

                // 1. 計算卡牌的朝向與位置
                var mouseWorldPos = _mouseHandler.MouseWorldPos;
                Vector3 cardPos = mouseWorldPos + _state.HeldCardOffset;



                // 2. 如果正在執行「指定敵人」的特效 (需要箭頭指向)，則放大並把座標鎖在特效位置
                if (_state.IsUsingSelectingEffectCard)
                {
                    cardTransform.localScale = selectedCardSize * Vector3.one;
                    cardPos.x = selectingEffectCardPos.x;
                    cardPos.y = selectingEffectCardPos.y;
                }

                cardTransform.position = cardPos;

                // 3. 顯示卡牌拖出手牌後的 Highlight 特效 (將由 CharacterHighlightController 處理)
                ActionTargetType actionTargetType = _state.HeldOutHandCard.TargetChoose;
                _highlightController.OnDraggedCardOutsideHand(actionTargetType);

                // 4. 如果不可選卡 (CombatManager.CanSelectCards == false) 或滑鼠進到手牌區，則視為「放回手牌」
                if (!CombatManager.Instance.CanSelectCards || _state.MouseInsideHand)
                {
                    OnCardDraggedInsideHandCore();
                    return;
                }

                // 5. 否則，滑鼠若放開，就嘗試打出卡牌
                PlayCard();
            }
        }

        /// <summary>
        /// 輔助函式：在拖出手牌後，若滑鼠移回手牌區域且仍在按著，<br/>
        /// 則把卡片復原放回手牌 (AddCardToHand)。
        /// </summary>
        private void OnCardDraggedInsideHandCore()
        {
            if (_state.IsUsingSelectingEffectCard)
            {
                // 如果原本是需要指定單體敵人的卡，要記得還原該卡的原始索引
                
                _highlightController.DeactivateAllHighlights();
                arrowController.Deactivate();
            }
            _state.DraggedInsideHandIndex = _state.TempDraggedInHandCardIndex;
            _state.TempDraggedInHandCardIndex = -1;

            AddCardToHand(_state.HeldOutHandCard, _state.DraggedInsideHandIndex);
            _state.HeldOutHandCard = null;
        }

        #endregion

        #region 打出卡牌 —— PlayCard

        /// <summary>
        /// 在卡牌被拖出手牌並放開滑鼠按鍵後呼叫，<br/>
        /// 1. 若可用卡 (驗證資源 + 若需指定敵人則檢查目標)，則真正呼叫 Use()，觸發卡牌效果。<br/>
        /// 2. 不可用就放回手牌。<br/>
        /// 3. 同時觸發對應的事件 (CardPlayed / CardReturnedToHand)。
        /// </summary>
        private void PlayCard()
        {
            // 1. 只在滑鼠放開 (GetMouseButtonUp) 時才實際判斷「可否打出」
            if (!Input.GetMouseButtonUp(0)) return;

            // 2. 先停用 Highlight 特效
            _highlightController.DeactivateAllHighlights();
            arrowController.Deactivate();

            bool backToHand = true;

            // 3. 找到滑鼠位置下的角色 (若沒有撞到任何 CharacterBase，就回傳 null)
            var mousePos = _mouseHandler.MousePos;
            CharacterBase hitCharacter = GetHitCharacter(mousePos);

            // 4. 驗證是否可以使用該卡 (資源 + 單體敵人判斷)
            if (CardPlayValidator.CanUseCard(_state.HeldOutHandCard, hitCharacter))
            {
                // 4a. 如果通過驗證，就派送 Use()，並觸發成功事件
                var targets = new List<CharacterBase> { hitCharacter };
                _state.HeldOutHandCard.Use(targets);
                DeactivateSelectingSingleEnemyEffect();
                backToHand = false;

                CardEvents.CardPlayed(_state.HeldOutHandCard);
            }

            // 5. 若無法打出則放回手牌，並觸發放回事件
            if (backToHand)
            {
                AddCardToHand(_state.HeldOutHandCard, _state.ClosestMouseIndex);
                CardEvents.CardReturnedToHand(_state.HeldOutHandCard);
            }

            // 6. 在任何情況下，手上都不再持有該卡
            _state.HeldOutHandCard = null;
        }

        #endregion

        #region 處理單一敵人 Highlight —— HandleCardSelectingSingleEnemy

        /// <summary>
        /// 如果當前 HeldCard 需要選單一敵人，就對滑鼠下方的碰撞物嘗試判斷是否為 Enemy。<br/>
        /// 1. 若滑鼠碰到敵人且尚未啟動 Highlight，就呼叫 ActivateSelectingSingleEnemyEffect。<br/>
        /// 2. 若滑鼠離開該敵人且之前曾經啟動 Highlight，就呼叫 DeactivateSelectingSingleEnemyEffect。<br/>
        /// </summary>
        private void HandleCardSelectingSingleEnemy()
        {
            if (_state.HeldOutHandCard == null) return;
            if (!_state.HeldOutHandCard.SpecifiedEnemyTarget) return;

            // 1. Raycast 看滑鼠下方是否擊中 Enemy
            Enemy selectedEnemy = null;
            RaycastHit hit;
            Ray mainRay = _mainCam.ScreenPointToRay(_mouseHandler.MousePos);
            if (Physics.Raycast(mainRay, out hit, 1000f, targetLayer))
            {
                selectedEnemy = hit.collider.gameObject.GetComponent<Enemy>();
            }

            // 2. 若找到敵人且尚未啟動，就啟用 Highlight 效果
            if (selectedEnemy != null && !_state.EnemyIsBeingSelected)
            {
                ActivateSelectingSingleEnemyEffect(selectedEnemy);
                _state.EnemyIsBeingSelected = true;
            }
            // 3. 若找不到敵人，且曾經啟動 Highlight，就停用
            else if (selectedEnemy == null && _state.EnemyIsBeingSelected)
            {
                DeactivateSelectingSingleEnemyEffect();
                _state.EnemyIsBeingSelected = false;
            }
        }

        private void ActivateSelectingSingleEnemyEffect(Enemy selectedEnemy)
        {
            _highlightController.ActivateEnemyHighlight(selectedEnemy);
            arrowController.OnEnterEnemy();
            _state.HeldOutHandCard?.UpdateCardDisplay();
        }

        private void DeactivateSelectingSingleEnemyEffect()
        {
            _highlightController.DeactivateEnemyHighlights();
            arrowController.OnLeaveEnemy();
            _state.HeldOutHandCard?.UpdateCardDisplay();
        }

        #endregion

        #region Utility — 滑鼠距離 & 命中角色

        /// <summary>
        /// 計算目前手牌中，與滑鼠「最接近」那張卡的距離平方 (供挑出 selectedIndex 用)。<br/>
        /// </summary>
        private void GetDistanceToCurrentSelectedCard( out float sqrDistance)
        {
            var count = hand.Count;
            sqrDistance = 1000f;
            if (_state.ClosestMouseIndex >= 0 && _state.ClosestMouseIndex < count)
            {
                float t = (_state.ClosestMouseIndex + 0.5f) / count;
                Vector3 p = LayoutCurveUtility.GetCurvePoint(_a, _b, _c, t);
                sqrDistance = (p - _mouseHandler.MouseWorldPos).sqrMagnitude;
            }
        }

        /// <summary>
        /// 執行射線檢查，取得滑鼠下方碰到的 CharacterBase (若沒有碰到，回傳 null)。</summary>
        private CharacterBase GetHitCharacter(Vector2 mousePos)
        {
            RaycastHit hit;
            Ray ray = _mainCam.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out hit, 1000f, targetLayer))
            {
                return hit.collider.gameObject.GetComponent<CharacterBase>();
            }
            return null;
        }

        #endregion

        #region 卡牌新增/刪除/移動 —— Card Management

        /// <summary>
        /// 將手牌列表中 index 位置的卡牌移動到 toIndex。<br/>
        /// 如果 updateHierarchyOrder 為 true，則更新該卡的 Transform.SiblingIndex (UI 兄弟順序)。</summary>
        public void MoveCardToIndex(int currentIndex, int toIndex)
        {
            if (currentIndex == toIndex) return;
            BattleCard battleCard = hand[currentIndex];
            hand.RemoveAt(currentIndex);
            hand.Insert(toIndex, battleCard);

            if (updateHierarchyOrder)
            {
                battleCard.transform.SetSiblingIndex(toIndex);
            }
        }

        /// <summary>
        /// 將一張卡牌加入到手牌 (可指定插入索引，若 index < 0 就加到列表末尾)。<br/>
        /// 若 updateHierarchyOrder 為 true，則直接把該卡設為 HandController 的 child 並拉到指定索引。</summary>
        public void AddCardToHand(BattleCard battleCard, int index = -1)
        {
            if (index < 0)
            {
                hand.Add(battleCard);
                index = hand.Count - 1;
            }
            else
            {
                hand.Insert(index, battleCard);
            }

            if (updateHierarchyOrder)
            {
                battleCard.transform.SetParent(transform);
                battleCard.transform.SetSiblingIndex(index);
            }
        }

        /// <summary>
        /// 從手牌中移除指定索引的卡牌 (並可選擇把該物件從畫面上抽離)。<br/>
        /// 若 updateHierarchyOrder 為 true，則把該卡物件 parent 指回 HandController.parent，避免遺漏在 UI 階層中。</summary>
        public void RemoveCardFromHand(int index)
        {
            if (updateHierarchyOrder)
            {
                BattleCard battleCard = hand[index];
                battleCard.transform.SetParent(transform.parent);
                battleCard.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            }
            hand.RemoveAt(index);
        }

        /// <summary>
        /// 根據 CardData 從手牌中找到對應的 BattleCard 並扔掉 (Discard)。</summary>
        public void RemoveCardFromHand(CardData cardData)
        {
            BattleCard card = hand.Find(c => c.CardData == cardData);
            if (card != null)
                card.Discard();
        }

        #endregion
    }
}
