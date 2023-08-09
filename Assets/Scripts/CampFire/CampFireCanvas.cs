using NueGames.UI;
using UnityEngine;

namespace CampFire
{
    public class CampFireCanvas : CanvasBase
    {
        [SerializeField] private GameObject optionPanel;

        [SerializeField] private CharacterSkillLevelUpPanel characterSkillLevelUpPanel;
        [SerializeField] private ThrowCardSelectPanel throwCardSelectPanel;


        public override void OpenCanvas()
        {
            base.OpenCanvas();
            optionPanel.SetActive(true);
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
            Debug.Log("ThrowCard");
            throwCardSelectPanel.Open();
        }
        
        public void OnSelectOption()
        {
            optionPanel.SetActive(false);
        }

        #endregion
    }
}