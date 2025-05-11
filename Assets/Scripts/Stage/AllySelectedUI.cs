using System.Collections.Generic;
using Characters.Ally;
using Managers;
using Relic;
using Relic.Data;
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

        [Required] public GameObject relicPanel;
        [Required] 
        public Image relicImage;
        [Required]
        public TextMeshProUGUI relicName;
        [Required]
        public TextMeshProUGUI relicDescription;
        [Required]
        public Transform spawnIconPos;
        [Required]
        public Image character;
        [Required]
        public GameObject iconPrefab;
        
        [Required] [SerializeField] private CanvasGroup _canvasGroup;

        
        private bool _haveAllySelected = false;
        
        [Required]
        public AllyDataOverview allyDataOverview;

        public void Init(List<AllyName> allyNames)
        {
            CreateSelectedIcons(allyNames);
            
            ClosePanel();
        }

        public void CreateSelectedIcons(List<AllyName> allyDataList)
        {
            // Clear any existing icons
            foreach (Transform child in spawnIconPos)
            {
                Destroy(child.gameObject);
            }

            // Create icons for each AllyData in the list
            foreach (var allyName in allyDataList)
            {
                CharacterSelectedIcon iconInstance =
                    Instantiate(iconPrefab, spawnIconPos).GetComponent<CharacterSelectedIcon>();
                var allyData = allyDataOverview.FindUniqueId(allyName.Id);
                iconInstance.SetAllyData(allyName, allyData);
            }
        }
        
        public void OnAllySelected(AllyData selectedAllyData)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
            
            // Update UI elements with the selected ally data
            characterName.text = selectedAllyData.CharacterName;
            health.text =  $"HP: " + selectedAllyData.MaxHealth.ToString();
            description.text = selectedAllyData.CharacterDescription;
            character.sprite = selectedAllyData.Sprite;

            UpdateRelicUI(selectedAllyData.initialRelic);
            
            _haveAllySelected = true;
        }

        private void UpdateRelicUI(List<RelicName> relicNames)
        {
            var characterHaveRelic = relicNames is { Count: > 0 };
            
            relicPanel.SetActive(characterHaveRelic);
            if (characterHaveRelic)
            {
                var intiRelicName = relicNames[0];
                var relicInfo = GameManager.Instance.RelicManager.GetRelicInfo(intiRelicName);
                var relicData = relicInfo.data;
                var relicLevel = relicInfo.relicSaveInfo.Level;
            
                relicImage.sprite = relicData.IconSprite;
                relicName.text = relicData.Title;
                relicDescription.text = relicData.GetDescription(relicLevel);
            }
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