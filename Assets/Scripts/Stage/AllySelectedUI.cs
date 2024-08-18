using System.Collections.Generic;
using NueGames.Data.Characters;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stage
{
    public class AllySelectedUI : MonoBehaviour
    {
        [Required]
        public TextMeshProUGUI characterName;
        [Required]
        public TextMeshProUGUI health;
        [Required]
        public TextMeshProUGUI description;
        [Required]
        public Transform spawnIconPos;
        [Required]
        public Image character;
        [Required]
        public GameObject iconPrefab;
        
        [Required] [SerializeField] private CanvasGroup _canvasGroup;

        private bool _haveAllySelected = false;

        public void Init(List<AllyData> allyData)
        {
            CreateSelectedIcons(allyData);
            
            ClosePanel();
        }

        public void CreateSelectedIcons(List<AllyData> allyDataList)
        {
            // Clear any existing icons
            foreach (Transform child in spawnIconPos)
            {
                Destroy(child.gameObject);
            }

            // Create icons for each AllyData in the list
            foreach (AllyData allyData in allyDataList)
            {
                CharacterSelectedIcon iconInstance =
                    Instantiate(iconPrefab, spawnIconPos).GetComponent<CharacterSelectedIcon>();
                iconInstance.SetAllyData(allyData);
            }
        }
        
        public void OnAllySelected(AllyData selectedAllyData)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
            
            // Update UI elements with the selected ally data
            characterName.text = selectedAllyData.CharacterName;
            health.text = selectedAllyData.MaxHealth.ToString();
            description.text = selectedAllyData.CharacterDescription;
            character.sprite = selectedAllyData.Sprite;

            _haveAllySelected = true;
        }

        public void ClosePanel()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;

            _haveAllySelected = false;
        }

        public bool HaveAllySelected()
        {
            return _haveAllySelected;
        }
    }
}