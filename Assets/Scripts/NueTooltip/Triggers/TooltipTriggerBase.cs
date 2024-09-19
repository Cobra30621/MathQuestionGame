using NueGames.NueDeck.ThirdParty.NueTooltip.Core;
using NueGames.NueDeck.ThirdParty.NueTooltip.CursorSystem;
using NueGames.NueDeck.ThirdParty.NueTooltip.Interfaces;
using NueTooltip.Core;
using UnityEngine;

namespace NueGames.NueDeck.ThirdParty.NueTooltip.Triggers
{
    public abstract class TooltipTriggerBase : MonoBehaviour, ITooltipTargetBase
    {
        
        [SerializeField] protected string headerText = "";
        [TextArea] [SerializeField] protected string contentText;
        [SerializeField] private Transform tooltipStaticTargetTransform;
        [SerializeField] private CursorType cursorType = CursorType.Default;
        [SerializeField] private float delayShowDuration =0;
        
        protected virtual void ShowTooltipInfo()
        {
            ShowTooltipInfo(contentText,headerText,tooltipStaticTargetTransform,cursorType,delayShow : delayShowDuration);
        }

        public void ShowTooltipInfo( string content, string header = "", Transform tooltipStaticTransform = null,CursorType targetCursor = CursorType.Default,Camera cam = null, float delayShow =0)
        {
            TooltipManager.Instance.ShowTooltip(content,header,tooltipStaticTransform,targetCursor,cam,delayShow);
        }

        public virtual void HideTooltipInfo()
        {
            TooltipManager.Instance.HideTooltip();
        }
    }
}