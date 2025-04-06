using NueGames.NueDeck.ThirdParty.NueTooltip.Interfaces;
using NueTooltip.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Reward.UI
{
    public class RewardContainer : MonoBehaviour , ITooltipTargetBase, IPointerEnterHandler,IPointerExitHandler
    {
        [SerializeField] private Image rewardImage;
        [SerializeField] private TextMeshProUGUI rewardText;

        [SerializeField] private bool needShowTooltip = false;
        [SerializeField] private string tooltipTitle;
        [SerializeField] private string tooltipDescription;
        

        public void BuildReward(Sprite rewardSprite,string rewardDescription)
        {
            rewardImage.sprite = rewardSprite;
            rewardText.text = rewardDescription;
            
        }

        public void NeedShowToolTip(string title, string description)
        {
            tooltipTitle = title;
            tooltipDescription = description;
            needShowTooltip = true;
        }
        
        #region ToolTip

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (needShowTooltip)
            {
                ShowTooltipInfo(tooltipDescription , 
                    tooltipTitle);
            }
            
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (needShowTooltip)
            {
                HideTooltipInfo();
            }
            
        }
        
        public void ShowTooltipInfo(string content, string header = "",
            Transform tooltipStaticTransform = null, Camera cam = null,
            float delayShow = 0)
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