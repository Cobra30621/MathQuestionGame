using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combat;
using NueGames.Data.Containers;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.UI;
using NueGames.NueDeck.ThirdParty.NueTooltip.Core;
using NueGames.NueDeck.ThirdParty.NueTooltip.CursorSystem;
using NueGames.NueDeck.ThirdParty.NueTooltip.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NueGames.Combat;
using NueGames.Power;
using NueTooltip.Core;

namespace NueGames.Characters
{
    [RequireComponent(typeof(Canvas))]
    public abstract class CharacterCanvas : MonoBehaviour, I2DTooltipTarget
    {
        [Header("References")]
        [SerializeField] protected Transform statusIconRoot;
        [SerializeField] protected Transform highlightRoot;
        [SerializeField] protected Transform descriptionRoot;
        [SerializeField] protected PowersData powersData;
        [SerializeField] protected GameObject intentionGO;
        [SerializeField] protected TextMeshProUGUI currentHealthText;
        [SerializeField] protected Image currentHealthBar;
        
        #region Cache

        protected Dictionary<PowerName, PowerIconsBase> StatusDict = new Dictionary<PowerName, PowerIconsBase>();

        protected Canvas TargetCanvas;


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
        public void ApplyStatus(PowerName targetPower, int value)
        {
            if (!StatusDict.ContainsKey(targetPower))
            {
                var targetData = powersData.GetPowerData(targetPower);
                var clone = Instantiate(powersData.PowerBasePrefab, statusIconRoot);
                clone.SetStatus(targetData);
                StatusDict.Add(targetPower, clone);
            }
            
            StatusDict[targetPower].SetStatusValue(value);
        }

        public void ClearStatus(PowerName targetPower)
        {
            if (StatusDict.ContainsKey(targetPower))
            {
                if(StatusDict[targetPower] != null)
                    Destroy(StatusDict[targetPower].gameObject);
                
                StatusDict.Remove(targetPower);
            }
        }
        
        public void UpdateStatusText(PowerName targetPower, int value)
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
        private Coroutine tooltipCoroutine;

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltipCoroutine = StartCoroutine(ShowTooltipAfterDelay(0.5f)); // 延遲 0.5 秒顯示提示
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (tooltipCoroutine != null)
            {
                StopCoroutine(tooltipCoroutine); // 停止顯示提示的協程
                tooltipCoroutine = null;
            }
            HideTooltipInfo();
        }

        private IEnumerator ShowTooltipAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            ShowTooltipInfo();
            
        }

        #endregion

        #region Tooltip
        public virtual void ShowTooltipInfo()
        {
            ShowPowerTooltipInfo();
        }

        protected void ShowPowerTooltipInfo()
        {
            foreach (var powerIconBase in StatusDict)
            {
                PowerData powerData = powerIconBase.Value.MyPowerData;
                ShowTooltipInfo(powerData.GetContent(),powerData.GetHeader(),descriptionRoot);
            }
        }
        
        public void ShowTooltipInfo(string content, string header = "", Transform tooltipStaticTransform = null, CursorType targetCursor = CursorType.Default,Camera cam = null, float delayShow =0)
        {
            TooltipManager.Instance.ShowTooltip(content,header,tooltipStaticTransform,targetCursor,cam,delayShow);
        }

        public void HideTooltipInfo()
        {
            TooltipManager.Instance.HideTooltip();
        }
        

        #endregion
       
    }
}