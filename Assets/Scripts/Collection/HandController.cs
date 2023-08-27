using System.Collections.Generic;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;
using UnityEngine;
using Kalkatos.DottedArrow;
using NueGames.Combat;
using CombatManager = NueGames.Combat.CombatManager;

namespace NueGames.Collection
{
    public class HandController : MonoBehaviour
    {
        [Header("Card Settings")] 
        [SerializeField] private bool cardUprightWhenSelected = true;
        [SerializeField] private bool cardTilt = true;
        
        [Header("Hand Settings")]
        [SerializeField] [Range(0, 5)] private float selectionSpacing = 1;
        [SerializeField] private Vector3 curveStart = new Vector3(2f, -0.7f, 0);
        [SerializeField] private Vector3 curveEnd = new Vector3(-2f, -0.7f, 0);
        [SerializeField] private Vector2 handOffset = new Vector2(0, -0.3f);
        [SerializeField] private Vector2 handSize = new Vector2(9, 1.7f);

        [Header("References")]
        public Transform discardTransform;
        public Transform exhaustTransform;
        public Transform drawTransform;
        public LayerMask selectableLayer;
        public LayerMask targetLayer;
        public Camera cam = null;
        public List<CardBase> hand; // Cards currently in hand

        [Header("Selecting Effect")] 
        private CharacterHighlightController characterHighlightController = new CharacterHighlightController();
        [SerializeField] private ArrowController arrowController;
        
        
        #region Cache
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        protected UIManager UIManager => UIManager.Instance;
        
        private Plane _plane; // world XY plane, used for mouse position raycasts
        private Vector3 _a, _b, _c; // Used for shaping hand into curve
       
        private int _selected = -1; // Card index that is nearest to mouse
        private int _dragged = -1; // Card index that is held by mouse (inside of hand)
        private CardBase _heldCard; // Card that is held by mouse (when outside of hand)
        private Vector3 _heldCardOffset;
        private Vector2 _heldCardTilt;
        private Vector2 _force;
        private Vector3 _mouseWorldPos;
        private Vector2 _prevMousePos;
        private Vector2 _mousePosDelta;

        private Rect _handBounds;
        private bool _mouseInsideHand;
        
        private bool updateHierarchyOrder = false;
        private bool showDebugGizmos = true;
        
        private Camera _mainCam;
        
        private int _usingSelectingEffectCardIndex = -1; 
        private bool _isUsingSelectingEffectCard;
        private bool _enemyIsBeingSelected;
        
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

            // Get distance to current selected card (for comparing against other cards later, to find closest)
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

                // Set to inactive material if not enough mana required to use card
                card.SetInactiveMaterialState(!PlayCardJudgment.EnoughResourceToUseCard(card));

                var noCardHeld = _heldCard == null; // Whether a card is "held" (outside of hand)
                var onSelectedCard = noCardHeld && _selected == i;
                var onDraggedCard = noCardHeld && _dragged == i;

                // Get Position along Curve (for card positioning)
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

                /* Card Tilt is disabled when in hand as they can clip through eachother :(
                if (cardTilt && onSelectedCard && mouseButton) {
                    cardForward -= new Vector3(heldCardOffset.x, heldCardOffset.y, 0);
                }*/

                // Sorting Order // 當滑鼠移動到卡片時顯示
                if (mouseHoveringOnSelected || onDraggedCard)
                {
                    // When selected bring card to front
                    if (cardUprightWhenSelected) cardUp = Vector3.up;
                    
                    cardTransform.localScale = 1.2f * Vector3.one;
                    cardPos.z = transform.position.z - 0.2f;
                }
                else
                {
                    cardTransform.localScale = Vector3.one;
                    cardPos.z = transform.position.z + t * 0.5f;
                }

                // Rotation
                cardTransform.rotation = Quaternion.RotateTowards(cardTransform.rotation,
                    Quaternion.LookRotation(cardForward, cardUp), 80f * Time.deltaTime);

                // Handle Start Dragging
                if (mouseHoveringOnSelected)
                {
                    var mouseButtonDown = Input.GetMouseButtonDown(0);
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
            if (_heldCard != null)
            {
                var cardTransform = _heldCard.transform;
                var cardUp = Vector3.up;
                var cardPos = _mouseWorldPos + _heldCardOffset;
                var cardForward = Vector3.forward;
                if (cardTilt && mouseButton) cardForward -= new Vector3(_heldCardTilt.x, _heldCardTilt.y, 0);
                
                // Bring card to front
                cardPos.z = transform.position.z - 0.2f;
                
                
                if (!_isUsingSelectingEffectCard)
                {
                    // Handle Position & Rotation
                    cardTransform.rotation = Quaternion.RotateTowards(cardTransform.rotation,
                        Quaternion.LookRotation(cardForward, cardUp), 80f * Time.deltaTime);
                    cardTransform.position = cardPos;
                }

                ActionTargetType actionTargetType = _heldCard.CardData.ActionTargetType;
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
                AddCardToHand(_heldCard, _selected);
                _dragged = _selected;
                _selected = -1;
            }
            _heldCard = null;
        }

        
        private void PlayCard(Vector2 mousePos)
        {
            // Use Card
            var mouseButtonUp = Input.GetMouseButtonUp(0);
            if (!mouseButtonUp) return;
            
            characterHighlightController.DeactivateAllHighlights();
            bool backToHand = true;

            if (PlayCardJudgment.CanUseCard(_heldCard))
            {
                CharacterBase hitCharacter = GetHitCharacter(mousePos);
                
                if (EnablePlayCard(_heldCard.CardData.ActionTargetType, hitCharacter))
                {
                    backToHand = false;
                    //  Arrow Effect for the card's ActionTargetType is single enemy
                    if (_heldCard.ActionTargetIsSingleEnemy())
                    {
                        RemoveCardFromHand(_usingSelectingEffectCardIndex);
                    }

                    var targetList = GetTargetList(_heldCard.CardData.ActionTargetType, hitCharacter);
                    
                    _heldCard.Use(targetList);
                    DeactivateSelectingSingleEnemyEffect();
                }
            }
            
            arrowController.Deactivate();

            // Cannot use card / Not enough mana! / Not the card use arrow effect Return card to hand!
            if (backToHand && (!_heldCard.ActionTargetIsSingleEnemy())) 
                AddCardToHand(_heldCard, _selected);

            _heldCard = null;
        }


        /// <summary>
        /// 是否能夠使用卡片
        /// </summary>
        /// <param name="cardActionTarget"></param>
        /// <param name="hitCharacter"></param>
        /// <param name="usableWithoutTarget"></param>
        /// <returns></returns>
        private bool EnablePlayCard(ActionTargetType cardActionTarget, CharacterBase hitCharacter)
        {
            // 只有目標是敵人時，才一定需要碰到目標
            if (cardActionTarget == ActionTargetType.Enemy)
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
        /// 取得目標對象的清單
        /// </summary>
        /// <param name="cardActionTarget"></param>
        /// <param name="hitTarget"></param>
        /// <returns></returns>
        private List<CharacterBase> GetTargetList(ActionTargetType cardActionTarget, CharacterBase hitTarget)
        {
            List<CharacterBase> targetList = new List<CharacterBase>();
            switch (cardActionTarget)
            {
                case ActionTargetType.Ally:
                    targetList.Add(CombatManager.MainAlly);
                    break;
                case ActionTargetType.Enemy:
                    if (hitTarget.IsCharacterType(CharacterType.Enemy))
                    {
                        targetList.Add(hitTarget);
                    }
                    break;
                case ActionTargetType.AllEnemies:
                    targetList.AddRange(CombatManager.Enemies);
                    break;
                case ActionTargetType.RandomEnemy:
                    throw new System.NotImplementedException();
                    break;

            }

            return targetList;
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

            if (_heldCard == null && mouseButton && _dragged != -1 && _selected != -1 && _dragged != _selected)
            {
                // Move dragged card
                MoveCardToIndex(_dragged, _selected);
                _dragged = _selected;
            }
        }

        private void OnCardDragedOutsideHand()
        {
            // Card is outside of the hand, so is considered "held" ready to be used
            _heldCard = hand[_dragged];
            _isUsingSelectingEffectCard = _heldCard.ActionTargetIsSingleEnemy();
            if (_isUsingSelectingEffectCard)
            {
                _usingSelectingEffectCardIndex = _dragged;
                arrowController.SetupAndActivate(_heldCard.transform);
            }
            else
            {
                // Remove from hand, so that cards in hand fill the hole that the card left
                RemoveCardFromHand(_dragged);
            }
            
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
            
            // Check card Action Target Is Single Enemy
            if (_heldCard != null)
            {
                if (_heldCard.ActionTargetIsSingleEnemy())
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
            CombatManager.SetSelectedEnemy(selectedEnemy);
            _heldCard?.UpdateCardText();
        }
        
        private void DeactivateSelectingSingleEnemyEffect()
        {
            characterHighlightController.DeactivateEnemyHighlights();
            arrowController.OnLeaveEnemy();
            CombatManager.SetSelectedEnemy(null);
            _heldCard?.UpdateCardText();
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
        /// Moves the card in hand from the currentIndex to the toIndex. If you want to move a card that isn't in hand, use AddCardToHand
        /// </summary>
        public void MoveCardToIndex(int currentIndex, int toIndex)
        {
            if (currentIndex == toIndex) return; // Same index, do nothing
            CardBase card = hand[currentIndex];
            hand.RemoveAt(currentIndex);
            hand.Insert(toIndex, card);

            if (updateHierarchyOrder)
            {
                card.transform.SetSiblingIndex(toIndex);
            }
        }

        /// <summary>
        /// Adds a card to the hand. Optional param to insert it at a given index.
        /// </summary>
        public void AddCardToHand(CardBase card, int index = -1)
        {
            if (index < 0)
            {
                // Add to end
                hand.Add(card);
                index = hand.Count - 1;
            }
            else
            {
                // Insert at index
                hand.Insert(index, card);
            }

            if (updateHierarchyOrder)
            {
                card.transform.SetParent(transform);
                card.transform.SetSiblingIndex(index);
            }
        }

        /// <summary>
        /// Remove the card at the specified index from the hand.
        /// </summary>
        public void RemoveCardFromHand(int index)
        {
            if (updateHierarchyOrder)
            {
                CardBase card = hand[index];
                card.transform.SetParent(transform.parent);
                card.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
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
