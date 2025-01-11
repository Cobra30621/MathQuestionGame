using Managers;
using NueGames.NueDeck.ThirdParty.NueTooltip.Interfaces;
using NueGames.Relic;
using NueTooltip.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NueGames.UI
{
    public class RelicIconsBase : MonoBehaviour,  ITooltipTargetBase, IPointerEnterHandler,IPointerExitHandler
    {
        [SerializeField] private Image statusImage;
        [SerializeField] private TextMeshProUGUI statusValueText;

        public RelicBase Relic;

        
        public Image StatusImage => statusImage;

        public TextMeshProUGUI StatusValueText => statusValueText;

        public TooltipManager TooltipManager => GameManager.Instance.TooltipManager;
        

        public void SetRelicClip( RelicBase relicBase)
        {
            Relic = relicBase;
            
            StatusImage.sprite = Relic.RelicInfo.data.IconSprite;
            // 當 counter 發生變化時，改變 UI 顯示
            if (Relic.UseCounter)
            {
                Relic.OnCounterChange += SetValue;
            }
            else
            {
                StatusValueText.text = "";
            }
            
        }

        private void SetValue(int counter)
        {
            StatusValueText.text = counter.ToString();
        }




        #region ToolTip

        public void OnPointerEnter(PointerEventData eventData)
        {
            var description = Relic.RelicInfo.GetDescription(); 
            ShowTooltipInfo(description , 
                Relic.RelicInfo.data.Title);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideTooltipInfo();
        }
        
        public void ShowTooltipInfo(string content, string header = "",
            Transform tooltipStaticTransform = null, Camera cam = null,
            float delayShow = 0)
        {
            TooltipManager.ShowTooltip(content,header,tooltipStaticTransform, cam,delayShow);
        }

        public void HideTooltipInfo()
        {
            TooltipManager.HideTooltip();
        }

        #endregion
    }
}