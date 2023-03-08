using System;
using System.Collections.Generic;
using System.Linq;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.UI;
using NueGames.NueDeck.ThirdParty.NueTooltip.Core;
using NueGames.NueDeck.ThirdParty.NueTooltip.CursorSystem;
using NueGames.NueDeck.ThirdParty.NueTooltip.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NueGames.NueDeck.Scripts.Characters
{
    [RequireComponent(typeof(Canvas))]
    public abstract class CharacterCanvas : MonoBehaviour,I2DTooltipTarget
    {
        [Header("References")]
        [SerializeField] protected Transform statusIconRoot;
        [SerializeField] protected Transform highlightRoot;
        [SerializeField] protected Transform descriptionRoot;
        [SerializeField] protected PowersData powersData;
        [SerializeField] protected TextMeshProUGUI currentHealthText;
        [SerializeField] protected Image currentHealthBar;
        
        #region Cache

        protected Dictionary<PowerType, PowerIconsBase> StatusDict = new Dictionary<PowerType, PowerIconsBase>();

        protected Canvas TargetCanvas;
        
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        protected UIManager UIManager => UIManager.Instance;

        #endregion
        
        #region Setup

        public void InitCanvas()
        {
            highlightRoot.gameObject.SetActive(false);

            TargetCanvas = GetComponent<Canvas>();

            if (TargetCanvas)
                TargetCanvas.worldCamera = Camera.main;
        }

        #endregion
        
        #region Public Methods
        public void ApplyStatus(PowerType targetPower, int value)
        {
            if (!StatusDict.ContainsKey(targetPower))
            {
                var targetData = powersData.PowerList.FirstOrDefault(x => x.PowerType == targetPower);
                if (targetData == null)
                {
                    Debug.LogError($"找不到 Power {targetPower} 的 powerData" +
                                   $"請去 Assets/NueGames/NueDeck/Data/Containers/Powers.asset設定");
                    return;
                }
                
                var clone = Instantiate(powersData.PowerBasePrefab, statusIconRoot);
                clone.SetStatus(targetData);
                StatusDict.Add(targetPower, clone);
            }
            
            StatusDict[targetPower].SetStatusValue(value);
        }

        public void ClearStatus(PowerType targetPower)
        {
            if (StatusDict.ContainsKey(targetPower))
            {
                Destroy(StatusDict[targetPower].gameObject);
                StatusDict.Remove(targetPower);
            }
        }
        
        public void UpdateStatusText(PowerType targetPower, int value)
        {
            if (!StatusDict.ContainsKey(targetPower)) return;
          
            StatusDict[targetPower].StatusValueText.text = $"{value}";
        }

        public void UpdateHealthInfo(int currentHealth, int maxHealth)
        {
            currentHealthText.text = $"{currentHealth}/{maxHealth}";
            currentHealthBar.fillAmount = (float)currentHealth / maxHealth;
        }  
        public void SetHighlight(bool open) => highlightRoot.gameObject.SetActive(open);
       
        #endregion

        #region Pointer Events
        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowTooltipInfo();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideTooltipInfo(TooltipManager.Instance);
        }

        #endregion

        #region Tooltip
        public virtual void ShowTooltipInfo()
        {
            ShowPowerTooltipInfo();
        }

        protected void ShowPowerTooltipInfo()
        {
            var tooltipManager = TooltipManager.Instance;

            foreach (var powerIconBase in StatusDict)
            {
                PowerData powerData = powerIconBase.Value.MyPowerData;
                ShowTooltipInfo(tooltipManager,powerData.GetContent(),powerData.GetHeader(),descriptionRoot);
            }
        }
        
        public void ShowTooltipInfo(TooltipManager tooltipManager, string content, string header = "", Transform tooltipStaticTransform = null, CursorType targetCursor = CursorType.Default,Camera cam = null, float delayShow =0)
        {
            tooltipManager.ShowTooltip(content,header,tooltipStaticTransform,targetCursor,cam,delayShow);
        }

        public void HideTooltipInfo(TooltipManager tooltipManager)
        {
            tooltipManager.HideTooltip();
        }
        

        #endregion
       
    }
}