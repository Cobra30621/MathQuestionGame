using System;
using Encounter;
using Stage;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace CampFire
{
    [RequireComponent(typeof(SceneChanger))]
    public class CampFireCanvas : CanvasBase
    {
        [SerializeField] private GameObject optionPanel;

        [SerializeField] private ThrowCardPanel throwCardPanel;

        
        [SerializeField] private Text healText;
        
        private SceneChanger _sceneChanger;
        
        public float healPercent = 0.3f;
        
        
        private void Awake()
        {
            _sceneChanger = GetComponent<SceneChanger>();
        }
        
        public override void OpenCanvas()
        {
            base.OpenCanvas();
            optionPanel.SetActive(true);

            var maxHealth = StageSelectedManager.Instance.GetAllyData().MaxHealth;
            int healAmount = (int) Math.Ceiling(maxHealth * healPercent);
            healText.text = $"回血 ({healAmount}";
        }

        public void Leave()
        {
            
            EncounterManager.Instance.OnRoomCompleted();
            StartCoroutine(_sceneChanger.OpenMapScene());
        }
        
        


        #region Options

        public void Heal()
        {
            GameManager.HealAlly(healPercent);
            
            Leave();
        }
        

        public void ThrowCard()
        {
            Debug.Log("ThrowCardUI");
            throwCardPanel.Open();
        }
        
        public void OnSelectOption()
        {
            optionPanel.SetActive(false);
            
            Leave();
        }

        #endregion
    }
}