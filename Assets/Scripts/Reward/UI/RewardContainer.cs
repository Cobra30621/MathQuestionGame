using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Reward.UI
{
    public class RewardContainer : MonoBehaviour
    {
        [SerializeField] private Image rewardImage;
        [SerializeField] private TextMeshProUGUI rewardText;


        public void BuildReward(Sprite rewardSprite,string rewardDescription)
        {
            rewardImage.sprite = rewardSprite;
            rewardText.text = rewardDescription;
        }
        
    }
}