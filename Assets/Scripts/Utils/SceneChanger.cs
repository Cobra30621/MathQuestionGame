using System;
using System.Collections;
using Managers;
using NueGames.Managers;
using NueGames.NueDeck.ThirdParty.NueTooltip.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NueGames.Utils
{
    
    public class SceneChanger : MonoBehaviour
    {
        private GameManager GameManager => GameManager.Instance;
        private UIManager UIManager => UIManager.Instance;
        

        
        public void OpenMainMenuScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.MainMenu));
        }

        
        public void OpenMapScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.Map));
        }
        public void OpenCombatScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.Combat));
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
            yield return UIManager.Fade(true);
            UIManager.ChangeScene(type);
        }

        public void ExitApp()
        {
            Application.Quit();
        }
    }
}
