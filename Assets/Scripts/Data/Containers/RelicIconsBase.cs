using NueGames.Data.Containers;
using NueGames.NueDeck.ThirdParty.NueTooltip.Core;
using NueGames.NueDeck.ThirdParty.NueTooltip.CursorSystem;
using NueGames.NueDeck.ThirdParty.NueTooltip.Interfaces;
using NueGames.Relic;
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

        public RelicClip RelicClip;

        public Image StatusImage => statusImage;

        public TextMeshProUGUI StatusValueText => statusValueText;

        public void SetRelicClip(RelicClip relicClip)
        {
            RelicClip = relicClip;
            StatusImage.sprite = RelicClip.Data.IconSprite;

            // 當 counter 發生變化時，改變 UI 顯示
            if (relicClip.Relic.UseCounter)
            {
                relicClip.Relic.OnCounterChange += SetValue;
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
            ShowTooltipInfo(TooltipManager.Instance, RelicClip.Data.GetContent() , RelicClip.Data.GetHeader());
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