using System.Collections;
using Managers;
using UI;
using UnityEngine;

namespace Utils
{
    
    public class SceneChanger : MonoBehaviour
    {
        private GameManager GameManager => GameManager.Instance;
        private UIManager UIManager => UIManager.Instance;
        

        
        public void OpenMainMenuScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.MainMenu));
        }

        
        public IEnumerator OpenMapScene()
        {
            yield return DelaySceneChange(SceneType.Map);
        }
        public IEnumerator OpenCombatScene()
        {
            yield return DelaySceneChange(SceneType.Combat);
        }

        public void OpenCompleteMapScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.CompleteMap));
        }

        public void OpenWinMapScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.Win));
        }
        
        private IEnumerator DelaySceneChange(SceneType type)
        {
            yield return UIManager.ChangeScene(type);
        }

        public void ExitApp()
        {
            Application.Quit();
        }
    }
}
