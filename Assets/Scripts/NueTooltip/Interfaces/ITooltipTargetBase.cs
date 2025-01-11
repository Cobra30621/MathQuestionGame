using UnityEngine;

namespace NueGames.NueDeck.ThirdParty.NueTooltip.Interfaces
{
    public interface ITooltipTargetBase
    {
        void ShowTooltipInfo(string content,string header ="",Transform tooltipStaticTransform = null,Camera cam = null, float delayShow =0);

        void HideTooltipInfo();
    }
}