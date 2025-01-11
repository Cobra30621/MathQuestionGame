using Aduio;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Feedback
{
    public class BlockFeedback : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI blockText;
        
        [SerializeField] private Animator animator;
        private static readonly int BlockChange = Animator.StringToHash("BlockChange");
        private static readonly int GainNewBlock = Animator.StringToHash("GainNewBlock");
        private static readonly int RemoveBlock = Animator.StringToHash("RemoveBlock");
        
        [SerializeField] protected GameObject gainBlockFeedbackPrefab;
        [SerializeField] protected Transform powerFeedbackSpawn;
        
        /// <summary>
        /// 當格擋值發生變化時播放動畫和音效
        /// </summary>
        /// <param name="amount">新的格擋值</param>
        [Button]
        public void PlayBlockChange(int amount)
        {
            animator.SetTrigger(BlockChange);
            blockText.text = amount.ToString();
            AudioManager.Instance.PlaySound("GainBlock");

            ShowBlockIcon();
        }
        
        /// <summary>
        /// 當獲得新的格擋值時播放動畫和音效
        /// </summary>
        /// <param name="amount">獲得的格擋值</param>
        [Button]
        public void PlayGainBlock(int amount)
        {
            ShowBlockIcon();
            
            animator.SetTrigger(GainNewBlock);
            blockText.text = amount.ToString();
            AudioManager.Instance.PlaySound("GainBlock");
        }

        /// <summary>
        /// 當格擋值減少時播放動畫和音效
        /// </summary>
        /// <param name="amount">減少後的格擋值</param>
        [Button]
        public void PlayReduceBlock(int amount)
        {
            animator.SetTrigger(BlockChange);
            blockText.text = amount.ToString();
            AudioManager.Instance.PlaySound("RemoveBlock");
        }

        /// <summary>
        /// 當格擋值被完全移除時播放動畫和音效
        /// </summary>
        [Button]
        public void PlayRemoveBlock()
        {
            animator.SetTrigger(RemoveBlock);
            AudioManager.Instance.PlaySound("RemoveBlock");
        }

        /// <summary>
        /// 在指定位置生成格擋圖標特效
        /// </summary>
        [Button]
        private void ShowBlockIcon()
        {
            var feelFeedback = Instantiate(gainBlockFeedbackPrefab, powerFeedbackSpawn);
        }
    }
}