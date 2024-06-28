using Money;
using NueGames.Managers;
using TMPro;
using UnityEngine;

namespace NueGames.UI
{
    public class InformationCanvas : CanvasBase
    {
        [Header("Settings")] 
        [SerializeField] private TextMeshProUGUI roomTextField;
        [SerializeField] private TextMeshProUGUI goldTextField;
        [SerializeField] private TextMeshProUGUI nameTextField;
        [SerializeField] private TextMeshProUGUI healthTextField;

        public TextMeshProUGUI RoomTextField => roomTextField;
        public TextMeshProUGUI GoldTextField => goldTextField;
        public TextMeshProUGUI NameTextField => nameTextField;
        public TextMeshProUGUI HealthTextField => healthTextField;
        
        
        #region Setup
        private void Start()
        {
            ResetCanvas();
        } 
        #endregion
        
        #region Public Methods
        public void SetRoomText(string mapName) => 
            RoomTextField.text = $"{mapName}";

        public void SetGoldText(int value)=>GoldTextField.text = $"{value}";

        public void SetNameText(string name) => NameTextField.text = $"{name}";

        public void SetHealthText(int currentHealth,int maxHealth) => HealthTextField.text = $"{currentHealth}/{maxHealth}";

        public override void ResetCanvas()
        {
            if (GameManager.PlayerData == null)
            {
                return;
            }
            
            SetHealthText(GameManager.PlayerData.AllyHealthData.CurrentHealth,
                GameManager.PlayerData.AllyHealthData.MaxHealth);
            SetGoldText(MoneyManager.Instance.Money);
            SetNameText(GameManager.MainAllyData.CharacterName);
        }
        #endregion
        
    }
}