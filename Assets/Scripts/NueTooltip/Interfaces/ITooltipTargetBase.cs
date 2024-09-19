using NueGames.NueDeck.ThirdParty.NueTooltip.Core;
using NueGames.NueDeck.ThirdParty.NueTooltip.CursorSystem;
using UnityEngine;

namespace NueGames.NueDeck.ThirdParty.NueTooltip.Interfaces
{
    public interface ITooltipTargetBase
    {
        void ShowTooltipInfo(string content,string header ="",Transform tooltipStaticTransform = null,CursorType targetCursor = CursorType.Default,Camera cam = null, float delayShow =0);

        void HideTooltipInfo();
    }
}