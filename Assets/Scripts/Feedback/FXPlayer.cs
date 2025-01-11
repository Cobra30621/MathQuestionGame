using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Feedback
{
    public class FXPlayer :  MonoBehaviour
    {
        [LabelText("使用 MMF Player")]
        public bool useMMF_Player;
        
        [ShowIf("@useMMF_Player==false")]
        [LabelText("特效清單")]
        public List<GameObject> fxs;

        [ShowIf("useMMF_Player")]
        [ValidateInput("HaveMMF_PlayerWhenUse", "請放入 MMF Player")]
        [LabelText("播放的特效")]
        [InfoBox("注意：請將要播放的粒子特效的 gameObject 先關掉，在 MMF_Player 要播放時再打開")]
        public MMF_Player mmfPlayer;
        
        public void Play()
        {
            if (useMMF_Player)
            {
                mmfPlayer.PlayFeedbacks();
            }
        }
        

        [ShowInInspector]
        public bool IsPlaying()
        {
            if (useMMF_Player)
            {
                return mmfPlayer.IsPlaying;
            }
            else
            {
                foreach (var fx in fxs)
                {
                    if (fx != null)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        #region Validate

        private bool HaveMMF_PlayerWhenUse(MMF_Player mmfPlayer)
        {
            if (useMMF_Player)
            {
                return mmfPlayer != null;
            }

            return true;
        }

        #endregion
    }
}