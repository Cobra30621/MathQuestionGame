using System;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Feedback
{
    [RequireComponent(typeof(MMF_Player))]
    public class FeelFeedback : IFeedback
    {
        private MMF_Player _mmfPlayer;

        private void Awake()
        {
            _mmfPlayer = GetComponent<MMF_Player>();
        }

        public override float FeedbackDuration()
        {
            return _mmfPlayer.TotalDuration;
        }

        public override void Play()
        {
            try
            {
                _mmfPlayer.Initialization();
                _mmfPlayer.PlayFeedbacks();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

        }
    }
}