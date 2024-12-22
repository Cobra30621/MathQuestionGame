using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

namespace Feedback
{
    public class BlockFeedback : MonoBehaviour
    {
        [SerializeField]
        private MMF_Player gainBlockPrefab;
        
        [SerializeField] private Transform powerFeedbackSpawn;

        public void PlayGainBlock()
        {
            // Show Block Bar
            
            var powerFeedback = Instantiate(gainBlockPrefab, powerFeedbackSpawn);
            powerFeedback.PlayFeedbacks();
        }

        public void PlayRemoveBlock()
        {
            
        }
    }
}