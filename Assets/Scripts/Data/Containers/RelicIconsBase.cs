using NueGames.Data.Containers;
using NueGames.NueDeck.ThirdParty.NueTooltip.Core;
using NueGames.NueDeck.ThirdParty.NueTooltip.CursorSystem;
using NueGames.NueDeck.ThirdParty.NueTooltip.Interfaces;
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

        public RelicData MyRelicData { get; private set; } = null;

        public Image StatusImage => statusImage;

        public TextMeshProUGUI StatusValueText => statusValueText;

        public void SetData(RelicData relicData)
        {
            MyRelicData = relicData;
            StatusImage.sprite = relicData.IconSprite;
            
        }

        public void SetValue(int statusValue)
        {
            StatusValueText.text = statusValue.ToString();
        }




        #region ToolTip

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowTooltipInfo(TooltipManager.Instance,MyRelicData.GetContent() ,MyRelicData.GetHeader());
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