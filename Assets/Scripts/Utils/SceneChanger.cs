using System;
using System.Collections;
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
        
        private enum SceneType
        {
            MainMenu,
            Map,
            Combat
        }
        public void OpenMainMenuScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.MainMenu));
        }
        private IEnumerator DelaySceneChange(SceneType type)
        {
            UIManager.SetCanvas(UIManager.Instance.InventoryCanvas,false,true);
            yield return StartCoroutine(UIManager.Instance.Fade(true));

            switch (type)
            {
                case SceneType.MainMenu:
                    SaveManager.Instance.SaveGame();
                    UIManager.ChangeScene(GameManager.SceneData.mainMenuSceneIndex);
                    UIManager.SetCanvas(UIManager.CombatCanvas,false,true);
                    UIManager.SetCanvas(UIManager.InformationCanvas,false,false);
                    UIManager.SetCanvas(UIManager.RewardCanvas,false,true);
                    UIManager.SetCanvas(UIManager.RelicCanvas, false, false);
                   
                    break;
                case SceneType.Map:
                    UIManager.ChangeScene(GameManager.SceneData.mapSceneIndex);
                    UIManager.SetCanvas(UIManager.CombatCanvas,false,true);
                    UIManager.SetCanvas(UIManager.InformationCanvas,true,false);
                    UIManager.SetCanvas(UIManager.RewardCanvas,false,true);
                    UIManager.SetCanvas(UIManager.RelicCanvas, true, true);
                   
                    break;
                case SceneType.Combat:
                    UIManager.ChangeScene(GameManager.SceneData.combatSceneIndex);
                    UIManager.SetCanvas(UIManager.CombatCanvas,false,true);
                    UIManager.SetCanvas(UIManager.InformationCanvas,true,false);
                    UIManager.SetCanvas(UIManager.RewardCanvas,false,true);
                    UIManager.SetCanvas(UIManager.RelicCanvas, true, true);
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        public void OpenMapScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.Map));
        }
        public void OpenCombatScene()
        {
            StartCoroutine(DelaySceneChange(SceneType.Combat));
        }
        public void ChangeScene(int sceneId)
        {

            if (sceneId == GameManager.SceneData.mainMenuSceneIndex)
                OpenMainMenuScene();
            else if (sceneId == GameManager.SceneData.mapSceneIndex)
                OpenMapScene();
            else if (sceneId == GameManager.SceneData.combatSceneIndex)
                OpenCombatScene();
            else
                SceneManager.LoadScene(sceneId);
            
            TooltipManager.Instance.HideTooltip();
        }
        public void ExitApp()
        {
            Application.Quit();
        }
    }
}
