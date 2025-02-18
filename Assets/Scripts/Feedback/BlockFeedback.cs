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
        
        [SerializeField] private GameObject gainBlockFeedbackPrefab;
        [SerializeField] private Transform powerFeedbackSpawn;
        
        /// <summary>
        /// 當格擋值發生變化時播放動畫和音效
        /// </summary>
        /// <param name="amount">新的格擋值</param>
        [Button]
        public void PlayBlockEffect(int amount, bool isNew, bool isNegative)
        {
            // 格檔值為 0，清除格檔
            if (amount <= 0)
            {
                PlayRemoveBlock();
            }
            // 從 0 獲得格檔
            else if (isNew)
            {
                PlayGainNewBlock(amount);
            }
            // 格檔值變更
            else
            {
                PlayBlockUpdate(amount, isNegative);
            }
        }
        
        /// <summary>
        /// 當獲得新的格擋值時播放動畫和音效
        /// </summary>
        /// <param name="amount">獲得的格擋值</param>
        private void PlayGainNewBlock(int amount)
        {
            ShowBlockIcon();
            animator.SetTrigger(GainNewBlock);
            UpdateBlockText(amount);
            AudioManager.Instance.PlaySound("GainBlock");
        }

        /// <summary>
        /// 當格擋值更新時播放動畫和音效
        /// </summary>
        /// <param name="amount">減少後的格擋值</param>
        private void PlayBlockUpdate(int amount, bool isNegative)
        {
            animator.SetTrigger(BlockChange);
            UpdateBlockText(amount);
            AudioManager.Instance.PlaySound(isNegative ? "RemoveBlock" : "GainBlock");
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
            Instantiate(gainBlockFeedbackPrefab, powerFeedbackSpawn);
        }

        private void UpdateBlockText(int amount)
        {
            blockText.text = amount.ToString();
        }
    }
}