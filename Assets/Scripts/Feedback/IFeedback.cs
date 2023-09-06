using Sirenix.OdinInspector;
using UnityEngine;

namespace Feedback
{
    public abstract class IFeedback  : MonoBehaviour
    {
        public abstract float FeedbackDuration();
        
        public abstract void Play();
    }
}