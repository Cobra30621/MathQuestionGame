using System.Collections;
using NueGames.Data.Containers;
using NueGames.NueDeck.ThirdParty.NueTooltip.Interfaces;
using NueTooltip.Core;
using Power;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Characters.Display
{
    public class CharacterTooltipDisplay : MonoBehaviour , I2DTooltipTarget
    {
        [Required]
        [SerializeField]
        private CharacterBase _characterBase;

        [Required]
        [SerializeField] private PowersData _powersData;
        


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
           // 顯示能力的提示
            foreach (var (powerName, power) in  _characterBase.GetPowerDict())
            {
                PowerData powerData = _powersData.GetPowerData(powerName);
                // 顯示能力層數
                int amount = power.Amount;
                var title = $"{powerData.GetHeader()} : {amount}";
                if (powerData.HideAmount)
                {
                    title = powerData.GetHeader();
                }
                ShowTooltipInfo(powerData.GetContent(), title);
            }
        }
        
        public void ShowTooltipInfo(string content, string header = "", Transform tooltipStaticTransform = null, Camera cam = null, float delayShow =0)
        {
            TooltipManager.Instance.ShowTooltip(content,header,tooltipStaticTransform, cam,delayShow);
        }

        public void HideTooltipInfo()
        {
            TooltipManager.Instance.HideTooltip();
        }
        

        #endregion
    }
}