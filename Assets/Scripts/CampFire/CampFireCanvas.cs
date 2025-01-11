using System;
using Encounter;
using NueGames.UI;
using NueGames.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace CampFire
{
    [RequireComponent(typeof(SceneChanger))]
    public class CampFireCanvas : CanvasBase
    {
        [SerializeField] private GameObject optionPanel;

        [SerializeField] private CharacterSkillLevelUpPanel characterSkillLevelUpPanel;
        [SerializeField] private ThrowCardPanel throwCardPanel;

        
        [SerializeField] private Text healText;
        
        private SceneChanger _sceneChanger;
        private void Awake()
        {
            _sceneChanger = GetComponent<SceneChanger>();
        }
        
        public override void OpenCanvas()
        {
            base.OpenCanvas();
            optionPanel.SetActive(true);
            
            int healAmount = (int) Math.Ceiling(GameManager.allyData.MaxHealth * CampFireManager.Instance.healPercent);
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
            CampFireManager.Instance.Heal();
        }

        public void CharacterSkillLevelUp()
        {
            Debug.Log("Character Skill Level Up");
            characterSkillLevelUpPanel.Open();
        }

        public void ThrowCard()
        {
            Debug.Log("ThrowCardUI");
            throwCardPanel.Open();
        }
        
        public void OnSelectOption()
        {
            optionPanel.SetActive(false);
        }

        #endregion
    }
}