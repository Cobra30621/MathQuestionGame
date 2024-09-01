using MoreMountains.Tools;
using NueGames.Data.Containers;
using NueGames.NueDeck.ThirdParty.NueTooltip.Core;
using NueGames.NueDeck.ThirdParty.NueTooltip.CursorSystem;
using NueGames.NueDeck.ThirdParty.NueTooltip.Interfaces;
using NueGames.Relic;
using Relic;
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
            ShowTooltipInfo(TooltipManager.Instance, description , 
                Relic.RelicInfo.data.Title);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideTooltipInfo(TooltipManager.Instance);
        }
        
        public void ShowTooltipInfo(TooltipManager tooltipManager, string content, string header = "",
            Transform tooltipStaticTransform = null, CursorType targetCursor = CursorType.Default, Camera cam = null,
            float delayShow = 0)
        {
            tooltipManager.ShowTooltip(content,header,tooltipStaticTransform,targetCursor,cam,delayShow);
        }

        public void HideTooltipInfo(TooltipManager tooltipManager)
        {
            TooltipManager.Instance.HideTooltip();
        }

        #endregion
    }
}