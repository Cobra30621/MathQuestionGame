using Combat;
using Managers;
using NueGames.Managers;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

namespace NueGames.UI
{
    public class CanvasBase : MonoBehaviour
    {
        
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
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