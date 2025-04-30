using Characters.Ally;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Stage
{
    public class CharacterSelectedIcon : MonoBehaviour
    {
        [Required]
        public Image selectedIcon;
        private AllyData _allyData;
        private AllyName _allyName;
        [Required]
        public Button SelectedButton;

        private void Awake()
        {
            SelectedButton.onClick.AddListener(SelectCharacter);
        }

        /// <summary>
        /// Sets the data of the selected Ally character and updates the UI elements accordingly.
        /// </summary>
        /// <param name="allyData">The AllyData object containing the character's data.</param>
        public void SetAllyData(AllyName allyName, AllyData allyData)
        {
            // Update UI elements with the selected AllyData
            selectedIcon.sprite = allyData.Icon;
            _allyName = allyName;
            _allyData = allyData;
        }

        private void SelectCharacter()
        {
            StageSelectedManager.Instance.SetAllyData(_allyName);
        }
    }
}