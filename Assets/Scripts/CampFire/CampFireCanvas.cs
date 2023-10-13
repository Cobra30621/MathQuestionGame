using NueGames.Encounter;
using NueGames.UI;
using NueGames.Utils;
using UnityEngine;

namespace CampFire
{
    [RequireComponent(typeof(SceneChanger))]
    public class CampFireCanvas : CanvasBase
    {
        [SerializeField] private GameObject optionPanel;

        [SerializeField] private CharacterSkillLevelUpPanel characterSkillLevelUpPanel;
        [SerializeField] private ThrowCardPanel throwCardPanel;

        
        private SceneChanger _sceneChanger;
        private void Awake()
        {
            _sceneChanger = GetComponent<SceneChanger>();
        }
        
        public override void OpenCanvas()
        {
            base.OpenCanvas();
            optionPanel.SetActive(true);
        }

        public void Leave()
        {
            
            EncounterManager.Instance.OnRoomCompleted();
            _sceneChanger.OpenMapScene();
        }
        
        


        #region Options

        public void Heal()
        {
            Debug.Log("Heal");
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