using Feedback;
using UnityEngine;

namespace Economy
{
    /// <summary>
    /// 播放金錢特效
    /// </summary>
    public class CoinFeedback : MonoBehaviour
    {
        [SerializeField] private IFeedback gainCoinFeedback;
        
        public void PlayGainCoin()
        {
            gainCoinFeedback.Play();
        }
    }
}