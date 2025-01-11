using Combat.Card;
using Feedback;
using NueGames.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class CombatCanvas : CanvasBase
    {
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI drawPileTextField;
        [SerializeField] private TextMeshProUGUI discardPileTextField;
        [SerializeField] private TextMeshProUGUI exhaustPileTextField;
        [SerializeField] private TextMeshProUGUI manaTextTextField;
        
        [Header("Panels")]
        [SerializeField] private GameObject combatLosePanel;

        [Header("FeedBack")] 
        [SerializeField] private IFeedback onMathManaChangeFeedback;

        [SerializeField] private IFeedback onManaChangeFeedback;
        public GameObject CombatLosePanel => combatLosePanel;

        [SerializeField] private Button endTurnButton; 
        

        #region Setup
        private void Awake()
        {
            CombatLosePanel.SetActive(false);
            
        }

        private void OnEnable()
        {
            ManaManager.OnGainMana += OnManaChange;
        }
        
        private void OnDisable()
        {
            ManaManager.OnGainMana -= OnManaChange;
        }

        private void Update()
        {
            if(!CombatManager.HasInstance()) return;
            
            bool isAllyTurn = CombatManager.Instance.CurrentCombatStateType == CombatStateType.AllyTurn;
            
            endTurnButton.interactable = isAllyTurn;
        }

        #endregion

        #region Public Methods
        public void SetPileTexts()
        {
            drawPileTextField.text = $"{CollectionManager.Instance.DrawPile.Count.ToString()}";
            discardPileTextField.text = $"{CollectionManager.Instance.DiscardPile.Count.ToString()}";
            exhaustPileTextField.text =  $"{CollectionManager.Instance.ExhaustPile.Count.ToString()}";
            manaTextTextField.text = $"{CombatManager.Instance.CurrentMana}/{CombatManager.Instance.MaxMana()}";
        }

        public void OnMathManaChange()
        {
            onMathManaChangeFeedback.Play();
        }

        public void OnManaChange(int value)
        {
           onManaChangeFeedback?.Play(); 
           manaTextTextField.text = $"{CombatManager.Instance.CurrentMana}/" +
                                    $"{CombatManager.Instance.MaxMana()}";
        }

        public override void ResetCanvas()
        {
            base.ResetCanvas();
            CombatLosePanel.SetActive(false);
        }

        public void EndTurn()
        {
            CombatManager.Instance.EndTurn();
        }
        #endregion
    }
}