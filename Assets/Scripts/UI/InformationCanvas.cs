﻿using Economy;
using Stage;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InformationCanvas : CanvasBase
    {
        [Header("Settings")] 
        [SerializeField] private TextMeshProUGUI roomTextField;
        [SerializeField] private TextMeshProUGUI goldTextField;
        [SerializeField] private TextMeshProUGUI nameTextField;
        [SerializeField] private TextMeshProUGUI healthTextField;
        [SerializeField] private TextMeshProUGUI stoneTextField;

        public TextMeshProUGUI RoomTextField => roomTextField;
        public TextMeshProUGUI GoldTextField => goldTextField;
        public TextMeshProUGUI NameTextField => nameTextField;
        public TextMeshProUGUI HealthTextField => healthTextField;
        public TextMeshProUGUI StoneTextField => stoneTextField;
        
        
        #region Setup
        private void Start()
        {
            ResetCanvas();
        }

        private void Update()
        {
            SetGoldText(CoinManager.Instance.Money);
            SetStoneText(CoinManager.Instance.Stone);
            
            if (StageSelectedManager.Instance.GetAllyData() != null)
            {
                SetNameText(StageSelectedManager.Instance.GetAllyData().CharacterName);
            }
        }

        #endregion
        
        #region Public Methods
        public void SetRoomText(string mapName) => 
            RoomTextField.text = $"{mapName}";

        public void SetGoldText(int value)=>GoldTextField.text = $"{value}";
        
        public void SetStoneText(int value) => StoneTextField.text = $"{value}";

        public void SetNameText(string name) => NameTextField.text = $"{name}";

        public void SetHealthText(int currentHealth,int maxHealth) => HealthTextField.text = $"{currentHealth}/{maxHealth}";

        public override void ResetCanvas()
        {
            var healthData = GameManager.AllyHealthHandler.GetAllyHealthData();
            SetHealthText(healthData.CurrentHealth,
                healthData.MaxHealth);
            SetGoldText(CoinManager.Instance.Money);
            SetStoneText(CoinManager.Instance.Stone);
            
            if(StageSelectedManager.Instance.GetAllyData() == null) return;
            
            SetNameText(StageSelectedManager.Instance.GetAllyData().CharacterName);
        }
        #endregion
        
    }
}