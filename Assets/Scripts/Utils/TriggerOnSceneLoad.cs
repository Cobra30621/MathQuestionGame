using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class TriggerOnSceneLoad : MonoBehaviour
    {
        public UnityEvent onLoad;
      
        private void Start()
        {
            onLoad?.Invoke();
        }
    }
}
