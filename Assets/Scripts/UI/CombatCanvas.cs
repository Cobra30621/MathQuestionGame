﻿using System;
using Feedback;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Power;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NueGames.UI
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
            bool isAllyTurn = CombatManager.CurrentCombatStateType == CombatStateType.AllyTurn;
            
            endTurnButton.interactable = isAllyTurn;
        }

        #endregion

        #region Public Methods
        public void SetPileTexts()
        {
            drawPileTextField.text = $"{CollectionManager.DrawPile.Count.ToString()}";
            discardPileTextField.text = $"{CollectionManager.DiscardPile.Count.ToString()}";
            exhaustPileTextField.text =  $"{CollectionManager.ExhaustPile.Count.ToString()}";
            manaTextTextField.text = $"{CombatManager.CurrentMana}/{CombatManager.MaxMana()}";
        }

        public void OnMathManaChange()
        {
            onMathManaChangeFeedback.Play();
        }

        public void OnManaChange(int value)
        {
           onManaChangeFeedback?.Play(); 
           manaTextTextField.text = $"{CombatManager.CurrentMana}/" +
                                    $"{CombatManager.MaxMana()}";
        }

        public override void ResetCanvas()
        {
            base.ResetCanvas();
            CombatLosePanel.SetActive(false);
        }

        public void EndTurn()
        {
            CombatManager.EndTurn();
        }
        #endregion
    }
}