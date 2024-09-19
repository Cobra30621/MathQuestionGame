using System.Collections.Generic;
using Enemy;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;
using UnityEngine;
using Kalkatos.DottedArrow;
using Managers;
using NueGames.Combat;
using CombatManager = Combat.CombatManager;

namespace NueGames.Collection
{
    public class HandController : MonoBehaviour
    {
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
        [SerializeField] private float selectedCardOffsetY, cardOffsetY;
        
        [Header("References")]
        public Transform discardTransform;
        public Transform exhaustTransform;
        public Transform drawTransform;
        public LayerMask selectableLayer;
        public LayerMask targetLayer;
        public Camera cam = null;
        public List<BattleCard> hand; // Cards currently in hand

        [Header("Selecting Effect")] 
        private CharacterHighlightController characterHighlightController = new CharacterHighlightController();
        [SerializeField] private ArrowController arrowController;
        
        
        #region Cache
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        protected UIManager UIManager => UIManager.Instance;
        
        private Plane _plane; // world XY plane, used for mouse position raycasts
        private Vector3 _a, _b, _c; // Used for shaping hand into curve
       
        [SerializeField] private int _selected = -1; // Card index that is nearest to mouse
        [SerializeField] private int _dragged = -1; // Card index that is held by mouse (inside of hand)
        private BattleCard _heldBattleCard; // Card that is held by mouse (when outside of hand)
        [SerializeField] private Vector3 _heldCardOffset;
        [SerializeField] private Vector2 _heldCardTilt;
        [SerializeField] private Vector2 _force;
        [SerializeField] private Vector3 _mouseWorldPos;
        [SerializeField] private Vector2 _prevMousePos;
        [SerializeField]  private Vector2 _mousePosDelta;

        private Rect _handBounds;
        [SerializeField] private bool _mouseInsideHand;
        
        private bool updateHierarchyOrder = false;
        private bool showDebugGizmos = true;
        
        private Camera _mainCam;
        
        [SerializeField] private int _usingSelectingEffectCardIndex = -1; 
        [SerializeField] private bool _isUsingSelectingEffectCard;
        [SerializeField] private bool _enemyIsBeingSelected;
        [SerializeField] private Vector3 selectingEffectCardPos;
        
        public bool IsDraggingActive { get; private set; } = true;

        #endregion

        #region Setup
        private void Awake()
        {
            _mainCam = Camera.main;
        }

        private void Start()
        {
            InitHand();
            arrowController.SetCamera(cam);
        }

        private void InitHand()
        {
            _a = transform.TransformPoint(curveStart);
            _b = transform.position;
            _c = transform.TransformPoint(curveEnd);
            _handBounds = new Rect((handOffset - handSize / 2), handSize);
            _plane = new Plane(-Vector3.forward, transform.position);
            _prevMousePos = Input.mousePosition;
        }
        

        #endregion

        #region Process
        private void Update()
        {
            // --------------------------------------------------------
            // HANDLE MOUSE & RAYCAST POSITION
            // --------------------------------------------------------

            if (!IsDraggingActive) return;
           
            var mousePos = HandleMouseInput(out var count, out var sqrDistance, out var mouseButton);
            // --------------------------------------------------------
            // HANDLE CARDS IN HAND
            // --------------------------------------------------------
            HandleCardsInHand(count, mouseButton, sqrDistance);

            // --------------------------------------------------------
            // HANDLE DRAGGED CARD
            // (Card held by mouse, inside hand)
            // --------------------------------------------------------
            HandleDraggedCardInsideHand(mouseButton, count);

            // --------------------------------------------------------
            // HANDLE HELD CARD
            // (Card held by mouse, outside of the hand)
            // --------------------------------------------------------
            HandleDraggedCardOutsideHand(mouseButton, mousePos);
            
            // --------------------------------------------------------
            // HANDLE Enemy Bounds
            // (If CardActionTarget is single enemy, show Selected Effect)
            // --------------------------------------------------------
            HandleCardSelectingSingleEnemy(mousePos);
        }
        #endregion
        
        #region Methods
        public void EnableDragging() => IsDraggingActive = true;
        public void DisableDragging() => IsDraggingActive = false;

        private Vector2 HandleMouseInput(out int count, out float sqrDistance, out bool mouseButton)
        {
            Vector2 mousePos = Input.mousePosition;

            // Allows mouse to go outside game window but keeps cards within window
            // If mouse doesn't need to go outside, could use "Cursor.lockState = CursorLockMode.Confined;" instead
            mousePos.x = Mathf.Clamp(mousePos.x, 0, Screen.width);
            mousePos.y = Mathf.Clamp(mousePos.y, 0, Screen.height);

            // Mouse movement velocity
            if (cardTilt) TiltCard(mousePos);

            // Get world position from mouse
            GetMouseWorldPosition(mousePos);

            // Get distance to current selected uiCard (for comparing against other cards later, to find closest)
            GetDistanceToCurrentSelectedCard(out count, out sqrDistance);

            // Check if mouse is inside hand bounds
            CheckMouseInsideHandBounds(out mouseButton);
            return mousePos;
        }

        private void HandleCardsInHand(int count, bool mouseButton, float sqrDistance)
        {
            for (var i = 0; i < count; i++)
            {
                var card = hand[i];
                var cardTransform = card.transform;

                // Set to inactive material if not enough mana required to use uiCard
                card.SetInactiveMaterialState(!PlayCardJudgment.EnoughResourceToUseCard(card));

                var noCardHeld = _heldBattleCard == null; // Whether a uiCard is "held" (outside of hand)
                var onSelectedCard = noCardHeld && _selected == i;
                var onDraggedCard = noCardHeld && _dragged == i;

                // Get Position along Curve (for uiCard positioning)
                float selectOffset = 0;
                if (noCardHeld)
                    selectOffset = 0.02f *
                                   Mathf.Clamp01(1 - Mathf.Abs(Mathf.Abs(i - _selected) - 1) / (float) count * 3) *
                                   Mathf.Sign(i - _selected);

                var t = (i + 0.5f) / count + selectOffset * selectionSpacing;
                var p = GetCurvePoint(_a, _b, _c, t);

                var d = (p - _mouseWorldPos).sqrMagnitude;
                var mouseCloseToCard = d < 0.5f;
                var mouseHoveringOnSelected =
                    onSelectedCard && mouseCloseToCard && _mouseInsideHand; //  && mouseInsideHand

                // Handle Card Position & Rotation
                //Vector3 cardUp = p - (transform.position + Vector3.down * 7);
                var cardUp = GetCurveNormal(_a, _b, _c, t);
                var cardPos = p + (mouseHoveringOnSelected ? cardTransform.up * 0.3f : Vector3.zero);
                var cardForward = Vector3.forward;

          

                // Show Selected Card
                if (mouseHoveringOnSelected || onDraggedCard)
                {
                    // When selected bring uiCard to front
                    if (cardUprightWhenSelected) cardUp = Vector3.up;
                    
                    cardTransform.localScale = selectedCardSize * Vector3.one;
                    cardPos.z = transform.position.z;
                    cardPos.y += selectedCardOffsetY;
                }
                else
                {
                    cardTransform.localScale = Vector3.one;
                    cardPos.z = transform.position.z + t * 0.5f ;
                }
                // Rotation
                cardTransform.rotation = Quaternion.RotateTowards(cardTransform.rotation,
                    Quaternion.LookRotation(cardForward, cardUp), 80f * Time.deltaTime);

                // Handle Start Dragging
                if (mouseHoveringOnSelected)
                {
                    var mouseButtonDown = Input.GetMouseButton(0);
                    if (mouseButtonDown)
                    {
                        _dragged = i;
                        _heldCardOffset = cardTransform.position - _mouseWorldPos;
                        _heldCardOffset.z = -0.1f;
                    }
                }

                // Handle Card Position
                if (onDraggedCard && mouseButton)
                {
                    // Held by mouse / dragging
                    cardPos = _mouseWorldPos + _heldCardOffset;
                    cardTransform.position = cardPos;
                }
                else
                {
                    cardPos = Vector3.MoveTowards(cardTransform.position, cardPos, 16f * Time.deltaTime);
                    cardTransform.position = cardPos;
                }

                // Get Selected Card
                if (GameManager.CanSelectCards)
                {
                    //float d = (p - mouseWorldPos).sqrMagnitude;
                    if (d < sqrDistance)
                    {
                        sqrDistance = d;
                        _selected = i;
                    }
                }
                else
                {
                    _selected = -1;
                    _dragged = -1;
                }

                // Debug Gizmos
                if (showDebugGizmos)
                {
                    var c = new Color(0, 0, 0, 0.2f);
                    if (i == _selected)
                    {
                        c = Color.red;
                        if (sqrDistance > 2) c = Color.blue;
                    }

                    Debug.DrawLine(p, _mouseWorldPos, c);
                }
            }
        }

        private void HandleDraggedCardOutsideHand(bool mouseButton, Vector2 mousePos)
        {
            if (_heldBattleCard != null)
            {
                var cardTransform = _heldBattleCard.transform;
                // Debug.Log($"cardTransform.position { cardTransform.position}");
                var cardUp = Vector3.up;
                var cardPos = _mouseWorldPos + _heldCardOffset;
                var cardForward = Vector3.forward;
                if (cardTilt && mouseButton) cardForward -= new Vector3(_heldCardTilt.x, _heldCardTilt.y, 0);
                
                // Handle Position & Rotation
                // cardTransform.rotation = Quaternion.RotateTowards(cardTransform.rotation,
                //     Quaternion.LookRotation(cardForward, cardUp), 80f * Time.deltaTime);
                if (_isUsingSelectingEffectCard)
                {
                    cardTransform.localScale = selectedCardSize * Vector3.one;
                    
                    cardPos.x = selectingEffectCardPos.x;
                    cardPos.y = selectingEffectCardPos.y;
                }
                
                // Debug.Log($"CardPos {cardPos}");
                cardTransform.position = cardPos;
                

                ActionTargetType actionTargetType = _heldBattleCard.TargetChoose;
                characterHighlightController.OnDraggedCardOutsideHand(actionTargetType);

                if (!GameManager.CanSelectCards || _mouseInsideHand)
                {
                    OnCardDragedInsideHand();
                    return;
                }

                PlayCard(mousePos);
            }
        }

        private void OnCardDragedInsideHand()
        {
            if (_isUsingSelectingEffectCard)
            {
                _dragged = _usingSelectingEffectCardIndex;
                _usingSelectingEffectCardIndex = -1;
                characterHighlightController.DeactivateAllHighlights();
                arrowController.Deactivate();
            }
            else
            {
                _dragged = _selected;
                _selected = -1;
            }
            AddCardToHand(_heldBattleCard, _dragged);
            
            
            _heldBattleCard = null;
        }

        
        private void PlayCard(Vector2 mousePos)
        {
            // Use Card
            var mouseButtonUp = Input.GetMouseButtonUp(0);
            if (!mouseButtonUp) return;
            
            characterHighlightController.DeactivateAllHighlights();
            bool backToHand = true;

            if (PlayCardJudgment.CanUseCard(_heldBattleCard))
            {
                CharacterBase hitCharacter = GetHitCharacter(mousePos);
                
                if (EnablePlayCard(_heldBattleCard.SpecifiedEnemyTarget, hitCharacter))
                {
                    backToHand = false;
                    
                    var specified = new List<CharacterBase>(){hitCharacter};
                    
                    _heldBattleCard.Use(specified);
                    DeactivateSelectingSingleEnemyEffect();
                }
            }
            
            arrowController.Deactivate();

            // Cannot use uiCard / Not enough mana! / Not the uiCard use arrow effect Return uiCard to hand!
            if (backToHand ) 
                AddCardToHand(_heldBattleCard, _selected);

            _heldBattleCard = null;
        }


        /// <summary>
        /// 是否能夠使用卡片
        /// </summary>
        /// <param name="cardActionTarget"></param>
        /// <param name="hitCharacter"></param>
        /// <param name="usableWithoutTarget"></param>
        /// <returns></returns>
        private bool EnablePlayCard(bool specifiedEnemyTarget, CharacterBase hitCharacter)
        {
            // 只有目標是單一敵人時，才一定需要碰到目標
            if (specifiedEnemyTarget)
            {
                // 如果 hitCharacter 沒有碰到任何目標，回傳 false
                if (hitCharacter == null)
                {
                    return false;
                }
                
                if (!hitCharacter.IsCharacterType(CharacterType.Enemy))
                {
                    return false;
                }
            }
            
            // 其他狀況，都可以使用卡片
            return true;

        }

       
        
        /// <summary>
        /// 取得畫面碰到的角色
        /// </summary>
        /// <param name="mousePos"></param>
        /// <returns></returns>
        private CharacterBase GetHitCharacter(Vector2 mousePos)
        {
            RaycastHit hit;
            var mainRay = _mainCam.ScreenPointToRay(mousePos);
            CharacterBase character;
            if (Physics.Raycast(mainRay, out hit, 1000, targetLayer))
            {
                character = hit.collider.gameObject.GetComponent<CharacterBase>();
            }
            else
            {
                character = null;
            }

            return character;
        }
        
        


        private void HandleDraggedCardInsideHand(bool mouseButton, int count)
        {
            if (!mouseButton)
            {
                // Stop dragging
                _heldCardOffset = Vector3.zero;
                _dragged = -1;
            }

            if (_dragged != -1)
            {
                if (mouseButton && !_mouseInsideHand)
                {
                    OnCardDragedOutsideHand();
                }
            }

            if (_heldBattleCard == null && mouseButton && _dragged != -1 && _selected != -1 && _dragged != _selected)
            {
                // Move dragged uiCard
                MoveCardToIndex(_dragged, _selected);
                _dragged = _selected;
            }
        }

        private void OnCardDragedOutsideHand()
        {
            // Card is outside of the hand, so is considered "held" ready to be used
            _heldBattleCard = hand[_dragged];
            _isUsingSelectingEffectCard = _heldBattleCard.SpecifiedEnemyTarget;
            if (_isUsingSelectingEffectCard)
            {
                _usingSelectingEffectCardIndex = _dragged;
                arrowController.SetupAndActivate(_heldBattleCard.transform);
            }
            
            // Remove from hand, so that cards in hand fill the hole that the uiCard left
            RemoveCardFromHand(_dragged);
            
            _dragged = -1;
        }

        private void CheckMouseInsideHandBounds(out bool mouseButton)
        {
            var point = transform.InverseTransformPoint(_mouseWorldPos);
            _mouseInsideHand = _handBounds.Contains(point);

            mouseButton = Input.GetMouseButton(0);
        }

        
        private void HandleCardSelectingSingleEnemy(Vector2 mousePos)
        {
            // Check Mouse Inside Enemy Bounds
            EnemyBase selectedEnemy= null;
            RaycastHit hit;
            var mainRay = _mainCam.ScreenPointToRay(mousePos);
            
            if (Physics.Raycast(mainRay, out hit, 1000, targetLayer))
            {
                selectedEnemy = hit.collider.gameObject.GetComponent<EnemyBase>();
            }
            
            // Check uiCard Action Target Is Single Enemy
            if (_heldBattleCard != null)
            {
                if (_heldBattleCard.SpecifiedEnemyTarget)
                {
                    HandleSelectingSingleEnemyEffect(selectedEnemy);
                }
            }
        }

        private void HandleSelectingSingleEnemyEffect(EnemyBase selectedEnemy)
        {
            if (selectedEnemy != null)
            {
                if (!_enemyIsBeingSelected)
                {
                    ActivateSelectingSingleEnemyEffect(selectedEnemy);
                    _enemyIsBeingSelected = true;
                }
            }
            else
            {
                if (_enemyIsBeingSelected)
                {
                    DeactivateSelectingSingleEnemyEffect();
                    _enemyIsBeingSelected = false;
                }
            }
        }

        private void ActivateSelectingSingleEnemyEffect(EnemyBase selectedEnemy)
        {
            characterHighlightController.ActivateEnemyHighlight(selectedEnemy);
            arrowController.OnEnterEnemy();
            _heldBattleCard?.UpdateCardDisplay();
        }
        
        private void DeactivateSelectingSingleEnemyEffect()
        {
            characterHighlightController.DeactivateEnemyHighlights();
            arrowController.OnLeaveEnemy();
            _heldBattleCard?.UpdateCardDisplay();
        }

        private void GetDistanceToCurrentSelectedCard(out int count, out float sqrDistance)
        {
            count = hand.Count;
            sqrDistance = 1000;
            if (_selected >= 0 && _selected < count)
            {
                var t = (_selected + 0.5f) / count;
                var p = GetCurvePoint(_a, _b, _c, t);
                sqrDistance = (p - _mouseWorldPos).sqrMagnitude;
            }
        }

        private void GetMouseWorldPosition(Vector2 mousePos)
        {
            var ray = cam.ScreenPointToRay(mousePos);
            if (_plane.Raycast(ray, out var enter)) _mouseWorldPos = ray.GetPoint(enter);
        }

        private void TiltCard(Vector2 mousePos)
        {
            _mousePosDelta = (mousePos - _prevMousePos) * new Vector2(1600f / Screen.width, 900f / Screen.height) *
                            Time.deltaTime;
            _prevMousePos = mousePos;

            var tiltStrength = 3f;
            var tiltDrag = 3f;
            var tiltSpeed = 50f;

            _force += (_mousePosDelta * tiltStrength - _heldCardTilt) * Time.deltaTime;
            _force *= 1 - tiltDrag * Time.deltaTime;
            _heldCardTilt += _force * (Time.deltaTime * tiltSpeed);
            // these calculations probably aren't correct, but hey, they work...? :P

            if (showDebugGizmos)
            {
                Debug.DrawRay(_mouseWorldPos, _mousePosDelta, Color.red);
                Debug.DrawRay(_mouseWorldPos, _heldCardTilt, Color.cyan);
            }
        }

        #endregion

        #region Cyan Methods

        /// <summary>
        /// Obtains a point along a curve based on 3 points. Equal to Lerp(Lerp(a, b, t), Lerp(b, c, t), t).
        /// </summary>
        public static Vector3 GetCurvePoint(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return (oneMinusT * oneMinusT * a) + (2f * oneMinusT * t * b) + (t * t * c);
        }

        /// <summary>
        /// Obtains the derivative of the curve (tangent)
        /// </summary>
        public static Vector3 GetCurveTangent(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            return 2f * (1f - t) * (b - a) + 2f * t * (c - b);
        }

        /// <summary>
        /// Obtains a direction perpendicular to the tangent of the curve
        /// </summary>
        public static Vector3 GetCurveNormal(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            Vector3 tangent = GetCurveTangent(a, b, c, t);
            return Vector3.Cross(tangent, Vector3.forward);
        }

        /// <summary>
        /// Moves the uiCard in hand from the currentIndex to the toIndex. If you want to move a uiCard that isn't in hand, use AddCardToHand
        /// </summary>
        public void MoveCardToIndex(int currentIndex, int toIndex)
        {
            if (currentIndex == toIndex) return; // Same index, do nothing
            BattleCard battleCard = hand[currentIndex];
            hand.RemoveAt(currentIndex);
            hand.Insert(toIndex, battleCard);

            if (updateHierarchyOrder)
            {
                battleCard.transform.SetSiblingIndex(toIndex);
            }
        }

        /// <summary>
        /// Adds a uiCard to the hand. Optional param to insert it at a given index.
        /// </summary>
        public void AddCardToHand(BattleCard battleCard, int index = -1)
        {
            if (index < 0)
            {
                // Add to end
                hand.Add(battleCard);
                index = hand.Count - 1;
            }
            else
            {
                // Insert at index
                hand.Insert(index, battleCard);
            }

            if (updateHierarchyOrder)
            {
                battleCard.transform.SetParent(transform);
                battleCard.transform.SetSiblingIndex(index);
            }
        }

        /// <summary>
        /// Remove the uiCard at the specified index from the hand.
        /// </summary>
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

        #endregion

        #region Editor
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.blue;

            Gizmos.DrawSphere(curveStart, 0.03f);
            //Gizmos.DrawSphere(Vector3.zero, 0.03f);
            Gizmos.DrawSphere(curveEnd, 0.03f);

            Vector3 p1 = curveStart;
            for (int i = 0; i < 20; i++)
            {
                float t = (i + 1) / 20f;
                Vector3 p2 = GetCurvePoint(curveStart, Vector3.zero, curveEnd, t);
                Gizmos.DrawLine(p1, p2);
                p1 = p2;
            }

            if (_mouseInsideHand)
            {
                Gizmos.color = Color.red;
            }

            Gizmos.DrawWireCube(handOffset, handSize);
        }
#endif

        #endregion

    }
}
