using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Feedback
{
    [RequireComponent(typeof(MMF_Player))]
    public class FeelFeedback :  IFeedback
    {
        private MMF_Player _mmfPlayer;

        private void Awake()
        {
            _mmfPlayer = GetComponent<MMF_Player>();
        }

        public override void Play()
        {
            _mmfPlayer.PlayFeedbacks();
        }
    }
}