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
        public void SetAllyData(AllyData allyData)
        {
            // Update UI elements with the selected AllyData
            selectedIcon.sprite = allyData.Icon;
            _allyData = allyData;
        }

        private void SelectCharacter()
        {
            Debug.Log("Selected character: " + _allyData.CharacterName);
            GameManager.Instance.StageSelectedHandler.SetAllyData(_allyData);
        }
    }
}