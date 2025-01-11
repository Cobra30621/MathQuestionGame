using Managers;
using UnityEngine;

namespace UI
{
    public class CanvasBase : MonoBehaviour
    {
        protected GameManager GameManager => GameManager.Instance;
        protected UIManager UIManager => UIManager.Instance;
        public virtual void OpenCanvas()
        {
            gameObject.SetActive(true);
        }


        public virtual void CloseCanvas()
        {
            gameObject.SetActive(false);
        }

        public virtual void ResetCanvas()
        {
            
        }
    }
}