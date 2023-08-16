using NueGames.Managers;
using TMPro;
using UnityEngine;

namespace NueGames.UI
{
    public class InformationCanvas : CanvasBase
    {
        [Header("Settings")] 
        [SerializeField] private GameObject randomizedDeckObject;
        [SerializeField] private TextMeshProUGUI roomTextField;
        [SerializeField] private TextMeshProUGUI goldTextField;
        [SerializeField] private TextMeshProUGUI nameTextField;
        [SerializeField] private TextMeshProUGUI healthTextField;

        public GameObject RandomizedDeckObject => randomizedDeckObject;
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
        public void SetRoomText(int roomNumber,bool useStage = false, int stageNumber = -1) => 
            RoomTextField.text = useStage ? $"Room {stageNumber}/{roomNumber}" : $"Room {roomNumber}";

        public void SetGoldText(int value)=>GoldTextField.text = $"{value}";

        public void SetNameText(string name) => NameTextField.text = $"{name}";

        public void SetHealthText(int currentHealth,int maxHealth) => HealthTextField.text = $"{currentHealth}/{maxHealth}";

        public override void ResetCanvas()
        {
            // RandomizedDeckObject.SetActive(GameManager.PlayerData.IsRandomHand);
            // SetHealthText(GameManager.MainAlly.AllyCharacterData.MaxHealth,
            //     GameManager.MainAlly.AllyCharacterData.MaxHealth);
            // SetNameText(GameManager.GameplayData.DefaultName);
            // SetRoomText(GameManager.PlayerData.CurrentEncounterId+1,GameManager.GameplayData.UseStageSystem,GameManager.PlayerData.CurrentStageId+1);
            // UIManager.InformationCanvas.SetGoldText(GameManager.PlayerData.CurrentGold);
        }
        #endregion
        
    }
}